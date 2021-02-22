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

using Moq;
using NotificationSystem.Services;
using NUnit.Framework;
using RabbitMQ.Client;

namespace NotificationSystem.Tests
{

    public class RabbitMQSubscriberTests
    {

        [Test]
        public void SubscribeToSubject_Valid_IModel()
        {
            // Arrange
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

            Mock<IConnection> connectionMock = new Mock<IConnection>();
            connectionMock.Setup(x => x.CreateModel())
                          .Returns(modelMock.Object);

            Mock<IRabbitMQConnectionFactory> connectionFactoryMock = new Mock<IRabbitMQConnectionFactory>();
            connectionFactoryMock.CallBase = true;
            connectionFactoryMock.Setup(x => x.CreateConnection())
                                 .Returns(connectionMock.Object);
            RabbitMQSubscriber rabbitMQSubscriber = new RabbitMQSubscriber(connectionFactoryMock.Object);

            // Act
            IModel channel = rabbitMQSubscriber.SubscribeToSubject("EMAIL");

            // Assert
            connectionFactoryMock.Verify();
            connectionMock.Verify();
            modelMock.Verify();
        }

    }

}
