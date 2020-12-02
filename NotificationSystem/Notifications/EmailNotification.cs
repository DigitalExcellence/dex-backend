using System;
using System.Collections.Generic;
using System.Text;
using NotificationSystem.Contracts;

namespace NotificationSystem.Notifications
{
    public class EmailNotification : INotification
    {
        public string RecipientEmail { get; set; }
        public string TextContent { get; set; }
        public string HtmlContent { get; set; }
        public string Subject { get; }

        public EmailNotification(string recipientEmail, string textContent, string htmlContent = null)
        {
            RecipientEmail = recipientEmail;
            TextContent = textContent;
            HtmlContent = htmlContent;
            Subject = "EMAIL";
        }
    }
}
