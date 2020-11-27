using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MessagebrokerPublisher.Contracts;

namespace MessagebrokerPublisher
{
    ///<summary>
    ///Interface for the NotificationSender.
    ///</summary>
    ///
    public interface INotificationSender
    {
        /// <summary>
        /// Method deletes the file from the file server
        /// </summary>
        /// <param name="notification"></param>
        void RegisterNotification(INotification notification);
    }

    /// <summary>
    /// This class is responsible for registering notifications to the messagebroker.
    /// </summary>
    public class NotificationSender : INotificationSender
    {
        private readonly string hostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST_NAME");
        private readonly string user = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME");
        private readonly string password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD");

        private ConnectionFactory connectionFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationSender"/> class.
        /// </summary>
        public NotificationSender()
        {
            CreateConnectionFactory();
        }
        private void CreateConnectionFactory()
        {
            connectionFactory = new ConnectionFactory
            {
                UserName = user,
                Password = password,
                HostName = hostName
            };
        }
        private IConnection CreateConnection()
        {
            return connectionFactory.CreateConnection();
        }

        /// <summary>
        /// Registers a specified message to a specified queue on the messagebroker.
        /// </summary>
        /// <param name="notification">The notification to be registered.</param>
        public void RegisterNotification(INotification notification)
        {
            string subjectString = notification.Subject.ToString();
            IConnection connection = CreateConnection();
            IModel channel = connection.CreateModel();
            channel.QueueDeclare(queue: subjectString, durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            byte[] body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(notification));
            IBasicProperties properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            channel.BasicPublish(exchange: "", routingKey: subjectString, basicProperties: properties, body: body);
        }
    }
}
