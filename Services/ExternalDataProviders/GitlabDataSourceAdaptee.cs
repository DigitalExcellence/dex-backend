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
using RestSharp.Authenticators;
using Services.ExternalDataProviders.Resources;
using Services.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Services.ExternalDataProviders
{

    /// <summary>
    /// This class is responsible for communicating with the external Gitlab API.
    /// </summary>
    public class GitlabDataSourceAdaptee : IPublicDataSourceAdaptee, IAuthorizedDataSourceAdaptee
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

        /// <summary>
        /// Initializes a new instance of the <see cref="GitlabDataSourceAdaptee" /> class./>
        /// </summary>
        /// <param name="configuration">The configuration which is used to retrieve keys from the configuration file.</param>
        /// <param name="restClientFactory">The rest client factory which is used to create rest clients.</param>
        /// <param name="mapper">The mapper which is used to map Github resource results to projects.</param>
        public GitlabDataSourceAdaptee(
            IRestClientFactory restClientFactory,
            IMapper mapper,
            IConfiguration configuration)
        {
            this.restClientFactory = restClientFactory;
            this.mapper = mapper;
            IConfigurationSection configurationSection = configuration.GetSection("App")
                                                                      .GetSection("DataSources")
                                                                      .GetSection(Title);

            clientId = configurationSection.GetSection("ClientId")
                                           .Value;
            clientSecret = configurationSection.GetSection("ClientSecret")
                                               .Value;
            RedirectUri = configurationSection.GetSection("RedirectUri")
                                              .Value;

            OauthUrl =
                $"https://gitlab.com/oauth/authorize?client_id={clientId}&redirect_uri={RedirectUri}&response_type=code&scope=read_repository+read_user";
        }

        /// <summary>
        /// Gets the value for the guid from the Gitlab data source adaptee.
        /// </summary>
        public string Guid => "66de59d4-5db0-4bf8-a9a5-06abe8d3443a";

        /// <summary>
        /// Gets or sets a value for the Title property from the Gitlab data source adaptee.
        /// </summary>
        public string Title { get; set; } = "Gitlab";

        /// <summary>
        /// Gets the value for the Base Url from the Gitlab data source adaptee.
        /// </summary>
        public string BaseUrl { get; set; } = "https://gitlab.com/api/v4/";

        /// <summary>
        /// Gets or sets a value for the IsVisible property from the Gitlab data source adaptee.
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets a value for the Icon property from the Gitlab data source adaptee.
        /// </summary>
        public File Icon { get; set; }

        /// <summary>
        /// Gets or sets a value for the Description property from the Gitlab data source adaptee.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value for the DataSourceWizardPages property from the Gitlab data source adaptee.
        /// </summary>
        public IList<DataSourceWizardPage> DataSourceWizardPages { get; set; }

        /// <summary>
        /// Gets or sets a value for the OauthUrl property from the Gitlab data source adaptee.
        /// </summary>
        public string OauthUrl { get; }

        /// <summary>
        /// Gets or sets a value for the RedirectUri property from the Gitlab data source adaptee.
        /// </summary>
        public string RedirectUri { get;}

        /// <summary>
        /// This method is responsible for retrieving all public projects from a user, via the username, from the Gitlab API.
        /// </summary>
        /// <param name="username">The username which will be used to search to retrieve all public projects from the user.</param>
        /// <returns>This method returns a collections of public projects from the user</returns>
        public async Task<IEnumerable<Project>> GetAllPublicProjects(string username)
        {
            GitlabDataSourceResourceResult[] resourceResults =
                (await FetchAllPublicGitlabRepositories(username)).ToArray();
            if(!resourceResults.Any()) return null;
            return mapper.Map<IEnumerable<GitlabDataSourceResourceResult>, IEnumerable<Project>>(resourceResults);
        }

        /// <summary>
        /// This method is responsible for retrieving the content from all the public Gitlab projects from a user.
        /// </summary>
        /// <param name="username">The username which is used to retrieve the correct collection of projects from the specified user.</param>
        /// <returns>This method returns a collection of Gitlab data source resource results from the specified user.</returns>
        /// <exception cref="ExternalException">This method could throw an external exception whenever the status code is not successful.</exception>
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

        /// <summary>
        /// This method is responsible for retrieving a public project from a uri, from the Gitlab API.
        /// </summary>
        /// <param name="sourceUri">The source uri which will be used to retrieve the correct project.</param>
        /// <returns>This method returns a public project from the specified source uri.</returns>
        public async Task<Project> GetPublicProjectFromUri(Uri sourceUri)
        {
            GitlabDataSourceResourceResult resourceResult = await FetchPublicRepository(sourceUri);
            Project project = mapper.Map<GitlabDataSourceResourceResult, Project>(resourceResult);
            return project;
        }

        /// <summary>
        /// This method is responsible for retrieving the content from a public Gitlab project by uri.
        /// </summary>
        /// <param name="sourceUri">The  source uri which is used to retrieve the correct project.</param>
        /// <returns>This method returns a Gitlab data source resource result from the specified source uri.</returns>
        /// <exception cref="ExternalException">This method could throw an external exception whenever the status code is not successful.</exception>
        private async Task<GitlabDataSourceResourceResult> FetchPublicRepository(Uri sourceUri)
        {
            string domain = sourceUri.GetLeftPart(UriPartial.Authority);

            string projectPath = sourceUri.AbsolutePath.Replace(domain, "")
                                          .Substring(1).Replace("/", "%2F");
            Uri serializedUri = new Uri(BaseUrl + "projects/" + projectPath);

            IRestClient client = restClientFactory.Create(serializedUri);
            RestRequest request = new RestRequest(Method.GET);
            IRestResponse response = await client.ExecuteAsync(request);

            if(!response.IsSuccessful) throw new ExternalException(response.ErrorMessage);
            if(string.IsNullOrEmpty(response.Content)) return null;

            return JsonConvert.DeserializeObject<GitlabDataSourceResourceResult>(response.Content);
        }

        /// <summary>
        /// This method is responsible for retrieving a public project from the user, by id from the Gitlab API.
        /// </summary>
        /// <param name="identifier">The identifier which will be used to retrieve the correct project.</param>
        /// <returns>This method returns a public project with the specified identifier.</returns>
        public async Task<Project> GetPublicProjectById(string identifier)
        {
            GitlabDataSourceResourceResult resourceResult = await FetchPublicGitlabRepositoryById(identifier);
            return mapper.Map<GitlabDataSourceResourceResult, Project>(resourceResult);
        }

        /// <summary>
        /// This method is responsible for retrieving the content from a public Gitlab project by id.
        /// </summary>
        /// <param name="identifier">The identifier which is used to retrieve the correct project.</param>
        /// <returns>This method returns a Gitlab data source resource result with the specified identifier.</returns>
        /// <exception cref="ExternalException">This method could throw an external exception whenever the status code is not successful.</exception>
        private async Task<GitlabDataSourceResourceResult> FetchPublicGitlabRepositoryById(string identifier)
        {
            IRestClient client = restClientFactory.Create(new Uri(BaseUrl));
            RestRequest request = new RestRequest($"projects/{identifier}", Method.GET);
            IRestResponse response = await client.ExecuteAsync(request);

            if(!response.IsSuccessful) throw new ExternalException(response.ErrorMessage);
            if(string.IsNullOrEmpty(response.ContentType)) return null;

            GitlabDataSourceResourceResult resourceResult =
                JsonConvert.DeserializeObject<GitlabDataSourceResourceResult>(response.Content);
            return resourceResult;
        }

        /// <summary>
        /// This method is responsible for retrieving Oauth tokens from the Gitlab API.
        /// </summary>
        /// <param name="code">The code which is used to retrieve the Oauth tokens.</param>
        /// <returns>This method returns the Oauth tokens.</returns>
        /// <exception cref="ExternalException">This method throws the External Exception whenever the response is not successful.</exception>
        public async Task<OauthTokens> GetTokens(string code)
        {
            Uri baseUriGitlab = new Uri("https://gitlab.com/");
            IRestClient client = restClientFactory.Create(baseUriGitlab);
            IRestRequest request = new RestRequest("oauth/token");
            client.Authenticator = new HttpBasicAuthenticator(clientId, clientSecret);
            request.AddParameter("code", code);
            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("redirect_uri", RedirectUri);
            IRestResponse response = await client.ExecuteAsync(request);

            if(!response.IsSuccessful) throw new ExternalException(response.ErrorMessage);
            if(string.IsNullOrEmpty(response.Content)) return null;

            OauthTokens tokens = JsonConvert.DeserializeObject<OauthTokens>(response.Content);
            return tokens;
        }

        /// <summary>
        /// This method is responsible for retrieving all projects from a user.
        /// </summary>
        /// <param name="accessToken">The access token which is used to authenticate and retrieve private projects.</param>
        /// <returns>This method returns all the projects from a user.</returns>
        public async Task<IEnumerable<Project>> GetAllProjects(string accessToken)
        {
            GitlabDataSourceUserResourceResult user = await FetchUserFromAccessToken(accessToken);
            List<GitlabDataSourceResourceResult> gitlabDataSources =
                (await FetchAllGitlabRepositories(accessToken, user.Username)).ToList();
            List<Project> projects =
                mapper.Map<List<GitlabDataSourceResourceResult>, List<Project>>(gitlabDataSources);
            return projects;
        }

        /// <summary>
        /// This method is responsible for retrieving all the gitlab project contents.
        /// </summary>
        /// <param name="accessToken">The access token which is used to authenticate.</param>
        /// <param name="username">The username which is used to retrieve the correct collection of projects.</param>
        /// <returns>This method returns a collection of Gitlab data source resource results.</returns>
        /// <exception cref="ExternalException">This method could throw an external exception whenever the status code is not successful.</exception>
        private async Task<IEnumerable<GitlabDataSourceResourceResult>> FetchAllGitlabRepositories(string accessToken, string username)
        {
            IRestClient client = restClientFactory.Create(new Uri(BaseUrl));
            IRestRequest request = new RestRequest($"users/{username}/projects");
            request.AddHeader("Authorization", $"Bearer {accessToken}");
            IRestResponse response = await client.ExecuteAsync(request);

            if(!response.IsSuccessful) throw new ExternalException(response.ErrorMessage);
            if(string.IsNullOrEmpty(response.Content)) return null;

            IEnumerable<GitlabDataSourceResourceResult> resourceResults =
                JsonConvert.DeserializeObject<IEnumerable<GitlabDataSourceResourceResult>>(response.Content);
            return resourceResults;
        }

        /// <summary>
        /// This method is responsible for retrieving a project from the user, via the access token, by id from the Gitlab API.
        /// </summary>
        /// <param name="accessToken">The access token which will be used to retrieve the correct project from the user.</param>
        /// <param name="projectId">The identifier of the project that will be used to search the correct project.</param>
        /// <returns>This method returns a project with this specified identifier.</returns>
        public async Task<Project> GetProjectById(string accessToken, string projectId)
        {
            GitlabDataSourceResourceResult resourceResult = await FetchGitlabRepositoryById(projectId, accessToken);
            return mapper.Map<GitlabDataSourceResourceResult, Project>(resourceResult);
        }

        /// <summary>
        /// This method is responsible for retrieving the content from a public Gitlab project by id.
        /// </summary>
        /// <param name="identifier">The identifier which is used to retrieve the correct project.</param>
        /// <returns>This method returns a Gitlab data source resource result with the specified identifier.</returns>
        /// <exception cref="ExternalException">This method could throw an external exception whenever the status code is not successful.</exception>
        private async Task<GitlabDataSourceResourceResult> FetchGitlabRepositoryById(string identifier, string accessToken)
        {
            IRestClient client = restClientFactory.Create(new Uri(BaseUrl));
            RestRequest request = new RestRequest($"projects/{identifier}", Method.GET);
            request.AddHeader("Authorization", $"Bearer {accessToken}");
            IRestResponse response = await client.ExecuteAsync(request);

            if(!response.IsSuccessful) throw new ExternalException(response.ErrorMessage);
            if(string.IsNullOrEmpty(response.ContentType)) return null;

            GitlabDataSourceResourceResult resourceResult =
                JsonConvert.DeserializeObject<GitlabDataSourceResourceResult>(response.Content);
            return resourceResult;
        }

        /// <summary>
        /// This method is responsible for retrieving a user from the specified access token.
        /// </summary>
        /// <param name="accessToken">The access token parameter which is used to retrieve the correct user.</param>
        /// <returns>This method returns the user from the access token.</returns>
        /// <exception cref="ExternalException">This method could throw an external exception whenever the status code is not successful.</exception>
        private async Task<GitlabDataSourceUserResourceResult> FetchUserFromAccessToken(string accessToken)
        {
            IRestClient client = restClientFactory.Create(new Uri(BaseUrl));
            IRestRequest request = new RestRequest("user");
            request.AddHeader("Authorization", $"Bearer {accessToken}");
            IRestResponse response = await client.ExecuteAsync(request);

            if(!response.IsSuccessful) throw new ExternalException(response.ErrorMessage);
            if(string.IsNullOrEmpty(response.Content)) return null;

            GitlabDataSourceUserResourceResult userResourceResult =
                JsonConvert.DeserializeObject<GitlabDataSourceUserResourceResult>(response.Content);
            return userResourceResult;
        }

    }

}
