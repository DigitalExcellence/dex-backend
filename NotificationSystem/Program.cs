using NotificationSystem.Contracts;
using NotificationSystem.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace NotificationSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            // todo change to env variables.
            string hostName = "localhost";
            string user = "guest";
            string password = "guest";

            RabbitMQSubscriber subscriber = new RabbitMQSubscriber(hostName, user, password);
            IModel channel = subscriber.SubscribeToSubject("email");

            RabbitMQListener listener = new RabbitMQListener(channel);
            // inject your notification service here
            INotificationService notificationService = null;
            EventingBasicConsumer consumer = listener.CreateConsumer(notificationService);
            listener.StartConsume(consumer, "email");

        }
    }
}
