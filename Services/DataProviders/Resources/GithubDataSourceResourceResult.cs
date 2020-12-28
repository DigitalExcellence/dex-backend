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

namespace Services.DataProviders.Resources
{
    /// <summary>
    /// Viewmodel for the project github data source.
    /// </summary>
    public class GithubDataSourceResourceResult
    {
        /// <summary>
        /// Gets or sets the unique identifier of the incoming project.
        /// </summary>
        /// <value>
        /// The unique identifier of the project.
        /// </value>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the incoming project.
        /// </summary>
        /// <value>
        /// The name of the project.
        /// </value>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the incoming project.
        /// </summary>
        /// <value>
        /// The unique description of the project.
        /// </value>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the boolean that represents whether the incoming project is private.
        /// </summary>
        /// <value>
        /// <see langword="true"/> represents that the project is private, <see langword="false"/> represents that the project is public.
        /// </value>
        [JsonProperty("private")]
        public bool IsPrivate { get; set; }
    }
}
