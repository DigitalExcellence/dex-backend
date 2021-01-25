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
    ///Interface for the TaskPublisher.
    ///</summary>
    ///
    public interface ITaskPublisher
    {
        /// <summary>
        /// Method deletes the file from the file server
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="subject"></param>
        void RegisterTask(string payload, Subject subject);
    }

    /// <summary>
    /// This class is responsible for registering notifications to the messagebroker.
    /// When working with this class in the API environment, you can request an instance
    /// in the constructor, since it's registered in the IOC and thus can be used with
    /// dependency injection.
    /// When using this class outside of the API, just instantiate a new TaskPublisher,
    /// and pass in a instance of the RabbitMQConnectionFactory class, providing the paramaters:
    /// Hostname, Username and password. For example when running a local instance of RabbitMQ,
    /// these would be respectively: localhost, guest, guest.
    /// </summary>
    public class TaskPublisher : ITaskPublisher
    {
       

        IConnection connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskPublisher"/> class.
        /// </summary>
        public TaskPublisher(IRabbitMQConnectionFactory connectionFactory)
        {
            connection = connectionFactory.CreateConnection();
            
        }
        

        /// <summary>
        /// Registers a specified message to a specified queue on the messagebroker.
        /// </summary>
        /// <param name="payload">The body of the task to be published.</param>
        /// <param name="subject">The subject, which is the channel which to publish to.</param>
        public void RegisterTask(string payload, Subject subject)
        {
            string subjectString = subject.ToString();
            IModel channel = connection.CreateModel();
            channel.QueueDeclare(queue: subjectString, durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            byte[] body = Encoding.UTF8.GetBytes(payload);
            IBasicProperties properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            channel.BasicPublish(exchange: "", routingKey: subjectString, false, basicProperties: properties, body: body);
            Console.WriteLine("Task published");
        }
    }
}
