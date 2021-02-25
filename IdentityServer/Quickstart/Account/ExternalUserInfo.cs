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

using Newtonsoft.Json;

namespace IdentityServer.Quickstart.Account
{

    /// <summary>
    ///     The viewmodel for /connect/userinfo
    /// </summary>
    public class ExternalUserInfo
    {

        /// <summary>
        ///     Gets or sets the subject id.
        ///     This is the token identifier
        /// </summary>
        /// <value>
        ///     The sub.
        /// </value>
        [JsonProperty("sub")]
        public string sub { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the name of the family.
        /// </summary>
        /// <value>
        ///     The name of the family.
        /// </value>
        [JsonProperty("family_name")]
        public string FamilyName { get; set; }

        /// <summary>
        ///     Gets or sets the given name of the external user.
        /// </summary>
        /// <value>
        ///     The given name of the external user.
        /// </value>
        [JsonProperty("given_name")]
        public string GivenName { get; set; }

        /// <summary>
        ///     Gets or sets the preferred username.
        /// </summary>
        /// <value>
        ///     The preferred username.
        /// </value>
        [JsonProperty("preferred_username")]
        public string PreferredUsername { get; set; }

        /// <summary>
        ///     Gets or sets the link to the external user profile.
        /// </summary>
        /// <value>
        ///     The profile.
        /// </value>
        [JsonProperty("profile")]
        public string Profile { get; set; }

        /// <summary>
        ///     Gets or sets the updated at.
        /// </summary>
        /// <value>
        ///     The updated at.
        /// </value>
        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        /// <summary>
        ///     Gets or sets the email.
        /// </summary>
        /// <value>
        ///     The email.
        /// </value>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        ///     Gets or sets the role.
        /// </summary>
        /// <value>
        ///     The role.
        /// </value>
        [JsonProperty("role")]
        public string[] Role { get; set; }

    }

}
