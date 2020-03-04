using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NetEscapades.Configuration.Validation;

namespace Digital_Excellence.Configuration
{
	public class Config : IValidatable
	{
		/// <summary>
		/// 
		/// </summary>
		public class SelfConfig
		{
			/// <summary>
			/// Gets or sets the front end.
			/// </summary>
			/// <value>
			/// The front end.
			/// </value>
			[Required, Url]
			public string FrontEnd { get; set; }
			/// <summary>
			/// Gets or sets the client identifier.
			/// </summary>
			/// <value>
			/// The client identifier.
			/// </value>
			[Required]
			public string ClientId { get; set; }
			/// <summary>
			/// Gets or sets the client secret.
			/// </summary>
			/// <value>
			/// The client secret.
			/// </value>
			[Required]
			public string ClientSecret { get; set; }
		}
		/// <summary>
		/// 
		/// </summary>
		public class IdentityServerConfig
		{
			/// <summary>
			/// Gets or sets the identity URL.
			/// </summary>
			/// <value>
			/// The identity URL.
			/// </value>
			[Required, Url]
			public string IdentityUrl { get; set; }
		}



		/// <summary>
		/// Gets or sets the original configuration.
		/// </summary>
		/// <value>
		/// The original configuration.
		/// </value>
		public IConfiguration OriginalConfiguration { get; set; }
		/// <summary>
		/// Gets or sets the self.
		/// </summary>
		/// <value>
		/// The self.
		/// </value>
		public SelfConfig Self { get; set; }
		/// <summary>
		/// Gets or sets the identity server.
		/// </summary>
		/// <value>
		/// The identity server.
		/// </value>
		public IdentityServerConfig IdentityServer { get; set; }


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
			
			//Validator.ValidateObject(Self, new ValidationContext(Self), validateAllProperties: true);
			//Validator.ValidateObject(IdentityServer, new ValidationContext(IdentityServer), validateAllProperties: true);
			//Validator.ValidateObject(Smtp, new ValidationContext(Smtp), validateAllProperties: true);
		}
	}
}
