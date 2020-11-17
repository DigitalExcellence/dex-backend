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

        private string hostName;
        private string user;
        private string password;
        private IConnection connection;
        private EventingBasicConsumer consumer;


        public RabbitMQSubscriber(string hostName, string user, string password)
        {
            this.hostName = hostName;
            this.user = user;
            this.password = password;
            ConnectToMessageBroker();
        }

        private void ConnectToMessageBroker()
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.UserName = user;
            factory.Password = password;
            factory.HostName = hostName;
            connection = factory.CreateConnection();
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
