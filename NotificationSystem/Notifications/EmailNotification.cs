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
        public String Subject { get; }

        public EmailNotification(string recipientEmail, string textContent, string htmlContent = null)
        {
            this.RecipientEmail = recipientEmail;
            this.TextContent = textContent;
            this.HtmlContent = htmlContent;
            this.Subject = "EMAIL";
        }
    }
}
