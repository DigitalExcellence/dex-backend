using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessageBrokerPublisher.Services
{
    ///<summary>
    ///Interface for the RabbitMQConnectionFactory.
    ///</summary>
    ///

    public interface IRabbitMQConnectionFactory
    {
        ///<summary>
        /// Method returns a connection
        /// </summary>
        IConnection CreateConnection();

    }

    /// <summary>
    /// This class is responsible for creating and return a connection to the messagebroker.
    /// </summary>
    public class RabbitMQConnectionFactory : IRabbitMQConnectionFactory
    {
        private readonly string hostName;
        private readonly string user;
        private readonly string password;

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMQConnectionFactory"/> class.
        /// </summary>
        public RabbitMQConnectionFactory(string hostName, string user, string password)
        {
            this.hostName = hostName;
            this.user = user;
            this.password = password;
        }


        /// <summary>
        /// Creates a new RabbitMQ Connection Factory
        /// </summary>
        private ConnectionFactory CreateConnectionFactory()
        {
            return new ConnectionFactory
            {
                UserName = user,
                Password = password,
                HostName = hostName
            };
        }
        /// <summary>
        /// Creates a new RabbitMQ Connection and returns it.
        /// </summary>
        public IConnection CreateConnection()
        {
            ConnectionFactory connectionFactory = CreateConnectionFactory();
            return connectionFactory.CreateConnection();
        }

    }
}
