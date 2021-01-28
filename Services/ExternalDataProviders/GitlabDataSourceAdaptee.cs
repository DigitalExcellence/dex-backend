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
using RestSharp.Extensions;
using Services.ExternalDataProviders.Resources;
using Services.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Services.ExternalDataProviders
{

    public class GitlabDataSourceAdaptee : IAuthorizedDataSourceAdaptee, IPublicDataSourceAdaptee
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

        public GitlabDataSourceAdaptee(
            IRestClientFactory restClientFactory,
            IMapper mapper,
            IConfiguration configuration)
        {
            this.restClientFactory = restClientFactory;
            this.mapper = mapper;
            IConfigurationSection configurationSection = configuration.GetSection("App")
                                                                      .GetSection(Title);

            clientId = configurationSection.GetSection("ClientId")
                                           .Value;
            clientSecret = configurationSection.GetSection("ClientSecret")
                                               .Value;
            OauthUrl = "";
        }

        public string Guid => "66de59d4-5db0-4bf8-a9a5-06abe8d3443a";

        public string Title { get; set; } = "Gitlab";

        public string BaseUrl { get; set; } = "https://gitlab.com/api/v4/";

        public bool IsVisible { get; set; }

        public File Icon { get; set; }

        public string Description { get; set; }

        public IList<DataSourceWizardPage> DataSourceWizardPages { get; set; }

        public string OauthUrl { get; }

        public async Task<IEnumerable<Project>> GetAllPublicProjects(string username)
        {
            GitlabDataSourceResourceResult[] resourceResults =
                (await FetchAllPublicGitlabRepositories(username)).ToArray();
            if(!resourceResults.Any()) return null;
            return mapper.Map<IEnumerable<GitlabDataSourceResourceResult>, IEnumerable<Project>>(resourceResults);
        }

        private async Task<IEnumerable<GitlabDataSourceResourceResult>> FetchAllPublicGitlabRepositories(
            string username)
        {
            IRestClient client = restClientFactory.Create(new Uri(BaseUrl));
            IRestRequest request = new RestRequest($"users/{username}/projects");
            IRestResponse response = await client.ExecuteAsync(request);

            if(string.IsNullOrEmpty(response.Content)) return null;
            if(!response.IsSuccessful) throw new ExternalException(response.ErrorMessage);

            IEnumerable<GitlabDataSourceResourceResult> resourceResults =
                JsonConvert.DeserializeObject<IEnumerable<GitlabDataSourceResourceResult>>(response.Content);
            return resourceResults;
        }

        public async Task<Project> GetPublicProjectFromUri(Uri sourceUri)
        {
            GitlabDataSourceResourceResult resourceResult = await FetchPublicRepository(sourceUri);
            Project project = mapper.Map<GitlabDataSourceResourceResult, Project>(resourceResult);
            return project;
        }

        private async Task<GitlabDataSourceResourceResult> FetchPublicRepository(Uri sourceUri)
        {
            string domain = sourceUri.GetLeftPart(UriPartial.Authority);

            string projectPath = sourceUri.AbsolutePath.Replace(domain, "")
                                          .Substring(1);
            Uri serializedUri = new Uri(BaseUrl + "repos/" + projectPath);

            IRestClient client = restClientFactory.Create(serializedUri);
            RestRequest request = new RestRequest(Method.GET);
            IRestResponse response = await client.ExecuteAsync(request);

            if(string.IsNullOrEmpty(response.Content)) return null;
            if(!response.IsSuccessful) throw new ExternalException(response.ErrorMessage);

            return JsonConvert.DeserializeObject<GitlabDataSourceResourceResult>(response.Content);
        }

        //Convert uri from normal web uri to api uri
        private Uri ConvertUri(Uri sourceUri)
        {
            string uriString = sourceUri.ToString();
            string cleanUri = uriString.Replace("https://gitlab.com/", "");
            string[] separatingStrings = { "/" };
            string[] splitted = cleanUri.ToString().Split(separatingStrings, StringSplitOptions.RemoveEmptyEntries);

            string seperator = "%2F";
            string convertedStringUri = "https://gitlab.com/api/v4/projects/";

            for(int i = 0; i < splitted.Count(); i++)
            {
                if(i == splitted.Count()-1)
                {
                    string temp = convertedStringUri + splitted[i];
                    convertedStringUri = temp;
                } else
                {
                    string temp = convertedStringUri + splitted[i] + seperator;
                    convertedStringUri = temp;
                }
            }
            Uri convertedUri = new Uri(convertedStringUri);
           return convertedUri;
        }

        public async Task<Project> GetPublicProjectById(string identifier)
        {
            GitlabDataSourceResourceResult resourceResult = await FetchGitlabRepositoryById(identifier);
            return mapper.Map<GitlabDataSourceResourceResult, Project>(resourceResult);
        }

        private async Task<GitlabDataSourceResourceResult> FetchGitlabRepositoryById(string identifier)
        {
            IRestClient client = restClientFactory.Create(new Uri(BaseUrl));
            RestRequest request = new RestRequest($"projects/{identifier}", Method.GET);
            IRestResponse response = await client.ExecuteAsync(request);

            if(string.IsNullOrEmpty(response.ContentType)) return null;
            if(!response.IsSuccessful) throw new ExternalException(response.ErrorMessage);

            GitlabDataSourceResourceResult resourceResult =
                JsonConvert.DeserializeObject<GitlabDataSourceResourceResult>(response.Content);
            return resourceResult;
        }

        private async Task<string> FetchPublicReadme(string readmeUrl)
        {
            readmeUrl = readmeUrl.Replace("blob", "raw");

            IRestClient client = restClientFactory.Create(new Uri(readmeUrl));
            RestRequest request = new RestRequest(Method.GET);
            IRestResponse response = await client.ExecuteAsync(request);
            return response.Content;
        }

        public Task<OauthTokens> GetTokens(string code)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Project>> GetAllProjects(string accessToken)
        {
            throw new NotImplementedException();
        }

        public Task<Project> GetProjectById(string accessToken, string projectId)
        {
            throw new NotImplementedException();
        }

    }

}
