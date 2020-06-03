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

        public SwaggerConfig Swagger { get; set; }

        /// <summary>
        /// Gets or sets the fhict oidc.
        /// </summary>
        /// <value>
        /// The ffhict oidc.
        /// </value>
        public FhictOIDConfig FfhictOIDC { get; set; }
        /// <summary>
        ///     Validates this instance.
        /// </summary>
        public void Validate()
        {
            Validator.ValidateObject(Self, new ValidationContext(Self), true);
            Validator.ValidateObject(Api, new ValidationContext(Api), true);
            Validator.ValidateObject(Fhict, new ValidationContext(Fhict), true);
            Validator.ValidateObject(Swagger, new ValidationContext(Swagger), true);
            Validator.ValidateObject(FfhictOIDC, new ValidationContext(FfhictOIDC), true);
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
            ///     Gets or sets the issuer uri.
            /// </summary>
            /// <value>
            ///     The issuer uri.
            /// </value>
            [Required]
            public string IssuerUri { get; set; }
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
            ///     Gets or sets the redirect uri of the frontend.
            /// </summary>
            public string RedirectUriFrontend { get; set; }
            /// <summary>
            ///     Gets or sets the redirect uri for Postman.
            /// </summary>
            public string RedirectUriPostman { get; set; }
            /// <summary>
            ///     Gets or sets the post logouts uri.
            /// </summary>
            public string PostLogoutUriFrontend { get; set; }
            /// <summary>
            ///     Gets or sets the client identifier.
            /// </summary>
            public string ClientId { get; set; }
            /// <summary>
            ///     Gets or sets the client secret.
            /// </summary>
            public string ClientSecret { get; set; }

        }

        /// <summary>
        /// Contains the swagger configuration.
        /// </summary>
        public class SwaggerConfig
        {
            /// <summary>
            /// Gets or sets the redirect uris swagger.
            /// </summary>
            /// <value>
            /// The redirect uris swagger.
            /// </value>
            public string RedirectUrisSwagger { get; set; }

            /// <summary>
            /// Gets or sets the post logout uris swagger.
            /// </summary>
            /// <value>
            /// The post logout uris swagger.
            /// </value>
            public string PostLogoutUrisSwagger { get; set; }
        }


        /// <summary>
        /// Contains the Fontys SSO configuration.
        /// </summary>
        public class FhictOIDConfig
        {
            /// <summary>
            /// Gets or sets the fhict identity URL.
            /// </summary>
            /// <value>
            /// The fhict identity URL.
            /// </value>
            [Required, Url]
            public string Authority { get; set; }
            /// <summary>
            /// Gets or sets the fhict client identifier.
            /// </summary>
            /// <value>
            /// The fhict client identifier.
            /// </value>
            [Required]
            public string ClientId { get; set; }
            /// <summary>
            /// Gets or sets the fhict client secret.
            /// </summary>
            /// <value>
            /// The fhict client secret.
            /// </value>
            [Required]
            public string ClientSecret { get; set; }
            /// <summary>
            /// Gets or sets the fhict scopes.
            /// </summary>
            /// <value>
            /// The fhict scopes.
            /// </value>
            [Required]
            public string Scopes { get; set; }
            /// <summary>
            /// Gets or sets the fhict redirect URI.
            /// </summary>
            /// <value>
            /// The fhict redirect URI.
            /// </value>
            [Required, Url]
            public string RedirectUri { get; set; }

        }
    }

}
