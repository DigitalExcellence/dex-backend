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
    ///     The viewmodel for /connect/token
    /// </summary>
    public class ExternalConnectToken
    {

        /// <summary>
        ///     Gets or sets the identifier token.
        /// </summary>
        /// <value>
        ///     The identifier token.
        /// </value>
        [JsonProperty("id_token")]
        public string IdToken { get; set; }

        /// <summary>
        ///     Gets or sets the access token.
        /// </summary>
        /// <value>
        ///     The access token.
        /// </value>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        ///     Gets or sets the expires in.
        /// </summary>
        /// <value>
        ///     The expires in.
        /// </value>
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        /// <summary>
        ///     Gets or sets the type of the token.
        /// </summary>
        /// <value>
        ///     The type of the token.
        /// </value>
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        /// <summary>
        ///     Gets or sets the refresh token.
        /// </summary>
        /// <value>
        ///     The refresh token.
        /// </value>
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

    }

}
