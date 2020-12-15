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


        public RabbitMQSubscriber(IRabbitMQConnectionFactory connectionFactory)
        {
            connection = connectionFactory.CreateConnection();
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
            IModel channel = connection.CreateModel();
            channel.QueueDeclare(queue: subject,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            return channel;
        }
    }
}
