using Microsoft.Extensions.Configuration;
using NotificationSystem.Contracts;
using NotificationSystem.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace NotificationSystem
{
    internal class Program
    {
        private static void Main()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables(prefix: "YOUR_APP_PREFIX_")
                .Build();


            string hostName;
            string user;
            string password;

            //check for if running in docker, else set values to run with local RabbitMQ service
            if (Environment.GetEnvironmentVariable("RABBITMQ_HOST_NAME") != null)
            {
                hostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST_NAME");
                user = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME");
                password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD");
            } else
            {
                hostName = "localhost";
                user = "guest";
                password = "guest";
            }
            

            RabbitMQSubscriber subscriber = new RabbitMQSubscriber(hostName, user, password);
            IModel channel = subscriber.SubscribeToSubject("EMAIL");

            RabbitMQListener listener = new RabbitMQListener(channel);

            // inject your notification service here
            INotificationService notificationService = new EmailSender();
            EventingBasicConsumer consumer = listener.CreateConsumer(notificationService);


            listener.StartConsumer(consumer, "EMAIL");
        }
    }
}
