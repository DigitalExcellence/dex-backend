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

using RabbitMQ.Client;

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
        private readonly string password;
        private readonly string user;

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
        /// Creates a new RabbitMQ Connection and returns it.
        /// </summary>
        public IConnection CreateConnection()
        {
            ConnectionFactory connectionFactory = CreateConnectionFactory();
            return connectionFactory.CreateConnection();
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

    }
}
