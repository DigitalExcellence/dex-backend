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

    public class JsFiddleDataSourceResourceResult
    {

        [JsonProperty("framework")]
        public string Framework { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("latest_version")]
        public int LatestVersion { get; set; }

        [JsonProperty("created")]
        public string Created { get; set; }


    }


}
