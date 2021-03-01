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

using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using NotificationSystem.Configuration;
using NotificationSystem.Notifications;
using NotificationSystem.Services;
using NUnit.Framework;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace NotificationSystem.Tests
{

    public class EmailSenderTests
    {

        [Test]
        public void ParsePayload_ValidBody_EmailNotification()
        {
            // Arrange
            EmailNotification notification = new EmailNotification("test@example.com", "plain text content");
            string payload = JsonConvert.SerializeObject(notification);
            EmailSender emailSender = new EmailSender(null, "test");

            // Act
            emailSender.ParsePayload(payload);

            // Assert
            Assert.AreEqual(notification.RecipientEmail, emailSender.Notification.RecipientEmail);
        }

        [Test]
        public void ParsePayload_InvalidBody_Exception()
        {
            // Arrange
            string payload = "Some invalid json object";
            EmailSender emailSender = new EmailSender(null, "test");

            // Act + Assert
            Assert.Throws<JsonReaderException>(() => emailSender.ParsePayload(payload));
        }

        [Test]
        public void ExecuteTask_PayloadVerified_CallsSendEmailAsync()
        {
            // Arrange
            EmailNotification notification = new EmailNotification("test@example.com", "plain text content");
            string payload = JsonConvert.SerializeObject(notification);
            string emailFromString = "test@gmail.com";
            EmailAddress emailFrom = new EmailAddress(emailFromString);
            EmailAddress emailTo = new EmailAddress(notification.RecipientEmail);
            string subject = "You have a new notification on DeX";
            SendGridMessage msg = MailHelper.CreateSingleEmail(emailFrom,
                                                               emailTo,
                                                               subject,
                                                               notification.TextContent,
                                                               notification.HtmlContent);
            Mock<ISendGridClient> sendgridMock = new Mock<ISendGridClient>();
            sendgridMock.Setup(x => x.SendEmailAsync(It.Is<SendGridMessage>(x => x.From == emailFrom &&
                                                                                x.Personalizations.First()
                                                                                    .Tos.First() ==
                                                                                emailTo),
                                                     It.IsAny<CancellationToken>()))
                        .Verifiable();

            EmailSender emailSender = new EmailSender(sendgridMock.Object, emailFromString);

            // Act
            emailSender.ParsePayload(payload);
            emailSender.ExecuteTask();

            // Assert
            sendgridMock.Verify();
        }

        [Test]
        public void ExecuteTask_PayloadNotParsed_CallsSendEmailAsync()
        {
            // Arrange
            EmailNotification notification = new EmailNotification("test@example.com", "plain text content");
            string emailFromString = "test@gmail.com";
            EmailAddress emailFrom = new EmailAddress(emailFromString);
            EmailAddress emailTo = new EmailAddress(notification.RecipientEmail);
            string subject = "You have a new notification on DeX";
            SendGridMessage msg = MailHelper.CreateSingleEmail(emailFrom,
                                                               emailTo,
                                                               subject,
                                                               notification.TextContent,
                                                               notification.HtmlContent);
            Mock<ISendGridClient> sendgridMock = new Mock<ISendGridClient>();
            sendgridMock.Setup(x => x.SendEmailAsync(It.Is<SendGridMessage>(x => x.From == emailFrom &&
                                                                                x.Personalizations.First()
                                                                                    .Tos.First() ==
                                                                                emailTo),
                                                     It.IsAny<CancellationToken>()))
                        .Verifiable();

            EmailSender emailSender = new EmailSender(sendgridMock.Object, emailFromString);

            // Act + Assert
            Assert.Throws<NullReferenceException>(() => emailSender.ExecuteTask());
        }

        [Test]
        public void ValidatePayload_PayloadNotParsed_Exception()
        {
            // Arrange
            string emailFromString = "test@gmail.com";
            EmailSender emailSender = new EmailSender(null, emailFromString);

            // Act + Assert
            Assert.Throws<NullReferenceException>(() => emailSender.ValidatePayload());
        }


        [Test]
        public void ValidatePayload_NonEmptyValues_True()
        {
            // Arrange
            EmailNotification notification = new EmailNotification("test@example.com", "plain text content");
            string payload = JsonConvert.SerializeObject(notification);
            string emailFromString = "test@gmail.com";
            EmailSender emailSender = new EmailSender(null, emailFromString);

            // Act
            emailSender.ParsePayload(payload);
            bool result = emailSender.ValidatePayload();

            // Assert
            Assert.AreEqual(true, result);
        }

        [Test]
        public void ValidatePayload_EmptyRecipientEmail_False()
        {
            // Arrange
            EmailNotification notification = new EmailNotification("", "plain text content");
            string payload = JsonConvert.SerializeObject(notification);
            string emailFromString = "test@gmail.com";
            EmailSender emailSender = new EmailSender(null, emailFromString);

            // Act
            emailSender.ParsePayload(payload);
            bool result = emailSender.ValidatePayload();

            // Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void ValidatePayload_EmptyTextContent_False()
        {
            // Arrange
            EmailNotification notification = new EmailNotification("test@gmail.com", "");
            string payload = JsonConvert.SerializeObject(notification);
            string emailFromString = "test@gmail.com";
            EmailSender emailSender = new EmailSender(null, emailFromString);

            // Act
            emailSender.ParsePayload(payload);
            bool result = emailSender.ValidatePayload();

            // Assert
            Assert.AreEqual(false, result);
        }


        // Valid SendGrid API key should be set in NotificationSystem/appsettings.Development.json or
        // as an environmental variable with key App__SendGrid__ApiKey for this test to pass
        [Test]
        public void EmailSending_WithValidApiKey_ValidEmail()
        {
            // Arrange
            EmailNotification notification = new EmailNotification("test@example.com", "plain text content");
            string payload = JsonConvert.SerializeObject(notification);

            string jsonConfig = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory,
                                                              "..\\..\\..\\..\\NotificationSystem\\appsettings.Development.json"));
            IConfiguration configuration = new ConfigurationBuilder()
                                           .AddJsonFile(jsonConfig, true, true)
                                           .AddEnvironmentVariables()
                                           .Build();

            Config config = configuration.GetSection("App")
                                         .Get<Config>();
            SendGridClient sendGridClient = new SendGridClient(config.SendGrid.ApiKey);

            // Act
            EmailSender sender = new EmailSender(sendGridClient, config.SendGrid.EmailFrom, true);
            sender.ParsePayload(payload);
            sender.ExecuteTask();

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, sender.Response.StatusCode);
        }

        [Test]
        public void EmailSending_WithInvalidApiKey_ValidEmail()
        {
            // Arrange
            EmailNotification notification = new EmailNotification("test@example.com", "plain text content");
            string payload = JsonConvert.SerializeObject(notification);

            string jsonConfig = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory,
                                                              "..\\..\\..\\..\\NotificationSystem\\appsettings.Development.json"));
            IConfiguration configuration = new ConfigurationBuilder()
                                           .AddJsonFile(jsonConfig, true, true)
                                           .AddEnvironmentVariables()
                                           .Build();

            Config config = configuration.GetSection("App")
                                         .Get<Config>();

            SendGridClient sendGridClient = new SendGridClient("test");

            // Act
            EmailSender sender = new EmailSender(sendGridClient, config.SendGrid.EmailFrom, true);
            sender.ParsePayload(payload);
            sender.ExecuteTask();

            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, sender.Response.StatusCode);
        }

        // Valid SendGrid API key should be set in NotificationSystem/appsettings.Development.json or
        // as an environmental variable with key App__SendGrid__ApiKey for this test to pass
        [Test]
        public void EmailSending_WithValidApiKey_InvalidEmail()
        {
            // Arrange
            EmailNotification notification = new EmailNotification("example.com", "plain text content");
            string payload = JsonConvert.SerializeObject(notification);

            string jsonConfig = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory,
                                                              "..\\..\\..\\..\\NotificationSystem\\appsettings.Development.json"));
            IConfiguration configuration = new ConfigurationBuilder()
                                           .AddJsonFile(jsonConfig, true, true)
                                           .AddEnvironmentVariables()
                                           .Build();

            Config config = configuration.GetSection("App")
                                         .Get<Config>();

            SendGridClient sendGridClient = new SendGridClient(config.SendGrid.ApiKey);

            // Act
            EmailSender sender = new EmailSender(sendGridClient, "test@example.com", true);
            sender.ParsePayload(payload);
            sender.ExecuteTask();

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, sender.Response.StatusCode);
        }

    }

}
