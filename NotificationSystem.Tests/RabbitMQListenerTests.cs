using Moq;
using NotificationSystem.Contracts;
using NotificationSystem.Services;
using NUnit.Framework;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationSystem.Tests
{
    public class RabbitMQListenerTests
    {
        [Test]
        public void SetCallBack_Valid_EventHandlerAdded()
        {
            // Arrange
            string payload = "test";
            byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);
            ReadOnlyMemory<byte> readOnlyMemory = new ReadOnlyMemory<byte>(payloadBytes);
            BasicDeliverEventArgs eventArgs = new BasicDeliverEventArgs(null, 1, false, null, null, null, readOnlyMemory);
            Mock<IModel> modelMock = new Mock<IModel>();
            modelMock.Setup(x => x.BasicAck(eventArgs.DeliveryTag, false)).Verifiable();
            Mock<EventingBasicConsumer> consumerMock = new Mock<EventingBasicConsumer>(modelMock.Object);
            Mock<INotificationService> notificationmock = new Mock<INotificationService>();
            notificationmock.Setup(x => x.ParsePayload(It.Is<string>(x => x == payload))).Verifiable();
            notificationmock.Setup(x => x.ValidatePayload()).Verifiable();
            notificationmock.Setup(x => x.ExecuteTask()).Verifiable();

            RabbitMQListener listener = new RabbitMQListener(modelMock.Object);

            // Act
            listener.CreateConsumer(notificationmock.Object);
            listener.SetCallBack(notificationmock.Object, consumerMock.Object);
            listener.CallBack(notificationmock.Object, eventArgs);

            // Assert
            notificationmock.Verify();
        }

        [Test]
        public void StartConsumer_Valid_BasicConsumeCalled()
        {
            // Arrange
            string subject = "test";
            Mock<IModel> modelMock = new Mock<IModel>();
            Mock<EventingBasicConsumer> consumerMock = new Mock<EventingBasicConsumer>(modelMock.Object);
            
            modelMock.Setup(x => x.BasicConsume(subject, false, "", false, false, null, consumerMock.Object)).Verifiable();
                        
            RabbitMQListener listener = new RabbitMQListener(modelMock.Object);

            // Act
            listener.StartConsumer(consumerMock.Object, subject);

            // Assert
            modelMock.Verify();    
        }
    }
}
