using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationSystem.Services
{
    public interface IRabbitMQSubscriber
    {
        IModel SubscribeToSubject(string subject);

    }
    public class RabbitMQSubscriber : IRabbitMQSubscriber
    {

        private readonly string hostName;
        private readonly string user;
        private readonly string password;
        private IConnection connection;


        public RabbitMQSubscriber(string hostName, string user, string password)
        {
            this.hostName = hostName;
            this.user = user;
            this.password = password;
            ConnectToMessageBroker();
        }

        private void ConnectToMessageBroker()
        {
            ConnectionFactory factory = new ConnectionFactory
            {
                UserName = user,
                Password = password,
                HostName = hostName
            };
            factory.AutomaticRecoveryEnabled = true;
            connection = factory.CreateConnection();
            Console.WriteLine("Connected with RabbitMQ");
        }

        public IModel SubscribeToSubject(string subject)
        {
            Console.WriteLine("Before subscribe to subject");
            IModel channel = connection.CreateModel();
            channel.QueueDeclare(queue: subject,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            Console.WriteLine("After subscribe to subject");
            return channel;
        }
    }
}
