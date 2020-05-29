using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Quickstart.Account
{
    /// <summary>
    /// The viewmodel for /connect/userinfo
    /// </summary>
    public class ExternalUserInfo
    {
        /// <summary>
        /// Gets or sets the sub.
        /// </summary>
        /// <value>
        /// The sub.
        /// </value>
        public string sub { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string name { get; set; }
        /// <summary>
        /// Gets or sets the name of the family.
        /// </summary>
        /// <value>
        /// The name of the family.
        /// </value>
        public string family_name { get; set; }
        /// <summary>
        /// Gets or sets the name of the given.
        /// </summary>
        /// <value>
        /// The name of the given.
        /// </value>
        public string given_name { get; set; }
        /// <summary>
        /// Gets or sets the preferred username.
        /// </summary>
        /// <value>
        /// The preferred username.
        /// </value>
        public string preferred_username { get; set; }
        /// <summary>
        /// Gets or sets the profile.
        /// </summary>
        /// <value>
        /// The profile.
        /// </value>
        public string profile { get; set; }
        /// <summary>
        /// Gets or sets the updated at.
        /// </summary>
        /// <value>
        /// The updated at.
        /// </value>
        public string updated_at { get; set; }
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string email { get; set; }
        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
        public string[] role { get; set; }
    }
}
