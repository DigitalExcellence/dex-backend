using MessagebrokerPublisher;
using NotificationSystem.Contracts;
using NotificationSystem.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System;
using System.Threading;

namespace NotificationSystem
{
    internal class Program
    {
        private static void Main()
        {
            string hostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST_NAME");
            string user = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME");
            string password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD");

            RabbitMQSubscriber subscriber = new RabbitMQSubscriber(hostName, user, password);
            IModel channel = subscriber.SubscribeToSubject(Subject.EMAIL.ToString());

            RabbitMQListener listener = new RabbitMQListener(channel);

            // inject your notification service here
            INotificationService notificationService = new EmailSender();
            EventingBasicConsumer consumer = listener.CreateConsumer(notificationService);
            listener.StartConsumer(consumer, Subject.EMAIL.ToString());
        }
    }
}
