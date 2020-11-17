using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using NotificationSystem.Contracts;
using RabbitMQ.Client.Events;
using Serilog;

namespace NotificationSystem.Services
{
    public interface IRabbitMQListener
    {
        EventingBasicConsumer CreateConsumer(INotificationService notificationService);
        void StartConsume(EventingBasicConsumer consumer, string subject);
    }
    public class RabbitMQListener : IRabbitMQListener
    {
        private IModel channel;

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
                string message = Encoding.UTF8.GetString(body);
                try
                {
                    notificationService.ValidateMessageBody(message);
                    notificationService.SendNotification(message);
                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                } catch(Exception e)
                {
                    Log.Logger.Error(e, "Error sending notification.");
                }
            };
            return consumer;
        }

        public void StartConsume(EventingBasicConsumer consumer, string subject)
        {
            channel.BasicConsume(queue: subject,
                autoAck: false,
                consumer: consumer);
        }
    }
}
