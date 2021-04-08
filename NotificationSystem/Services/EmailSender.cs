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

using Newtonsoft.Json;
using NotificationSystem.Contracts;
using NotificationSystem.Notifications;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace NotificationSystem.Services
{
    public class EmailSender : ICallbackService
    {

        private readonly ISendGridClient client;
        private readonly EmailAddress from;
        private readonly bool sandboxMode;
        private EmailNotification notification;
        private Response response;
        
        public EmailSender(ISendGridClient sendGridClient,  string emailFrom, bool sandboxMode = false)
        {
            client = sendGridClient;
            from = new EmailAddress(emailFrom);
            this.sandboxMode = sandboxMode;
        }


        public void ParsePayload(string jsonBody)
        {
            notification = JsonConvert.DeserializeObject<EmailNotification>(jsonBody);
        }

        public void ExecuteTask()
        {
            EmailNotification emailNotification = notification;
            response = Execute(emailNotification.RecipientEmail, emailNotification.TextContent, emailNotification.HtmlContent).Result;
        }

        public bool ValidatePayload()
        {
            EmailNotification emailNotification = notification;

            if(string.IsNullOrEmpty(emailNotification.RecipientEmail) ||
               string.IsNullOrWhiteSpace(emailNotification.RecipientEmail))
            {
                return false;
            }

            if(string.IsNullOrEmpty(emailNotification.TextContent))
            {
                return false;
            }

            return true;
        }

        private async Task<Response> Execute(string recipient, string textContent, string htmlContent = null)
        {
            string subject = "You have a new notification on DeX";
            EmailAddress to = new EmailAddress(recipient);
            SendGridMessage msg = MailHelper.CreateSingleEmail(from, to, subject, textContent, htmlContent);
            msg.SetSandBoxMode(sandboxMode);

            return await client.SendEmailAsync(msg);
        }

        public Response Response { get => response; set => response = value; }
        public EmailNotification Notification { get => notification;}
    }

}
