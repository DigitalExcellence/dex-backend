using RabbitMQ.Client;
using System;
using System.Text;
using NotificationSystem.Contracts;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using NotificationSystem.Notifications;

namespace NotificationSystem.Services
{
    public interface IRabbitMQListener
    {
        EventingBasicConsumer CreateConsumer(INotificationService notificationService);
        void StartConsumer(EventingBasicConsumer consumer, string subject);
    }
    public class RabbitMQListener : IRabbitMQListener
    {
        private readonly IModel channel;

        public RabbitMQListener(IModel channel)
        {
            this.channel = channel;
        }

        public EventingBasicConsumer CreateConsumer(INotificationService notificationService)
        {
            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            SetCallBack(notificationService, consumer);
            return consumer;
        }

        public EventingBasicConsumer SetCallBack(INotificationService notificationService, IBasicConsumer basicConsumer)
        {
            EventingBasicConsumer consumer = (EventingBasicConsumer) basicConsumer;
            consumer.Received += (sender, ea) =>
            {
                CallBack(notificationService, ea);
            };
            return consumer;
        }

        public void CallBack(INotificationService notificationService, BasicDeliverEventArgs ea)
        {
            byte[] body = ea.Body.ToArray();
            string jsonBody = Encoding.UTF8.GetString(body);

            try
            {
                notificationService.ParsePayload(jsonBody);
                notificationService.ValidatePayload();
                notificationService.ExecuteTask();
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                Console.WriteLine("Task executed");
            } catch(Exception e)
            {
                Console.WriteLine("Task failed");
                Console.WriteLine(e.Message);
            }
        }

        public void StartConsumer(EventingBasicConsumer consumer, string subject)
        {
            channel.BasicConsume(queue: subject,
                autoAck: false,
                consumer: consumer);            
        }
    }
}
