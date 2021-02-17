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
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Services.ExternalDataProviders
{
    /// <summary>
    /// This class is responsible for communicating with the external Github API.
    /// </summary>
    public class GithubDataSourceAdaptee : IAuthorizedDataSourceAdaptee, IPublicDataSourceAdaptee
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
        /// Initializes a new instance of the <see cref="GithubDataSourceAdaptee" /> class./>
        /// </summary>
        /// <param name="configuration">The configuration which is used to retrieve keys from the configuration file.</param>
        /// <param name="restClientFactory">The rest client factory which is used to create rest clients.</param>
        /// <param name="mapper">The mapper which is used to map Github resource results to projects.</param>
        public GithubDataSourceAdaptee(
            IConfiguration configuration,
            IRestClientFactory restClientFactory,
            IMapper mapper)
        {
            this.restClientFactory = restClientFactory;
            this.mapper = mapper;
            IConfigurationSection configurationSection = configuration.GetSection("App")
                                                                      .GetSection(Title);
            clientId = configurationSection.GetSection("ClientId")
                                           .Value;
            clientSecret = configurationSection.GetSection("ClientSecret")
                                               .Value;
            RedirectUri = configurationSection.GetSection("RedirectUri")
                                              .Value;

            OauthUrl = "https://github.com/login/oauth/authorize?client_id=" + clientId + $"&scope=repo";
        }

        /// <summary>
        /// Gets the value for the guid from the Github data source adaptee.
        /// </summary>
        public string Guid => "de38e528-1d6d-40e7-83b9-4334c51c19be";

        /// <summary>
        /// Gets or sets a value for the Title property from the Github data source adaptee.
        /// </summary>
        public string Title { get; set; } = "Github";

        /// <summary>
        /// Gets the value for the Base Url from the Github data source adaptee.
        /// </summary>
        public string BaseUrl { get; set; } = "https://api.github.com/";

        /// <summary>
        /// Gets or sets a value for the IsVisible property from the Github data source adaptee.
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets a value for the Icon property from the Github data source adaptee.
        /// </summary>
        public File Icon { get; set; }

        /// <summary>
        /// Gets or sets a value for the Description property from the Github data source adaptee.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value for the DataSourceWizardPages property from the Github data source adaptee.
        /// </summary>
        public IList<DataSourceWizardPage> DataSourceWizardPages { get; set; }

        /// <summary>
        /// Gets or sets a value for the OauthUrl property from the Github data source adaptee.
        /// </summary>
        public string OauthUrl { get; }

        /// <summary>
        /// Gets or sets a value for the RedirectUri property from the Gitlab data source adaptee.
        /// </summary>
        public string RedirectUri { get; }

        /// <summary>
        /// This method is responsible for retrieving Oauth tokens from the Github API.
        /// </summary>
        /// <param name="code">The code which is used to retrieve the Oauth tokens.</param>
        /// <returns>This method returns the Oauth tokens.</returns>
        /// <exception cref="ExternalException">This method throws the External Exception whenever the response is not successful.</exception>
        public async Task<OauthTokens> GetTokens(string code)
        {
            Uri baseGithubUrl = new Uri("https://github.com/");
            IRestClient client = restClientFactory.Create(baseGithubUrl);
            IRestRequest request = new RestRequest("login/oauth/access_token");
            client.Authenticator = new HttpBasicAuthenticator(clientId, clientSecret);
            request.AddParameter("code", code);
            request.AddParameter("redirect_uri", RedirectUri);
            IRestResponse response = await client.ExecuteAsync(request);

            if(string.IsNullOrEmpty(response.Content)) return null;
            if(!response.IsSuccessful) throw new ExternalException(response.ErrorMessage);

            OauthTokens tokens = JsonConvert.DeserializeObject<OauthTokens>(response.Content);
            return tokens;
        }

        /// <summary>
        /// This method is responsible for retrieving all projects from the user, via the access token, from the Github API.
        /// </summary>
        /// <param name="accessToken">The access token which will be used to retrieve all projects from the user.</param>
        /// <returns>This method returns a collection of projects from the user.</returns>
        public async Task<IEnumerable<Project>> GetAllProjects(string accessToken)
        {
            List<GithubDataSourceResourceResult> githubDataSources =
                (await FetchAllGithubProjects(accessToken)).ToList();
            List<Project> projects =
                mapper.Map<List<GithubDataSourceResourceResult>, List<Project>>(githubDataSources);
            for(int i = 0; i < projects.Count; i++)
            {
                GithubDataSourceResourceResult githubDatasource = githubDataSources[i];
                Project p = projects[i];
                p.Description = await FetchReadme(githubDatasource.Owner.Login, githubDatasource.Name, accessToken) ?? p.Description;
            }
            return projects;
        }

        /// <summary>
        /// This method is responsible for retrieving a project from the user, via the access token, by id from the Github API.
        /// </summary>
        /// <param name="accessToken">The access token which will be used to retrieve the correct project from the user.</param>
        /// <param name="projectId">The identifier of the project that will be used to search the correct project.</param>
        /// <returns>This method returns a project with this specified identifier.</returns>
        public async Task<Project> GetProjectById(string accessToken, string projectId)
        {
            IEnumerable<GithubDataSourceResourceResult> projects = await FetchAllGithubProjects(accessToken);
            GithubDataSourceResourceResult projectWithSpecifiedId = projects?.SingleOrDefault(resource => resource.Id.ToString() == projectId);
            if(projectWithSpecifiedId == null) return null;
            Project p = mapper.Map<GithubDataSourceResourceResult, Project>(projectWithSpecifiedId);
            p.Description = await FetchReadme(projectWithSpecifiedId.Owner.Login, projectWithSpecifiedId.Name, accessToken) ?? p.Description;
            return p;
        }

        /// <summary>
        /// This method is responsible for retrieving all the github repository contents.
        /// </summary>
        /// <param name="accessToken">The access token which is used to authenticate.</param>
        /// <returns>This method returns a collection of github data source resource results.</returns>
        /// <exception cref="ExternalException">This method could throw an external exception whenever the status code is not successful.</exception>
        private async Task<IEnumerable<GithubDataSourceResourceResult>> FetchAllGithubProjects(string accessToken)
        {
            IRestClient client = restClientFactory.Create(new Uri(BaseUrl));
            IRestRequest request = new RestRequest("user/repos", Method.GET);

            request.AddHeaders(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Authorization", $"Bearer {accessToken}"),
                new KeyValuePair<string, string>("accept", "application/vnd.github.v3+json")
            });

            request.AddQueryParameter("visibility", "all");
            IRestResponse response = await client.ExecuteAsync(request);

            if(string.IsNullOrEmpty(response.Content)) return null;
            if(!response.IsSuccessful) throw new ExternalException(response.ErrorMessage);
            IEnumerable<GithubDataSourceResourceResult> projects =
                JsonConvert.DeserializeObject<IEnumerable<GithubDataSourceResourceResult>>(response.Content);
            return projects;
        }

        /// <summary>
        /// This method is responsible for retrieving all public projects from a user, via the username, from the Github API.
        /// </summary>
        /// <param name="username">The username which will be used to search to retrieve all public projects from the user.</param>
        /// <returns>This method returns a collections of public projects from the user</returns>
        public async Task<IEnumerable<Project>> GetAllPublicProjects(string username)
        {
            GithubDataSourceResourceResult[] resourceResults = (await FetchAllPublicGithubRepositories(username)).ToArray();
            if(!resourceResults.Any()) return null;
            List<Project> projects = mapper.Map<IEnumerable<GithubDataSourceResourceResult>, IEnumerable<Project>>(resourceResults).ToList();
            for(int i = 0; i < projects.Count; i++)
            {
                GithubDataSourceResourceResult githubDatasource = resourceResults[i];
                Project p = projects[i];
                p.Description = await FetchReadme(githubDatasource.Owner.Login, githubDatasource.Name) ?? p.Description;
            }
            return projects;
        }

        /// <summary>
        /// This method is responsible for retrieving the content from all public repositories from a user.
        /// </summary>
        /// <param name="username">The username which is used to retrieve projects from the correct user.</param>
        /// <returns>This method returns a collection of github data source resource results.</returns>
        /// <exception cref="ExternalException">This method could throw an external exception whenever the status code is not successful.</exception>
        private async Task<IEnumerable<GithubDataSourceResourceResult>> FetchAllPublicGithubRepositories(string username)
        {
            IRestClient client = restClientFactory.Create(new Uri(BaseUrl));
            IRestRequest request = new RestRequest($"users/{username}/repos", Method.GET);
            IRestResponse response = await client.ExecuteAsync(request);

            if(string.IsNullOrEmpty(response.Content)) return null;
            if(!response.IsSuccessful) throw new ExternalException(response.ErrorMessage);
            IEnumerable<GithubDataSourceResourceResult> resourceResults =
                JsonConvert.DeserializeObject<IEnumerable<GithubDataSourceResourceResult>>(response.Content);
            return resourceResults;
        }

        /// <summary>
        /// This method is responsible for retrieving a public project from a uri, from the Github API.
        /// </summary>
        /// <param name="sourceUri">The source uri which will be used to retrieve the correct project.</param>
        /// <returns>This method returns a public project from the specified source uri.</returns>
        public async Task<Project> GetPublicProjectFromUri(Uri sourceUri)
        {
            GithubDataSourceResourceResult githubDataSource = await FetchPublicRepository(sourceUri);
            Project p = mapper.Map<GithubDataSourceResourceResult, Project>(githubDataSource);
            p.Description = await FetchReadme(githubDataSource.Owner.Login, githubDataSource.Name) ?? p.Description;
            return p;
        }

        /// <summary>
        /// This method is responsible for retrieving the content from a repository by uri.
        /// </summary>
        /// <param name="sourceUri">The source uri which is used to retrieve the correct project.</param>
        /// <returns>This method returns a github data source resource result from the specified uri.</returns>
        /// <exception cref="ExternalException">This method could throw an external exception whenever the status code is not successful.</exception>
        private async Task<GithubDataSourceResourceResult> FetchPublicRepository(Uri sourceUri)
        {
            string domain = sourceUri.GetLeftPart(UriPartial.Authority);

            // Get the project path without the prefix slash
            string projectPath = sourceUri.AbsolutePath.Replace(domain, "")
                                          .Substring(1);
            Uri serializedUri = new Uri(BaseUrl + "repos/" + projectPath);

            IRestClient client = restClientFactory.Create(serializedUri);
            RestRequest request = new RestRequest(Method.GET);
            IRestResponse response = await client.ExecuteAsync(request);

            if(string.IsNullOrEmpty(response.Content)) return null;
            if(!response.IsSuccessful) throw new ExternalException(response.ErrorMessage);
            return JsonConvert.DeserializeObject<GithubDataSourceResourceResult>(response.Content);
        }

        /// <summary>
        /// This method is responsible for retrieving a public project from the user, by id from the Github API.
        /// </summary>
        /// <param name="identifier">The identifier which will be used to retrieve the correct project.</param>
        /// <returns>This method returns a public project with the specified identifier.</returns>
        public async Task<Project> GetPublicProjectById(string identifier)
        {
            GithubDataSourceResourceResult projectResourceResult = await FetchPublicGithubProjectById(identifier);
            Project p = mapper.Map<GithubDataSourceResourceResult, Project>(projectResourceResult);
            p.Description = await FetchReadme(projectResourceResult.Owner.Login, projectResourceResult.Name) ?? p.Description;
            return p;
        }

        /// <summary>
        /// This method is responsible for retrieving the content from a public repositories from an identifier.
        /// </summary>
        /// <param name="identifier">The identifier which is used to retrieve the correct project.</param>
        /// <returns>This method returns a github data source resource result.</returns>
        /// <exception cref="ExternalException">This method could throw an external exception whenever the status code is not successful.</exception>
        private async Task<GithubDataSourceResourceResult> FetchPublicGithubProjectById(string identifier)
        {
            IRestClient client = restClientFactory.Create(new Uri(BaseUrl));
            IRestRequest request = new RestRequest($"repositories/{identifier}", Method.GET);
            IRestResponse response = await client.ExecuteAsync(request);

            if(string.IsNullOrEmpty(response.Content)) return null;
            if(!response.IsSuccessful) throw new ExternalException(response.ErrorMessage);
            GithubDataSourceResourceResult resourceResult =
                JsonConvert.DeserializeObject<GithubDataSourceResourceResult>(response.Content);
            return resourceResult;
        }

        /// <summary>
        /// This method is responsible for retrieving the readme from a repository.
        /// </summary>
        /// <param name="user">This parameter represents the owners of the repository. This is in most cases the name of the user
        /// or in some other case the name of the organization.</param>
        /// <param name="repository">This parameter represents the name of the repository.</param>
        /// <param name="accessToken">This parameter is the access token, which is used to retrieve a readme from a private
        /// repository. In case of a public repository, the default value is null.</param>
        /// <returns>This method returns the content of the readme.</returns>
        /// <exception cref="ExternalException">This method could throw an external exception whenever the status code is not successful.</exception>
        private async Task<string> FetchReadme(string user, string repository, string accessToken = null)
        {
            IRestClient client = restClientFactory.Create(new Uri(BaseUrl));
            IRestRequest request = new RestRequest($"repos/{user}/{repository}/readme");

            if(accessToken != null)
            {
                request.AddHeaders(new List<KeyValuePair<string, string>>
                                   {
                                       new KeyValuePair<string, string>("Authorization", $"Bearer {accessToken}"),
                                       new KeyValuePair<string, string>("accept", "application/vnd.github.v3+json")
                                   });
            }

            IRestResponse response = await client.ExecuteAsync(request);

            if(!response.IsSuccessful) throw new ExternalException(response.ErrorMessage);
            if(string.IsNullOrEmpty(response.Content)) return null;
            GithubDataSourceReadmeResourceResult resourceResult =
                JsonConvert.DeserializeObject<GithubDataSourceReadmeResourceResult>(response.Content);

            return await FetchReadmeContent(resourceResult.DownloadUrl);
        }

        /// <summary>
        /// This method is responsible for retrieving the content from the readme.
        /// </summary>
        /// <param name="downloadUri">The download uri parameter represents the uri where the content of the readme gets downloaded.</param>
        /// <returns>This method will return the content from the readme.</returns>
        /// <exception cref="ExternalException">This method could throw an external exception whenever the status code is not successful.</exception>
        private async Task<string> FetchReadmeContent(Uri downloadUri)
        {
            IRestClient client = restClientFactory.Create(downloadUri);
            IRestRequest request = new RestRequest(Method.GET);
            IRestResponse response = await client.ExecuteAsync(request);

            if(!response.IsSuccessful)
            {
                if(response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                throw new ExternalException(response.ErrorMessage);
            }

            return response.Content;
        }

    }

}
