using Moq;
using NotificationSystem.Services;
using NUnit.Framework;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

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
                                                It.Is<bool>(x => x == true),
                                                It.Is<bool>(x => x == false),
                                                It.Is<bool>(x => x == false),
                                                null)).Verifiable();

            modelMock.Setup(x => x.BasicQos(It.Is<uint>(x => x == 0),
                                            It.Is<ushort>(x => x == 1),
                                            It.Is<bool>(x => x == false)
                                            )).Verifiable();
            
            Mock<IConnection> connectionMock = new Mock<IConnection>();
            connectionMock.Setup(x => x.CreateModel()).Returns(modelMock.Object);

            Mock<IRabbitMQConnectionFactory> connectionFactoryMock = new Mock<IRabbitMQConnectionFactory>();
            connectionFactoryMock.CallBase = true;
            connectionFactoryMock.Setup(x => x.CreateConnection()).Returns(connectionMock.Object);
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
