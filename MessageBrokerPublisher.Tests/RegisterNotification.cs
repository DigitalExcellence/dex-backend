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

using MessageBrokerPublisher.Models;
using MessageBrokerPublisher.Services;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using RabbitMQ.Client;
using System;
using System.Text;

namespace MessageBrokerPublisher.Tests
{

    public class Tests
    {

        [Test]
        public void RegisterTask_EmailNotification_NotificationRegistered()
        {
            // Arrange
            EmailNotificationRegister notification =
                new EmailNotificationRegister("test@example.com", "plain text content");
            string payload = JsonConvert.SerializeObject(notification);
            byte[] body = Encoding.UTF8.GetBytes(payload);

            Mock<IBasicProperties> basicPropertiesMock = new Mock<IBasicProperties>();
            basicPropertiesMock.Setup(x => x.Persistent)
                               .Verifiable();
            Mock<IModel> modelMock = new Mock<IModel>();
            modelMock.Setup(x => x.QueueDeclare(It.IsAny<string>(),
                                                It.Is<bool>(x => x),
                                                It.Is<bool>(x => x == false),
                                                It.Is<bool>(x => x == false),
                                                null))
                     .Verifiable();

            modelMock.Setup(x => x.BasicQos(It.Is<uint>(x => x == 0),
                                            It.Is<ushort>(x => x == 1),
                                            It.Is<bool>(x => x == false)))
                     .Verifiable();

            modelMock.Setup(x => x.CreateBasicProperties())
                     .Returns(basicPropertiesMock.Object)
                     .Verifiable();

            modelMock.Setup(x => x.BasicPublish(It.IsAny<string>(),
                                                It.Is<string>(x => x == Subject.EMAIL.ToString()),
                                                It.Is<bool>(x => x == false),
                                                It.IsAny<IBasicProperties>(),
                                                It.IsAny<ReadOnlyMemory<byte>>()))
                     .Verifiable();

            Mock<IConnection> connectionMock = new Mock<IConnection>();
            connectionMock.Setup(x => x.CreateModel())
                          .Returns(modelMock.Object);

            Mock<IRabbitMQConnectionFactory> connectionFactoryMock = new Mock<IRabbitMQConnectionFactory>();
            connectionFactoryMock.CallBase = true;
            connectionFactoryMock.Setup(x => x.CreateConnection())
                                 .Returns(connectionMock.Object);

            ITaskPublisher taskPublisher = new TaskPublisher(connectionFactoryMock.Object);

            // Act
            taskPublisher.RegisterTask(payload, Subject.EMAIL);

            // Assert
            connectionFactoryMock.Verify();
            connectionMock.Verify();
            modelMock.Verify();
        }

        [Test]
        public void RegisterTask_EmailNotification_WithoutPayload_Throws()
        {
            // Arrange
            EmailNotificationRegister notification =
                new EmailNotificationRegister("test@example.com", "plain text content");
            string payload = JsonConvert.SerializeObject(notification);
            byte[] body = Encoding.UTF8.GetBytes(payload);

            Mock<IBasicProperties> basicPropertiesMock = new Mock<IBasicProperties>();
            basicPropertiesMock.Setup(x => x.Persistent)
                               .Verifiable();
            Mock<IModel> modelMock = new Mock<IModel>();
            modelMock.Setup(x => x.QueueDeclare(It.IsAny<string>(),
                                                It.Is<bool>(x => x),
                                                It.Is<bool>(x => x == false),
                                                It.Is<bool>(x => x == false),
                                                null))
                     .Verifiable();

            modelMock.Setup(x => x.BasicQos(It.Is<uint>(x => x == 0),
                                            It.Is<ushort>(x => x == 1),
                                            It.Is<bool>(x => x == false)))
                     .Verifiable();

            modelMock.Setup(x => x.CreateBasicProperties())
                     .Returns(basicPropertiesMock.Object)
                     .Verifiable();

            modelMock.Setup(x => x.BasicPublish(It.IsAny<string>(),
                                                It.Is<string>(x => x == Subject.EMAIL.ToString()),
                                                It.Is<bool>(x => x == false),
                                                It.IsAny<IBasicProperties>(),
                                                It.IsAny<ReadOnlyMemory<byte>>()))
                     .Verifiable();

            Mock<IConnection> connectionMock = new Mock<IConnection>();
            connectionMock.Setup(x => x.CreateModel())
                          .Returns(modelMock.Object);

            Mock<IRabbitMQConnectionFactory> connectionFactoryMock = new Mock<IRabbitMQConnectionFactory>();
            connectionFactoryMock.CallBase = true;
            connectionFactoryMock.Setup(x => x.CreateConnection())
                                 .Returns(connectionMock.Object);

            ITaskPublisher taskPublisher = new TaskPublisher(connectionFactoryMock.Object);

            // Act + Assert
            Assert.Throws<ArgumentNullException>(() => taskPublisher.RegisterTask(null, Subject.EMAIL));
        }

    }

}
