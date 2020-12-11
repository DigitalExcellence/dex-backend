using MessageBrokerPublisher.Services;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBrokerPublisher
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
        /// <param name="payload"></param>
        /// <param name="subject"></param>
        void RegisterNotification(string payload, Subject subject);
    }

    /// <summary>
    /// This class is responsible for registering notifications to the messagebroker.
    /// </summary>
    public class NotificationSender : INotificationSender
    {
       

        IConnection connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationSender"/> class.
        /// </summary>
        public NotificationSender(IRabbitMQConnectionFactory connectionFactory)
        {
            connection = connectionFactory.CreateConnection();
            
        }
        

        /// <summary>
        /// Registers a specified message to a specified queue on the messagebroker.
        /// </summary>
        /// <param name="notification">The notification to be registered.</param>
        public void RegisterNotification(string payload, Subject subject)
        {
            string subjectString = subject.ToString();
            IModel channel = connection.CreateModel();
            channel.QueueDeclare(queue: subjectString, durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            byte[] body = Encoding.UTF8.GetBytes(payload);
            IBasicProperties properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            channel.BasicPublish(exchange: "", routingKey: subjectString, false, basicProperties: properties, body: body);
        }
    }
}
