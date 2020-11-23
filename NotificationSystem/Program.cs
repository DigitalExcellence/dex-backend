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
        private static void Main(string[] args)
        {
            //wait for rabbitmq service to start
            Thread.Sleep(10000);
            try
            {
                // todo change to env variables.
                string hostName = "rabbitmq";
                string user = "notificationservice";
                string password = "C6S&jph1VQUv";

                RabbitMQSubscriber subscriber = new RabbitMQSubscriber(hostName, user, password);
                IModel channel = subscriber.SubscribeToSubject("email");

                RabbitMQListener listener = new RabbitMQListener(channel);
                // inject your notification service here
                INotificationService notificationService = null;
                EventingBasicConsumer consumer = listener.CreateConsumer(notificationService);
                listener.StartConsume(consumer, "email");
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }

        }
    }
}
