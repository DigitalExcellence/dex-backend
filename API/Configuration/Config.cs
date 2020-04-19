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
        ///     Validates this instance.
        /// </summary>
        public void Validate()
        {
            Validator.ValidateObject(Frontend, new ValidationContext(Frontend), true);
            Validator.ValidateObject(IdentityServer, new ValidationContext(IdentityServer), true);
        }

        /// <summary>
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

        }

    }

}
