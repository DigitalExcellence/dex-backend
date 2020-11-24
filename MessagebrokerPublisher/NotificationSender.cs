using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// <param name="message"></param>
        /// <param name="subject"></param>
        void RegisterNotification(string message, Subject subject);
    }

    /// <summary>
    /// This class is responsible for registering notifications to the messagebroker.
    /// </summary>
    public class NotificationSender : INotificationSender
    {
        // todo change to env variables so that when not ran in docker, it does not crash.
        // docker service name
        private readonly string hostName = "rabbitmq";
        private readonly string user = "notificationservice";
        private readonly string password = "C6S&jph1VQUv";

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
        /// <param name="message">The message to be registered.</param>
        /// <param name="subject">The queue to be registered to.</param>
        public void RegisterNotification(string message, Subject subject)
        {
            string subjectString = subject.ToString();
            IConnection connection = CreateConnection();
            IModel channel = connection.CreateModel();
            channel.QueueDeclare(queue: subjectString, durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            byte[] body = Encoding.UTF8.GetBytes(message);
            IBasicProperties properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            channel.BasicPublish(exchange: "", routingKey: subjectString, basicProperties: properties, body: body);
        }


    }
}
