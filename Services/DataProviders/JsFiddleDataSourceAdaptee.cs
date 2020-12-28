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

using AutoMapper;
using Models;
using Newtonsoft.Json;
using RestSharp;
using Services.DataProviders.Resources;
using Services.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Services.DataProviders
{

    public class JsFiddleDataSourceAdaptee : IPublicDataSourceAdaptee
    {
        /// <summary>
        /// A factory that will generate a rest client to make API requests.
        /// </summary>
        private readonly IRestClientFactory restClientFactory;

        /// <summary>
        /// Mapper object from auto mapper that will automatically maps one object to another.
        /// </summary>
        private readonly IMapper mapper;

        public JsFiddleDataSourceAdaptee(IRestClientFactory restClientFactory, IMapper mapper)
        {
            this.restClientFactory = restClientFactory;
            this.mapper = mapper;
        }

        public string Guid => "96666870-3afe-44e2-8d62-337d49cf972d";

        public string Name => "JsFiddle";

        public string BaseUrl { get; } = "http://jsfiddle.net/api/";

        public async Task<IEnumerable<Project>> GetAllPublicProjects(string username)
        {
            IEnumerable<JsFiddleDataSourceResourceResult> resourceResult = await FetchAllFiddlesFromUser(username);
            if(!resourceResult.Any()) return null;
            return mapper.Map<IEnumerable<JsFiddleDataSourceResourceResult>, IEnumerable<Project>>(resourceResult);
        }

        private async Task<IEnumerable<JsFiddleDataSourceResourceResult>> FetchAllFiddlesFromUser(string username)
        {
            IRestClient client = restClientFactory.Create(new Uri(BaseUrl));
            IRestRequest request = new RestRequest($"user/{username}/demo/list.json", Method.GET);
            IRestResponse response = await client.ExecuteAsync(request);

            if(response.StatusCode != HttpStatusCode.OK ||
               string.IsNullOrEmpty(response.Content))
            {
                return null;
            }

            IEnumerable<JsFiddleDataSourceResourceResult> resourceResult =
                JsonConvert.DeserializeObject<IEnumerable<JsFiddleDataSourceResourceResult>>(response.Content);
            return resourceResult;
        }

        public Task<Project> GetPublicProjectFromUri(Uri sourceUri)
        {
            return null;
        }

        public Task<Project> GetPublicProjectById(string identifier)
        {
            throw new NotImplementedException();
        }

    }

}
