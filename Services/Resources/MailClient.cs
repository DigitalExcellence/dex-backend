using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Resources
{
    public class MailClient
    {
        private readonly ISendGridClient sendGridClient;
        private readonly EmailAddress fromEmailAdress;

        //This class might be temporary untill the RabbitMQ emailSender has been fixed
        public MailClient()
        {
            sendGridClient = new SendGridClient(Environment.GetEnvironmentVariable("App__SendGrid__ApiKey"));
            fromEmailAdress = new EmailAddress(Environment.GetEnvironmentVariable("App__SendGrid__EmailFrom"));
        }

        public async Task<Response> SendMail(string receiverAdress, string plainTextContent,string subject, string htmlContent)
        {
            EmailAddress receiverEmail = new EmailAddress(receiverAdress);
            SendGridMessage msg = MailHelper.CreateSingleEmail(fromEmailAdress, receiverEmail, subject, plainTextContent, htmlContent);

            return await sendGridClient.SendEmailAsync(msg);
        }
    }
}
