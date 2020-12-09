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

using System.ComponentModel.DataAnnotations;

namespace ElasticSynchronizer.Configuration
{
    public class ElasticConfig
    {
        /// <summary>
        ///     Gets or sets the hostname.
        /// </summary>
        /// <value>
        ///     The hostname of the elastic instance.
        /// </value>
        [Required]
        public string Hostname { get; set; }

        /// <summary>
        ///     Gets or sets the username.
        /// </summary>
        /// <value>
        ///     The username of the elastic instance.
        /// </value>
        [Required]
        public string Username { get; set; }

        /// <summary>
        ///     Gets or sets the password.
        /// </summary>
        /// <value>
        ///     The password of the elastic instance.
        /// </value>
        [Required]
        public string Password { get; set; }


        /// <summary>
        ///     Gets or sets the index.
        /// </summary>
        /// <value>
        ///     The index of the elastic document.
        /// </value>
        [Required]
        public string IndexUrl { get; set;  }
    }
}
