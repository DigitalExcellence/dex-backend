using NetEscapades.Configuration.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Configuration
{
    public class Config : IValidatable
    {
        /// <summary>
        /// 
        /// </summary>
        public class SelfConfig
        {
            /// <summary>
            /// Gets or sets the JWT authority.
            /// </summary>
            /// <value>
            /// The JWT authority.
            /// </value>
            [Required, Url]
            public string JwtAuthority { get; set; }
            /// <summary>
            /// Gets or sets the delete token life time in days.
            /// </summary>
            /// <value>
            /// The delete token life time in days.
            /// </value>
            [Required, Range(0, int.MaxValue)]
            public int DeleteTokenLifeTimeInDays { get; set; }
            /// <summary>
            /// Gets or sets the identity appliations.
            /// </summary>
            /// <value>
            /// The identity appliations.
            /// </value>
            public List<Dictionary<string, string>> IdentityApplications { get; set; }
        }
        /// <summary>
        /// 
        /// </summary>
        public class ApiConfig
        {
            /// <summary>
            /// Gets or sets the DeX rest API URL.
            /// </summary>
            /// <value>
            /// The DeX rest API URL.
            /// </value>
            [Required, Url]
            public string DeXApiUrl { get; set; }
        }
        /// <summary>
        /// 
        /// </summary>
        public class FhictConfig
        {
            /// <summary>
            /// Gets or sets the fhict identity URL.
            /// </summary>
            /// <value>
            /// The fhict identity URL.
            /// </value>
            [Required, Url]
            public string FhictIdentityUrl { get; set; }
            /// <summary>
            /// Gets or sets the fhict client identifier.
            /// </summary>
            /// <value>
            /// The fhict client identifier.
            /// </value>
            [Required]
            public string FhictClientId { get; set; }
            /// <summary>
            /// Gets or sets the fhict client secret.
            /// </summary>
            /// <value>
            /// The fhict client secret.
            /// </value>
            [Required]
            public string FhictClientSecret { get; set; }
            /// <summary>
            /// Gets or sets the fhict scopes.
            /// </summary>
            /// <value>
            /// The fhict scopes.
            /// </value>
            [Required]
            public string FhictScopes { get; set; }
            /// <summary>
            /// Gets or sets the fhict redirect URI.
            /// </summary>
            /// <value>
            /// The fhict redirect URI.
            /// </value>
            [Required, Url]
            public string FhictRedirectUri { get; set; }

        }

        /// <summary>
        /// Gets or sets the self.
        /// </summary>
        /// <value>
        /// The self.
        /// </value>
        public SelfConfig Self { get; set; }
        /// <summary>
        /// Gets or sets the API.
        /// </summary>
        /// <value>
        /// The API.
        /// </value>
        public ApiConfig Api { get; set; }
        /// <summary>
        /// Gets or sets the fhict.
        /// </summary>
        /// <value>
        /// The fhict.
        /// </value>
        public FhictConfig Fhict { get; set; }
        /// <summary>		
        /// Initializes a new instance of the <see cref="Config"/> class.
        /// </summary>
        public Config()
        {
        }
        /// <summary>
        /// Validates this instance.
        /// </summary>
        public void Validate()
        {
            Validator.ValidateObject(Self, new ValidationContext(Self), validateAllProperties: true);
            Validator.ValidateObject(Api, new ValidationContext(Api), validateAllProperties: true);
            Validator.ValidateObject(Fhict, new ValidationContext(Fhict), validateAllProperties: true);
        }
    }
}