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
using System.Net;
using System.Threading.Tasks;

namespace Services.DataProviders
{

    public class GitlabDataSourceAdaptee : IPublicDataSourceAdaptee
    {

        /// <summary>
        /// A factory that will generate a rest client to make API requests.
        /// </summary>
        private readonly IRestClientFactory restClientFactory;

        private readonly IMapper mapper;

        public GitlabDataSourceAdaptee(IRestClientFactory restClientFactory, IMapper mapper)
        {
            this.restClientFactory = restClientFactory;
            this.mapper = mapper;
        }

        public string Guid { get; }

        public string Name { get; }

        public string BaseUrl { get; }

        public Task<IEnumerable<Project>> GetAllPublicProjects()
        {
            throw new NotImplementedException();
        }

        public async Task<Project> GetPublicProjectFromUri(Uri sourceUri)
        {
            GitlabDataSourceResourceResult gitlabDataSource = await FetchPublicRepository(sourceUri);
            Project project = mapper.Map<GitlabDataSourceResourceResult, Project>(gitlabDataSource);
            project.Description = await FetchPublicReadme(gitlabDataSource.ReadmeUrl);
            return project;
        }

        private async Task<GitlabDataSourceResourceResult> FetchPublicRepository(Uri sourceUri)
        {
            IRestClient client = restClientFactory.Create(sourceUri);
            RestRequest request = new RestRequest(Method.GET);
            IRestResponse response = await client.ExecuteAsync(request);
            if(response.StatusCode != HttpStatusCode.OK || string.IsNullOrEmpty(response.Content))
            {
                return null;
            }
            return JsonConvert.DeserializeObject<GitlabDataSourceResourceResult>(response.Content);
        }

        private async Task<string> FetchPublicReadme(string readmeUrl)
        {
            readmeUrl = readmeUrl.Replace("blob", "raw");

            IRestClient client = restClientFactory.Create(new Uri(readmeUrl));
            RestRequest request = new RestRequest(Method.GET);
            IRestResponse response = await client.ExecuteAsync(request);
            return response.Content;
        }

    }

}
