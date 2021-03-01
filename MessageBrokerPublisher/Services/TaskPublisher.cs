/*
* Digital Excellence Copyright (C) 2020 Brend Smits
*
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU Lesser General Public License as published
* by the Free Software Foundation version 3 of the License.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty
* of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
* See the GNU Lesser General Public License for more details.
*
* You can find a copy of the GNU Lesser General Public License
* along with this program, in the LICENSE.md file in the root project directory.
* If not, see https://www.gnu.org/licenses/lgpl-3.0.txt
*/

using MessageBrokerPublisher.Services;
using RabbitMQ.Client;
using System;
using System.Text;

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

        private readonly IConnection connection;

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
            channel.QueueDeclare(subjectString, true, false, false, null);
            channel.BasicQos(0, 1, false);

            byte[] body = Encoding.UTF8.GetBytes(payload);
            IBasicProperties properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            channel.BasicPublish("", subjectString, false, properties, body);
            Console.WriteLine("Task published");
        }

    }
}
