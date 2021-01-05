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

            consumer.Received += (sender, ea) =>
            {
                
                byte[] body = ea.Body.ToArray();
                string jsonBody = Encoding.UTF8.GetString(body);

                // Currently we have only EmailNotification, this should change later to match other types of noticiations
                EmailNotification notification = JsonConvert.DeserializeObject<EmailNotification>(jsonBody);

                try
                {                   
                    notificationService.SendNotification(notification);
                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);                    
                } catch(Exception e)
                {
                    throw e;
                }
            };
            return consumer;
        }

        public void StartConsumer(EventingBasicConsumer consumer, string subject)
        {
            channel.BasicConsume(queue: subject,
                autoAck: false,
                consumer: consumer);
            Console.ReadLine();
        }
    }
}
