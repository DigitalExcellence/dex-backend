using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.HelperClasses
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
        void RegisterNotification(string message, string subject);
    }

    /// <summary>
    /// This class is responsible for registering notifications to the messagebroker.
    /// </summary>
    public class NotificationSender : INotificationSender
    {
        // todo change to env variables.
        // docker service name
        private string hostName = "localhost";
        private string user = "notificationservice";
        private string password = "C6S&jph1VQUv";

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
            connectionFactory = new ConnectionFactory();
            connectionFactory.UserName = user;
            connectionFactory.Password = password;
            connectionFactory.HostName = hostName;
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
        public void RegisterNotification(string message, string subject)
        {
            IConnection connection = CreateConnection();
            IModel channel = connection.CreateModel();
            channel.QueueDeclare(queue: subject, durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            Byte[] body = Encoding.UTF8.GetBytes(message);
            IBasicProperties properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            channel.BasicPublish(exchange: "", routingKey: subject, basicProperties: properties, body: body);
        }

        
    }
}
