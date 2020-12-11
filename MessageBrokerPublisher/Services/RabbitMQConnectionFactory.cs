using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessageBrokerPublisher.Services
{
    public interface IRabbitMQConnectionFactory
    {
        IConnection CreateConnection();

    }

    public class RabbitMQConnectionFactory : IRabbitMQConnectionFactory
    {
        private readonly string hostName = Environment.GetEnvironmentVariable("App__RabbitMQ__Hostname");
        private readonly string user = Environment.GetEnvironmentVariable("App__RabbitMQ__Username");
        private readonly string password = Environment.GetEnvironmentVariable("App__RabbitMQ__Password");


        public RabbitMQConnectionFactory()
        {
            if(string.IsNullOrEmpty(hostName))
            {
                hostName = "localhost";
                user = "guest";
                password = "guest";
            }
        }


        private ConnectionFactory CreateConnectionFactory()
        {
            return new ConnectionFactory
            {
                UserName = user,
                Password = password,
                HostName = hostName
            };
        }
        public IConnection CreateConnection()
        {
            ConnectionFactory connectionFactory = CreateConnectionFactory();
            return connectionFactory.CreateConnection();
        }

    }
}
