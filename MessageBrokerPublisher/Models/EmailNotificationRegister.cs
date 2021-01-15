namespace MessageBrokerPublisher.Models
{
    public class EmailNotificationRegister
    {
        public string RecipientEmail { get; set; }
        public string TextContent { get; set; }
        public string HtmlContent { get; set; }
        public string Subject { get; }

        public EmailNotificationRegister(string recipientEmail, string textContent, string htmlContent = null)
        {
            RecipientEmail = recipientEmail;
            TextContent = textContent;
            HtmlContent = htmlContent;
            Subject = "EMAIL";
        }
    }
}
