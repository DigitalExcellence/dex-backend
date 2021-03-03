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

namespace NotificationSystem.Services
{

    public interface IRabbitMQConnectionFactory
    {

        IConnection CreateConnection();

    }

    public class RabbitMQConnectionFactory : IRabbitMQConnectionFactory
    {

        private readonly string hostName;
        private readonly string password;
        private readonly string user;


        public RabbitMQConnectionFactory(string hostName, string user, string password)
        {
            this.hostName = hostName;
            this.user = user;
            this.password = password;
        }

        public IConnection CreateConnection()
        {
            ConnectionFactory connectionFactory = CreateConnectionFactory();
            return connectionFactory.CreateConnection();
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

    }

}
