using RabbitMQ.Client;
using System;
using System.Text;
using NotificationSystem.Contracts;
using RabbitMQ.Client.Events;
using Serilog;
using Newtonsoft.Json;
using MessagebrokerPublisher;
using MessagebrokerPublisher.Contracts;

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
            Console.WriteLine("Before creating Consumer");
            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

            consumer.Received += (sender, ea) =>
            {
                
                byte[] body = ea.Body.ToArray();

                // Currently we have only EmailNotification, this should change later to match other types of noticiations
                var notification = JsonConvert.DeserializeObject<EmailNotification>(Encoding.UTF8.GetString(body));

                try
                {
                    if(notificationService != null)
                    {
                        notificationService.SendNotification(notification);
                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                        Console.WriteLine("Delivered");
                    } else
                    {
                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                        Console.WriteLine("Delivered");
                    }
                    
                } catch(Exception e)
                {
                    throw e;
                }
            };
            Console.WriteLine("After creating Consumer");
            return consumer;
        }

        public void StartConsumer(EventingBasicConsumer consumer, string subject)
        {
            Console.WriteLine("Before starting consument");
            channel.BasicConsume(queue: subject,
                autoAck: false,
                consumer: consumer);
            Console.WriteLine("After starting consument");
            Console.ReadLine();
        }
    }
}
