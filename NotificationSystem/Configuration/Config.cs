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

using NetEscapades.Configuration.Validation;
using System.ComponentModel.DataAnnotations;

namespace NotificationSystem.Configuration
{

    /// <summary>
    ///     Config class
    /// </summary>
    /// <seealso cref="NetEscapades.Configuration.Validation.IValidatable" />
    public class Config : IValidatable
    {

        /// <summary>
        ///     Gets or sets the rabbit mq config.
        /// </summary>
        /// <value>
        ///     The rabbit mq config.
        /// </value>
        public RabbitMQConfig RabbitMQ { get; set; }

        /// <summary>
        ///     Gets or sets the sendgrid Config
        /// </summary>
        /// <value>
        ///     The SendGrid config.
        /// </value>
        public SendGridConfig SendGrid { get; set; }

        /// <summary>
        ///     Validates this instance.
        /// </summary>
        public void Validate()
        {
            Validator.ValidateObject(RabbitMQ, new ValidationContext(RabbitMQ), true);
            Validator.ValidateObject(SendGrid, new ValidationContext(SendGrid), true);
        }

    }

    /// <summary>
    ///     Configuration settings for Rabbit MQ Instance
    /// </summary>
    public class RabbitMQConfig
    {

        /// <summary>
        ///     Gets or sets the hostname.
        /// </summary>
        /// <value>
        ///     The hostname of the rabbit mq instance.
        /// </value>
        [Required]
        public string Hostname { get; set; }

        /// <summary>
        ///     Gets or sets the username.
        /// </summary>
        /// <value>
        ///     The usename of the rabbit mq instance.
        /// </value>
        [Required]
        public string Username { get; set; }

        /// <summary>
        ///     Gets or sets the password.
        /// </summary>
        /// <value>
        ///     The password of the rabbit mq instance.
        /// </value>
        [Required]
        public string Password { get; set; }

    }

    /// <summary>
    ///     Configuration settings for Sendgrid
    /// </summary>
    public class SendGridConfig
    {

        /// <summary>
        ///     Gets or sets the api key.
        /// </summary>
        /// <value>
        ///     The api key for SendGrid.
        /// </value>
        [Required]
        public string ApiKey { get; set; }

        /// <summary>
        ///     Gets or sets the email address that emails are send from.
        /// </summary>
        /// <value>
        ///     The email that is used to send emails from.
        /// </value>
        [Required]
        public string EmailFrom { get; set; }


        /// <summary>
        ///     Gets or sets the email sandbox settings.
        /// </summary>
        [Required]
        public bool SandboxMode { get; set; }

    }

}
