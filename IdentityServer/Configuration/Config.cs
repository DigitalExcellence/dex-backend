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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Configuration
{

    public class Config : IValidatable
    {

        /// <summary>
        ///     Gets or sets the self.
        /// </summary>
        /// <value>
        ///     The self.
        /// </value>
        public SelfConfig Self { get; set; }

        /// <summary>
        ///     Gets or sets the API.
        /// </summary>
        /// <value>
        ///     The API.
        /// </value>
        public ApiConfig Api { get; set; }

        /// <summary>
        ///     Gets or sets the fhict.
        /// </summary>
        /// <value>
        ///     The fhict.
        /// </value>
        public FhictConfig Fhict { get; set; }

        /// <summary>
        ///     Gets or sets the frontend
        /// </summary>
        /// <value>
        ///     The fhict.
        /// </value>
        public FrontendConfig Frontend { get; set; }

        /// <summary>
        ///     Validates this instance.
        /// </summary>
        public void Validate()
        {
            Validator.ValidateObject(Self, new ValidationContext(Self), true);
            Validator.ValidateObject(Api, new ValidationContext(Api), true);
            Validator.ValidateObject(Fhict, new ValidationContext(Fhict), true);
        }

        /// <summary>
        /// </summary>
        public class SelfConfig
        {

            /// <summary>
            ///     Gets or sets the JWT authority.
            /// </summary>
            /// <value>
            ///     The JWT authority.
            /// </value>
            [Required]
            [Url]
            public string JwtAuthority { get; set; }

            /// <summary>
            ///     Gets or sets the delete token life time in days.
            /// </summary>
            /// <value>
            ///     The delete token life time in days.
            /// </value>
            [Required]
            [Range(0, int.MaxValue)]
            public int DeleteTokenLifeTimeInDays { get; set; }

            /// <summary>
            ///     Gets or sets the identity applications.
            /// </summary>
            /// <value>
            ///     The identity applications.
            /// </value>
            public List<Dictionary<string, string>> IdentityApplications { get; set; }

        }

        /// <summary>
        /// </summary>
        public class ApiConfig
        {

            /// <summary>
            ///     Gets or sets the DeX rest API URL.
            /// </summary>
            /// <value>
            ///     The DeX rest API URL.
            /// </value>
            [Required]
            [Url]
            public string DeXApiUrl { get; set; }

        }

        /// <summary>
        /// </summary>
        public class FhictConfig
        {

            /// <summary>
            ///     Gets or sets the fhict identity URL.
            /// </summary>
            /// <value>
            ///     The fhict identity URL.
            /// </value>
            [Required]
            [Url]
            public string FhictIdentityUrl { get; set; }

            /// <summary>
            ///     Gets or sets the fhict client identifier.
            /// </summary>
            /// <value>
            ///     The fhict client identifier.
            /// </value>
            [Required]
            public string FhictClientId { get; set; }

            /// <summary>
            ///     Gets or sets the fhict client secret.
            /// </summary>
            /// <value>
            ///     The fhict client secret.
            /// </value>
            [Required]
            public string FhictClientSecret { get; set; }

            /// <summary>
            ///     Gets or sets the fhict scopes.
            /// </summary>
            /// <value>
            ///     The fhict scopes.
            /// </value>
            [Required]
            public string FhictScopes { get; set; }

            /// <summary>
            ///     Gets or sets the fhict redirect URI.
            /// </summary>
            /// <value>
            ///     The fhict redirect URI.
            /// </value>
            [Required]
            [Url]
            public string FhictRedirectUri { get; set; }

        }
        /// <summary>
        /// </summary>
        public class FrontendConfig
        {
            /// <summary>
            ///     Gets or sets the redirect urls of the frontend
            /// </summary>
            public List<string> RedirectUrisFrontend { get; set; }
            /// <summary>
            ///     Gets or sets the post logouts urls
            /// </summary>
            public List<string> PostLogoutUrisFrontend { get; set; }

        }

    }

}
