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

namespace Services.ExternalDataProviders.Resources
{

    /// <summary>
    ///     The jsFiddle Data source resource result
    /// </summary>
    public class JsFiddleDataSourceResourceResult
    {

        /// <summary>
        ///     The framework
        /// </summary>
        [JsonProperty("framework")]
        public string Framework { get; set; }

        /// <summary>
        ///     The version
        /// </summary>
        [JsonProperty("version")]
        public int Version { get; set; }

        /// <summary>
        ///     The description
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        ///     The title
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        ///     The url
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        ///     The author
        /// </summary>
        [JsonProperty("author")]
        public string Author { get; set; }

        /// <summary>
        ///     The latest version
        /// </summary>
        [JsonProperty("latest_version")]
        public int LatestVersion { get; set; }

        /// <summary>
        ///     The created date
        /// </summary>
        [JsonProperty("created")]
        public string Created { get; set; }

    }

}
