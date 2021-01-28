using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationSystem.Services
{
    public interface IRabbitMQConnectionFactory
    {
        IConnection CreateConnection();

    }

    public class RabbitMQConnectionFactory : IRabbitMQConnectionFactory
    {
        private readonly string hostName;
        private readonly string user;
        private readonly string password;


        public RabbitMQConnectionFactory(string hostName, string user, string password)
        {
            this.hostName = hostName;
            this.user = user;
            this.password = password;
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
