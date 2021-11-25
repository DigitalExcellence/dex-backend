using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using JobScheduler;
using MessageBrokerPublisher.HelperClasses;
using Microsoft.Extensions.Logging;

namespace JobScheduler.Tests
{

    public class GraduationWorkerTests
    {

        [Test]
        public void GraduationJob_EmailNotification_NotificationGraduating()
        {
            IEmailSender emailSender = new EmailSender();
            ILogger logger;
            IApiRequestHandler apiRequestHandler = new ApiRequestHandler();
            Config config = new Config();

            GraduationWorker gw = new GraduationWorker(logger, apiRequestHandler, emailSender, config);

        }
    }
}
