using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NotificationSystem.Contracts;
using NotificationSystem.Notifications;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace NotificationSystem.Services
{
    public class EmailSender : INotificationService
    {
        private readonly SendGridClient client;
        private readonly EmailAddress from;

        public EmailSender()
        {
            client = new SendGridClient(Environment.GetEnvironmentVariable("SENDGRID_API_KEY"));
            from = new EmailAddress(Environment.GetEnvironmentVariable("SENDGRID_EMAIL_FROM"));
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

            if (string.IsNullOrEmpty(emailNotification.RecipientEmail) || string.IsNullOrWhiteSpace(emailNotification.RecipientEmail))
            {
                return false;
            }

            if (string.IsNullOrEmpty(emailNotification.TextContent))
            {
                return false;
            }

            return true;
        }

        private async Task Execute(string recipient, string textContent, string htmlContent = null)
        {
            string subject = "You have a new notification on DeX";
            EmailAddress to = new EmailAddress(recipient);
            SendGridMessage msg = MailHelper.CreateSingleEmail(from, to, subject, textContent, htmlContent);
            _ = await client.SendEmailAsync(msg);
        }
    }
}
