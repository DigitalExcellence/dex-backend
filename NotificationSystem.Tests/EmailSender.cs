using Moq;
using NotificationSystem.Notifications;
using NotificationSystem.Services;
using NUnit.Framework;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Linq;
using System.Threading;

namespace NotificationSystem.Tests
{
    public class Tests
    {
        [Test]
        public void ParsePayload_ValidBody_EmailNotification()
        {
            EmailNotification notification = new EmailNotification("test@example.com", "plain text content");
            string payload = Newtonsoft.Json.JsonConvert.SerializeObject(notification);
            EmailSender emailSender = new EmailSender(null, "test");

            emailSender.ParsePayload(payload);

            Assert.AreEqual(notification.RecipientEmail, emailSender.Notification.RecipientEmail);
        }

        [Test]
        public void ParsePayload_InvalidBody_Exception()
        {
            //arange
            string payload = "Some invalid json object";
            EmailSender emailSender = new EmailSender(null, "test");

            //act +assert
            Assert.Throws<Newtonsoft.Json.JsonReaderException>(() => emailSender.ParsePayload(payload));
        }

        [Test]
        public void ExecuteTask_PayloadVerified_CallsSendEmailAsync()
        {
            //arrange
            EmailNotification notification = new EmailNotification("test@example.com", "plain text content");
            string payload = Newtonsoft.Json.JsonConvert.SerializeObject(notification);
            string emailFromString = "test@gmail.com";
            EmailAddress emailFrom = new EmailAddress(emailFromString);
            EmailAddress emailTo = new EmailAddress(notification.RecipientEmail);
            string subject = "You have a new notification on DeX";
            SendGridMessage msg = MailHelper.CreateSingleEmail(emailFrom, emailTo,
                subject, notification.TextContent, notification.HtmlContent);
            Mock<ISendGridClient> sendgridMock = new Mock<ISendGridClient>();
            sendgridMock.Setup(x => x.SendEmailAsync(It.Is<SendGridMessage>(x => x.From == emailFrom &&
                x.Personalizations.First().Tos.First() == emailTo),
                It.IsAny<CancellationToken>())).Verifiable();

            EmailSender emailSender = new EmailSender(sendgridMock.Object, emailFromString);

            //act
            emailSender.ParsePayload(payload);
            emailSender.ExecuteTask();

            //assert
            sendgridMock.Verify();
        }

        [Test]
        public void ExecuteTask_PayloadNotVerified_CallsSendEmailAsync()
        {
            //arrange
            EmailNotification notification = new EmailNotification("test@example.com", "plain text content");
            string emailFromString = "test@gmail.com";
            EmailAddress emailFrom = new EmailAddress(emailFromString);
            EmailAddress emailTo = new EmailAddress(notification.RecipientEmail);
            string subject = "You have a new notification on DeX";
            SendGridMessage msg = MailHelper.CreateSingleEmail(emailFrom, emailTo,
                subject, notification.TextContent, notification.HtmlContent);
            Mock<ISendGridClient> sendgridMock = new Mock<ISendGridClient>();
            sendgridMock.Setup(x => x.SendEmailAsync(It.Is<SendGridMessage>(x => x.From == emailFrom &&
                x.Personalizations.First().Tos.First() == emailTo),
                It.IsAny<CancellationToken>())).Verifiable();

            EmailSender emailSender = new EmailSender(sendgridMock.Object, emailFromString);

            //act + assert
            Assert.Throws<NullReferenceException>(() => emailSender.ExecuteTask());

            
        }

    }

   
}
