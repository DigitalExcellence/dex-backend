using MessageBrokerPublisher;
using MessageBrokerPublisher.Models;
using MessageBrokerPublisher.Services;
using Moq;
using NUnit.Framework;
using RabbitMQ.Client;
using System;
using System.Text;

namespace MessageBrokerPublisher.Tests
{
    public class Tests
    {
        [Test]
        public void RegisterNotification_EmailNotification_NotificationRegistered()
        {
            // Arrange
            EmailNotificationRegister notification = new EmailNotificationRegister("test@example.com", "plain text content");
            string payload = Newtonsoft.Json.JsonConvert.SerializeObject(notification);
            byte[] body = Encoding.UTF8.GetBytes(payload);

            Mock<IBasicProperties> basicPropertiesMock = new Mock<IBasicProperties>();
            basicPropertiesMock.Setup(x => x.Persistent).Verifiable();
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

            modelMock.Setup(x => x.CreateBasicProperties()).Returns(basicPropertiesMock.Object).Verifiable();

            modelMock.Setup(x => x.BasicPublish(It.IsAny<string>(),
                                                It.Is<string>(x => x == Subject.EMAIL.ToString()),
                                                It.Is<bool>(x => x == false),
                                                It.IsAny<IBasicProperties>(),
                                                It.IsAny<ReadOnlyMemory<byte>>()
                                                )).Verifiable();

            Mock <IConnection> connectionMock = new Mock<IConnection>();
            connectionMock.Setup(x => x.CreateModel()).Returns(modelMock.Object);

            Mock<IRabbitMQConnectionFactory> connectionFactoryMock = new Mock<IRabbitMQConnectionFactory>();
            connectionFactoryMock.CallBase = true;
            connectionFactoryMock.Setup(x => x.CreateConnection()).Returns(connectionMock.Object);

            INotificationSender notificationSender = new NotificationSender(connectionFactoryMock.Object);

            // Act
            notificationSender.RegisterNotification(payload, Subject.EMAIL);

            // Assert
            connectionFactoryMock.Verify();
            connectionMock.Verify();
            modelMock.Verify();
        }

        [Test]
        public void RegisterNotification_EmailNotification_WithoutPayload_Throws()
        {
            // Arrange
            EmailNotificationRegister notification = new EmailNotificationRegister("test@example.com", "plain text content");
            string payload = Newtonsoft.Json.JsonConvert.SerializeObject(notification);
            byte[] body = Encoding.UTF8.GetBytes(payload);

            Mock<IBasicProperties> basicPropertiesMock = new Mock<IBasicProperties>();
            basicPropertiesMock.Setup(x => x.Persistent).Verifiable();
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

            modelMock.Setup(x => x.CreateBasicProperties()).Returns(basicPropertiesMock.Object).Verifiable();

            modelMock.Setup(x => x.BasicPublish(It.IsAny<string>(),
                                                It.Is<string>(x => x == Subject.EMAIL.ToString()),
                                                It.Is<bool>(x => x == false),
                                                It.IsAny<IBasicProperties>(),
                                                It.IsAny<ReadOnlyMemory<byte>>()
                                                )).Verifiable();

            Mock<IConnection> connectionMock = new Mock<IConnection>();
            connectionMock.Setup(x => x.CreateModel()).Returns(modelMock.Object);

            Mock<IRabbitMQConnectionFactory> connectionFactoryMock = new Mock<IRabbitMQConnectionFactory>();
            connectionFactoryMock.CallBase = true;
            connectionFactoryMock.Setup(x => x.CreateConnection()).Returns(connectionMock.Object);

            INotificationSender notificationSender = new NotificationSender(connectionFactoryMock.Object);


            // Act + Assert
            Assert.Throws<ArgumentNullException>(() => notificationSender.RegisterNotification(null, Subject.EMAIL));
        }
    }
}
