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
using Microsoft.Extensions.Configuration;
using Models;
using Newtonsoft.Json;
using RestSharp;
using Services.DataProviders.Resources;
using Services.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Services.DataProviders
{

    public class GithubDataSourceAdaptee
    {
        /// <summary>
        /// A factory that will generate a rest client to make API requests.
        /// </summary>
        private readonly IRestClientFactory restClientFactory;

        /// <summary>
        /// Mapper object from auto mapper that will automatically maps one object to another.
        /// </summary>
        private readonly IMapper mapper;

        private readonly string clientSecret;
        private readonly string clientId;
        private readonly Uri baseUrl = new Uri("https://api.github.com/");

        public GithubDataSourceAdaptee(IConfiguration configuration, IRestClientFactory restClientFactory, IMapper mapper)
        {
            this.restClientFactory = restClientFactory;
            this.mapper = mapper;

            clientId = configuration.GetSection($"{Name}ClientId").Value;
            clientSecret = configuration.GetSection($"{Name}ClientSecret").Value;
            OauthUrl = "https://github.com/login/oauth/authorize?client_id=" + clientId + $"&scope=repo&state={Name}";
        }

        public string Guid => "de38e528-1d6d-40e7-83b9-4334c51c19be";

        public string Name => "Github";

        public string OauthUrl { get; }

        public async Task<OauthTokens> GetTokens(string code)
        {
            using HttpClient client = new HttpClient();
            Dictionary<string, string> accessRefreshTokenRequirements = new Dictionary<string, string>
            {
                { "client_id",  clientId },
                { "client_secret", clientSecret},
                {"code", code },
                {"state", Name }
            };
            FormUrlEncodedContent data = new FormUrlEncodedContent(accessRefreshTokenRequirements);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Ruby77");
            HttpResponseMessage response = await client.PostAsync("https://github.com/login/oauth/access_token", data);

            if(response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<OauthTokens>();

            throw new Exception(response.ReasonPhrase);
        }

        public async Task<IEnumerable<Project>> GetAllProjects(string accessToken)
        {
            IEnumerable<GithubDataSourceResourceResult> projects = await FetchAllGithubProjects(accessToken);
            if(projects == null) return null;
            return mapper.Map<IEnumerable<GithubDataSourceResourceResult>, IEnumerable<Project>>(projects);
        }

        public async Task<Project> GetProject(int projectId, string accessToken)
        {
            IEnumerable<GithubDataSourceResourceResult> projects = await FetchAllGithubProjects(accessToken);
            if(projects == null) return null;
            GithubDataSourceResourceResult projectWithSpecifiedId = projects.SingleOrDefault(project => project.Id == projectId);
            return mapper.Map<GithubDataSourceResourceResult, Project>(projectWithSpecifiedId);
        }

        private async Task<IEnumerable<GithubDataSourceResourceResult>> FetchAllGithubProjects(string accessToken)
        {
            IRestClient client = restClientFactory.Create(baseUrl);
            IRestRequest request = new RestRequest("user/repos", Method.GET);

            request.AddHeaders(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Authorization", $"Bearer {accessToken}"),
                new KeyValuePair<string, string>("accept", $"application/vnd.github.v3+json")
            });

            request.AddQueryParameter("visibility", "all");
            IRestResponse response = await client.ExecuteAsync(request);

            if(response.StatusCode != HttpStatusCode.OK ||
               string.IsNullOrEmpty(response.Content))
            {
                return null;
            }

            IEnumerable<GithubDataSourceResourceResult> projects =
                JsonConvert.DeserializeObject<IEnumerable<GithubDataSourceResourceResult>>(response.Content);
            return projects;
        }
    }

}
