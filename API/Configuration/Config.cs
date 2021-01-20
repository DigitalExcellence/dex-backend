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

using Microsoft.Extensions.Configuration;
using NetEscapades.Configuration.Validation;
using System.ComponentModel.DataAnnotations;

namespace API.Configuration
{
    /// <summary>
    /// Config class
    /// </summary>
    /// <seealso cref="NetEscapades.Configuration.Validation.IValidatable" />
    public class Config : IValidatable
    {
        /// <summary>
        ///     Gets or sets the original configuration.
        /// </summary>
        /// <value>
        ///     The original configuration.
        /// </value>
        public IConfiguration OriginalConfiguration { get; set; }

        /// <summary>
        ///     Gets or sets the self.
        /// </summary>
        /// <value>
        ///     The self.
        /// </value>
        public FrontendConfig Frontend { get; set; }

        /// <summary>
        ///     Gets or sets the identity server.
        /// </summary>
        /// <value>
        ///     The identity server.
        /// </value>
        public IdentityServerConfig IdentityServer { get; set; }

        /// <summary>
        /// Gets or sets the swagger configuration.
        /// </summary>
        /// <value>
        /// The swagger.
        /// </value>
        public SwaggerConfig Swagger { get; set; }

        /// <summary>
        /// Gets or sets the RabbitMQ configuration.
        /// </summary>
        /// <value>
        /// The RabbitMQ.
        /// </value>
        public RabbitMQConfig RabbitMQ { get; set; }
        /// <summary>
        ///     Validates this instance.
        /// </summary>
        public void Validate()
        {
            Validator.ValidateObject(Frontend, new ValidationContext(Frontend), true);
            Validator.ValidateObject(IdentityServer, new ValidationContext(IdentityServer), true);
            Validator.ValidateObject(Swagger, new ValidationContext(Swagger), true);
            Validator.ValidateObject(RabbitMQ, new ValidationContext(RabbitMQ), true);
        }
    }

    /// <summary>
    /// Configuration settings for the frontend.
    /// </summary>
    public class FrontendConfig
    {
        /// <summary>
        ///     Gets or sets the front end.
        /// </summary>
        /// <value>
        ///     The front end.
        /// </value>
        [Required]
        [Url]
        public string FrontendUrl { get; set; }

        /// <summary>
        ///     Gets or sets the client identifier.
        /// </summary>
        /// <value>
        ///     The client identifier.
        /// </value>
        [Required]
        public string ClientId { get; set; }

        /// <summary>
        ///     Gets or sets the client secret.
        /// </summary>
        /// <value>
        ///     The client secret.
        /// </value>
        [Required]
        public string ClientSecret { get; set; }
    }

    /// <summary>
    /// Contains the identity server configuration.
    /// </summary>
    public class IdentityServerConfig
    {
        /// <summary>
        ///     Gets or sets the identity URL.
        /// </summary>
        /// <value>
        ///     The identity URL.
        /// </value>
        [Required]
        [Url]
        public string IdentityUrl { get; set; }
        /// <summary>
        ///     Gets or sets the Development identity URL.
        /// This is used mostly to fix docker environments.
        /// </summary>
        /// <value>
        ///     The identity URL.
        /// </value>
        [Url]
        public string DevelopmentIdentityUrl { get; set; }
    }

    /// <summary>
    /// Contains the swagger configuration.
    /// </summary>
    public class SwaggerConfig
    {
        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        /// <value>
        /// The client identifier.
        /// </value>
        [Required]
        public string ClientId { get; set; }
    }

    /// <summary>
    /// Contains the RabbitMQConfig configuration.
    /// </summary>
    public class RabbitMQConfig
    {
        /// <summary>
        /// Gets or sets the hostname.
        /// </summary>
        /// <value>
        /// The hostname.
        /// </value>
        [Required]
        public string Hostname { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        [Required]
        public string Password { get; set; }
    }
}
