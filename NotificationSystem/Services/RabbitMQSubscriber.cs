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
        private IConnection connection;


        public RabbitMQSubscriber(IRabbitMQConnectionFactory connectionFactory)
        {
            connection = connectionFactory.CreateConnection();
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
