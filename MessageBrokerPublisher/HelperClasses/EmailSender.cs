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

using MessageBrokerPublisher.Models;
using Newtonsoft.Json;

namespace MessageBrokerPublisher.HelperClasses
{

    /// <summary>
    ///     EmailSender Interface
    /// </summary>
    public interface IEmailSender
    {

        /// <summary>
        ///     Method to send email
        /// </summary>
        /// <param name="recipient">The email address of the recipient</param>
        /// <param name="textContent">The text content of the email</param>
        /// <param name="htmlContent">The HTML content of the email</param>
        public void Send(string recipient, string textContent, string htmlContent);

    }

    /// <summary>
    ///     Class which is responsible for sending emails
    /// </summary>
    public class EmailSender : IEmailSender
    {

        private readonly ITaskPublisher notificationSender;

        /// <summary>
        ///     Constructor to instantiate the email sender
        /// </summary>
        public EmailSender(ITaskPublisher notificationSender)
        {
            this.notificationSender = notificationSender;
        }

        /// <summary>
        ///     Method to send email
        /// </summary>
        /// <param name="recipient">The email address of the recipient</param>
        /// <param name="textContent">The text content of the email</param>
        /// <param name="htmlContent">The HTML content of the email</param>
        public void Send(string recipient, string textContent, string htmlContent = null)
        {
            EmailNotificationRegister emailNotification =
                new EmailNotificationRegister(recipient, textContent, htmlContent);
            notificationSender.RegisterTask(JsonConvert.SerializeObject(emailNotification), Subject.EMAIL);
        }

    }

    public class OfflineEmailSender : IEmailSender
    {
        public void Send(string recipient, string textContent, string htmlContent)
        {
            return;
        }
    }

}
