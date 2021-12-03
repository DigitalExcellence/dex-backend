using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using JobScheduler;
using MessageBrokerPublisher.HelperClasses;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Models;

namespace JobScheduler.Tests
{

    public class GraduationWorkerTests
    {

        [Test]
        public async Task GraduationJob_EmailNotification_NotificationGraduating()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton(Mock.Of<IEmailSender>());
            services.AddSingleton(Mock.Of<IApiRequestHandler>());
            services.AddSingleton(Mock.Of<ILogger<GraduationWorker>>());
            services.AddSingleton(Mock.Of<Config>());
            services.AddHostedService<GraduationWorker>();
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            Mock<IEmailSender> emailSenderMock = Mock.Get(serviceProvider.GetService<IEmailSender>());
            Mock<IApiRequestHandler> apiRequestHandlerMock = Mock.Get(serviceProvider.GetService<IApiRequestHandler>());
            GraduationWorker backgroundService = serviceProvider.GetService<IHostedService>() as GraduationWorker;

            apiRequestHandlerMock.Setup(a => a.GetExpectedGraduationUsersAsync()).Returns(
                Task.FromResult(
                    new List<UserTask>{
                        new UserTask
                        {
                            Id = 1,
                            Status = UserTaskStatus.Mailed,
                            Type = UserTaskType.GraduationReminder,
                            User = new User()
                        }
                    }
                )
            );

            // Act
            await backgroundService?.StartAsync(CancellationToken.None);
            await Task.Delay(22000); // GraduationWorker has a delay of 20000 for starting up API and IdentityServer

            // Assert
            apiRequestHandlerMock.Verify(a => a.GetExpectedGraduationUsersAsync(), Times.Once);

            emailSenderMock.Verify(e => e.Send(
                It.IsAny<string>(),
                It.IsAny<string>(),
                null
            ), Times.Once);

            apiRequestHandlerMock.Verify(a => a.SetGraduationTaskStatusToMailed(It.IsAny<int>()), Times.Once);

            await backgroundService?.StopAsync(CancellationToken.None);
        }
    }
}
