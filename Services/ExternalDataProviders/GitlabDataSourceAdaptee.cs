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
using Services.ExternalDataProviders.Resources;
using Services.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Services.ExternalDataProviders
{

    public class GitlabDataSourceAdaptee : IAuthorizedDataSourceAdaptee, IPublicDataSourceAdaptee
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
            Uri requestUri = new Uri(BaseUrl+"users/"+username+"/projects");

            IRestClient client = restClientFactory.Create(requestUri);
            RestRequest request = new RestRequest(Method.GET);

            IRestResponse response = await client.ExecuteAsync(request);
            if(response.StatusCode != HttpStatusCode.OK || string.IsNullOrEmpty(response.Content))
            {
                return null;
            }
            IEnumerable<GitlabDataSourceResourceResult> gitlabDataSourceResourceResults =  JsonConvert.DeserializeObject<IEnumerable<GitlabDataSourceResourceResult>>(response.Content);
            IEnumerable<Project> projects = mapper.Map<IEnumerable<GitlabDataSourceResourceResult>, IEnumerable<Project>>(gitlabDataSourceResourceResults);

            return projects;
        }

        public async Task<Project> GetPublicProjectFromUri(Uri sourceUri)
        {
            Uri requestUri = ConvertUri(sourceUri);


            IRestClient client = restClientFactory.Create(requestUri);
            RestRequest request = new RestRequest(Method.GET);

            IRestResponse response = await client.ExecuteAsync(request);
            if(response.StatusCode != HttpStatusCode.OK || string.IsNullOrEmpty(response.Content))
            {
                return null;
            }
            GitlabDataSourceResourceResult gitlabDataSourceResourceResults = JsonConvert.DeserializeObject<GitlabDataSourceResourceResult>(response.Content);
            Project project = mapper.Map<GitlabDataSourceResourceResult, Project>(gitlabDataSourceResourceResults);

            return project;
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
            Uri requestUri = new Uri(BaseUrl + "projects/" + identifier);

            IRestClient client = restClientFactory.Create(requestUri);
            RestRequest request = new RestRequest(Method.GET);

            IRestResponse response = await client.ExecuteAsync(request);
            if(response.StatusCode != HttpStatusCode.OK || string.IsNullOrEmpty(response.Content))
            {
                return null;
            }
            GitlabDataSourceResourceResult gitlabDataSourceResourceResults = JsonConvert.DeserializeObject<GitlabDataSourceResourceResult>(response.Content);
            Project project = mapper.Map<GitlabDataSourceResourceResult, Project>(gitlabDataSourceResourceResults);

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
