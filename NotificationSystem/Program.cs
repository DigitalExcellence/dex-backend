using Microsoft.Extensions.Configuration;
using NotificationSystem.Configuration;
using NotificationSystem.Contracts;
using NotificationSystem.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SendGrid;
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

            IRabbitMQConnectionFactory connectionFactory = new RabbitMQConnectionFactory(config.RabbitMQ.Hostname, config.RabbitMQ.Username, config.RabbitMQ.Password);

            RabbitMQSubscriber subscriber = new RabbitMQSubscriber(connectionFactory);
            IModel channel = subscriber.SubscribeToSubject("EMAIL");

            RabbitMQListener listener = new RabbitMQListener(channel);

            // inject your notification service here
            ISendGridClient sendGridClient = new SendGridClient(config.SendGrid.ApiKey);
            INotificationService notificationService = new EmailSender(sendGridClient, config.SendGrid.EmailFrom, config.SendGrid.SandboxMode);
            EventingBasicConsumer consumer = listener.CreateConsumer(notificationService);

            listener.StartConsumer(consumer, "EMAIL");
            Console.ReadLine();
        }
    }
}
