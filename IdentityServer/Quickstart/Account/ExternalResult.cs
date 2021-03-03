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

using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer.Quickstart.Account
{

    /// <summary>
    ///     The ExternalResult model, this contains all the information needed to authenticate an external user.
    /// </summary>
    public class ExternalResult
    {

        /// <summary>
        ///     Gets or sets the return URL.
        /// </summary>
        /// <value>
        ///     The return URL.
        /// </value>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     Gets or sets the claims.
        /// </summary>
        /// <value>
        ///     The claims.
        /// </value>
        public IEnumerable<Claim> Claims { get; set; }

        /// <summary>
        ///     Gets or sets the schema.
        /// </summary>
        /// <value>
        ///     The schema.
        /// </value>
        public string Schema { get; set; }

        /// <summary>
        ///     Gets or sets the identifier token.
        /// </summary>
        /// <value>
        ///     The identifier token.
        /// </value>
        public string IdToken { get; set; }

    }

}
