using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using JobScheduler;
using MessageBrokerPublisher.HelperClasses;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace JobScheduler.Tests
{

    public class GraduationWorkerTests
    {

        [Test]
        public async Task GraduationJob_EmailNotification_NotificationGraduating()
        {
            // Arrange
            Config config = new Config();

            IServiceCollection services = new ServiceCollection();
            services.AddSingleton(Mock.Of<IEmailSender>());
            services.AddSingleton(Mock.Of<IApiRequestHandler>());
            services.AddHostedService<GraduationWorker>();

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            Mock<IEmailSender> emailSenderMock = Mock.Get(serviceProvider.GetService<IEmailSender>());
            GraduationWorker backgroundService = serviceProvider.GetService<IHostedService>() as GraduationWorker;

            // Act
            await backgroundService?.StartAsync(CancellationToken.None);
            await Task.Delay(1000);

            // Assert
            emailSenderMock.Verify(c => c.Send(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()
                ), Times.Once);

            await backgroundService?.StopAsync(CancellationToken.None);
        }
    }
}
