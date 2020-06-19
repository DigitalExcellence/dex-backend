using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Models
{
    /// <summary>
    /// The user model that is used in the identity server.
    /// </summary>
    public class IdentityUser
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the subject identifier.
        /// This is our own subject identifier.
        /// </summary>
        /// <value>
        /// The subject identifier.
        /// </value>
        public string SubjectId { get; set; }

        /// <summary>
        /// Gets or sets the provider identifier.
        /// </summary>
        /// <value>
        /// The provider identifier.
        /// </value>
        public string ProviderId { get; set; }

        /// <summary>
        /// Gets or sets the password hash.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the lastname.
        /// </summary>
        /// <value>
        /// The lastname.
        /// </value>
        public string Lastname { get; set; }

        /// <summary>
        /// Gets or sets the firstname.
        /// </summary>
        /// <value>
        /// The firstname.
        /// </value>
        public string Firstname { get; set; }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// The name used to log in.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the external profile URL.
        /// This url links to the external providers user profile.
        /// </summary>
        /// <value>
        /// The external profile URL.
        /// </value>
        public string ExternalProfileUrl { get; set; }

        /// <summary>
        /// Gets or sets the external picture.
        /// This url links to the external providers api to get the profile picture.
        /// </summary>
        /// <value>
        /// The external picture.
        /// </value>
        public string ExternalPicture { get; set; }

        /// <summary>
        /// Gets or sets the external subject identifier.
        /// This is the sub claim from an external provider.
        /// </summary>
        /// <value>
        /// The external subject identifier.
        /// </value>
        public string ExternalSubjectId { get; set; }

    }
}
