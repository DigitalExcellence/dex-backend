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

using Models;
using Newtonsoft.Json;
using RestSharp;
using Services.Sources.Resources;
using System;
using System.Text;

namespace Services.Sources
{
    /// <summary>
    /// GitlabSource
    /// </summary>
    /// <seealso cref="ISource" />
    public class GitLabSource : ISource
    {
        /// <summary>
        /// The gitlab API URL
        /// </summary>
        private readonly string gitlabApiUrl = "https://gitlab.com/api/v4/projects/";

        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void GetSource(string url)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the project information.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public Project GetProjectInformation(string url)
        {
            string serializedUrl = this.gitlabApiUrl +
                            new StringBuilder(url)
                                .Replace("https://gitlab.com/", "")
                                .Replace("/", "%2F");

            Project project = new Project();

            GitLabResourceResult resourceResult = FetchRepo(serializedUrl);
            project.Name = resourceResult.name;
            project.ShortDescription = resourceResult.description;
            project.Uri = url;
            if(!string.IsNullOrEmpty(resourceResult.readme_url))
            {
                project.Description = FetchReadme(resourceResult.readme_url);
            }

            return project;
        }

        /// <summary>
        /// Searches the specified search term.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void Search(string searchTerm)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fetches the repo.
        /// </summary>
        /// <param name="repoUrl">The repo URL.</param>
        /// <returns></returns>
        private GitLabResourceResult FetchRepo(string repoUrl)
        {
            RestClient client = new RestClient(repoUrl);
            RestRequest request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<GitLabResourceResult>(response.Content);
        }

        /// <summary>
        /// Fetches the readme.
        /// </summary>
        /// <param name="readmeUrl">The readme URL.</param>
        /// <returns></returns>
        private string FetchReadme(string readmeUrl)
        {
            readmeUrl = readmeUrl.Replace("-/blob", "-/raw");
            RestClient client = new RestClient(readmeUrl);
            RestRequest request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            return response.Content;
        }
    }
}
