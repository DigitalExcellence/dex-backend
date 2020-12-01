using Microsoft.Extensions.Configuration;
using NotificationSystem.Configuration;
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
            string environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                .AddEnvironmentVariables()
                .Build();
            Config config = configuration.GetSection("App").Get<Config>();
           

            RabbitMQSubscriber subscriber = new RabbitMQSubscriber(config.RabbitMQ.Hostname, config.RabbitMQ.Username, config.RabbitMQ.Password);
            IModel channel = subscriber.SubscribeToSubject("EMAIL");

            RabbitMQListener listener = new RabbitMQListener(channel);

            // inject your notification service here
            INotificationService notificationService = new EmailSender(config);
            EventingBasicConsumer consumer = listener.CreateConsumer(notificationService);


            listener.StartConsumer(consumer, "EMAIL");
        }
    }
}
