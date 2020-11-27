using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MessagebrokerPublisher;
using MessagebrokerPublisher.Contracts;
using NotificationSystem.Contracts;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace NotificationSystem.Services
{
    public class EmailSender : INotificationService
    {
        private SendGridClient client;
        private EmailAddress from;

        public EmailSender()
        {
            this.client = new SendGridClient(Environment.GetEnvironmentVariable("SENDGRID_API_KEY"));
            this.from = new EmailAddress(Environment.GetEnvironmentVariable("SENDGRID_EMAIL_FROM"));
        }

        public void SendNotification(INotification notification)
        {
            EmailNotification emailNotification = (EmailNotification) notification;

            if (ValidateNotification(emailNotification))
            {
                Execute(emailNotification.RecipientEmail, emailNotification.TextContent, emailNotification.HtmlContent).Wait();
            }
        }

        public bool ValidateNotification(INotification notification)
        {
            EmailNotification emailNotification = (EmailNotification) notification;

            if (String.IsNullOrEmpty(emailNotification.RecipientEmail) || String.IsNullOrWhiteSpace(emailNotification.RecipientEmail))
            {
                return false;
            }

            if (String.IsNullOrEmpty(emailNotification.TextContent))
            {
                return false;
            }

            return true;
        }

        async Task Execute(string recipient, string textContent, string htmlContent = null)
        {
            string subject = "You have a new notification on DeX";
            EmailAddress to = new EmailAddress(recipient);
            SendGridMessage msg = MailHelper.CreateSingleEmail(this.from, to, subject, textContent, htmlContent);
            var result = await client.SendEmailAsync(msg);
        }
    }
}
