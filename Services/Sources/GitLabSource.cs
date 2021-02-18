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
using System.Net;
using System.Text.RegularExpressions;

namespace Services.Sources
{

    /// <summary>
    ///     This is the interface of the GitLab source
    /// </summary>
    public interface IGitLabSource : ISource
    {
        /// <summary>
        /// Fetches the repo.
        /// </summary>
        /// <param name="sourceUri">The source URI.</param>
        /// <returns>Returns the gitlabresourceresult.</returns>
        GitLabResourceResult FetchRepo(Uri sourceUri);

        /// <summary>
        /// Fetches the readme.
        /// </summary>
        /// <param name="readmeUrl">The readme URL.</param>
        /// <returns>the readme content.</returns>
        string FetchReadme(string readmeUrl);
    }

    /// <summary>
    /// GitlabSource
    /// </summary>
    /// <seealso cref="ISource" />
    public class GitLabSource : IGitLabSource
    {
        /// <summary>
        /// The rest client
        /// </summary>
        private readonly IRestClientFactory restClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitLabSource"/> class.
        /// </summary>
        /// <param name="restClientFactory"></param>
        public GitLabSource(IRestClientFactory restClientFactory)
        {
            this.restClientFactory = restClientFactory;
        }

        /// <summary>
        /// The gitlab API URL
        /// </summary>
        private readonly string gitlabApiUri = "/api/v4/projects/";

        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void GetSource(Uri uri)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the project information.
        /// </summary>
        /// <param name="sourceUri">The source URI.</param>
        /// <returns>the project object filled with information retrieved online.</returns>
        public Project GetProjectInformation(Uri sourceUri)
        {
            // Create valid URL
            try
            {
                Uri.TryCreate(sourceUri.AbsoluteUri, UriKind.Absolute, out sourceUri);
            } catch(InvalidOperationException)
            {
                Uri.TryCreate("https://" + sourceUri.ToString(), UriKind.Absolute, out sourceUri);
            }
            string domain = sourceUri.GetLeftPart(UriPartial.Authority);
            
            // Get the project path without the prefix slash.
            string projectPath = sourceUri.AbsoluteUri.Replace(domain, "").Substring(1);
            Uri serializedUrl = new Uri(domain + gitlabApiUri + projectPath.Replace("/", "%2F"));

            Project project = new Project();

            GitLabResourceResult resourceResult = FetchRepo(serializedUrl);
            if(resourceResult == null)
            {
                return project;
            }
            project.Name = resourceResult.Name;
            project.ShortDescription = resourceResult.Description;
            project.Uri = resourceResult.WebUrl;
            if(!string.IsNullOrEmpty(resourceResult.ReadmeUrl))
            {
                project.Description = FetchReadme(resourceResult.ReadmeUrl);
            }

            return project;
        }

        /// <summary>
        ///     This method checks if the project uri matches
        /// </summary>
        /// <param name="sourceUri"></param>
        /// <returns></returns>
        public bool ProjectURIMatches(Uri sourceUri)
        {
            //Create a valid URL
            try
            {
                Uri.TryCreate(sourceUri.AbsoluteUri, UriKind.Absolute, out sourceUri);
            } catch(InvalidOperationException)
            {
                Uri.TryCreate("https://" + sourceUri.ToString(), UriKind.Absolute, out sourceUri);
            }
            /*
             * This regex matches the following url schemas:
             * http://domain.com/group/project
             * https://domain.com/group/project
             * http://www.domain.com/group/project
             * https://www.domain.com/group/project
             * domain.com/group/project
             * www.domain.com/group/project
             * http://domain.com:123/group/project
             * https://domain.com:123/group/project
             * http://1.2.3.4/group/project
             * https://1.2.3.4/group/project
             * http://1.2.3.4:123/group/project
             * https://1.2.3.4:123/group/project
             * 1.2.3.4/group/project
             * 1.2.3.4:123/group/project
            */
            bool domainMatch = new Regex(@"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)(:\d+)?\/.+\/.+", RegexOptions.Compiled | RegexOptions.IgnoreCase).Match(sourceUri.AbsoluteUri).Success;

            if(!domainMatch) return false;

            IRestClient client = restClientFactory.Create(sourceUri);
            RestRequest request = new RestRequest(Method.GET);
            request.AddHeader("accept", "*/*");
            IRestResponse response = client.Execute(request);
            return response.Content.Contains("<meta content=\"GitLab\" property=\"og:site_name\">");
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
        /// <param name="sourceUri">The source URI.</param>
        /// <returns>Returns the gitlabresourceresult.</returns>
        public GitLabResourceResult FetchRepo(Uri sourceUri)
        {
            IRestClient client = restClientFactory.Create(sourceUri);
            RestRequest request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            if(response.StatusCode != HttpStatusCode.OK || string.IsNullOrEmpty(response.Content))
            {
                return null;
            }
            return JsonConvert.DeserializeObject<GitLabResourceResult>(response.Content);
        }

        /// <summary>
        /// Fetches the readme.
        /// </summary>
        /// <param name="readmeUrl">The readme URL.</param>
        /// <returns>the readme content.</returns>
        public string FetchReadme(string readmeUrl)
        {
            readmeUrl = readmeUrl.Replace("blob", "raw");

            IRestClient client = restClientFactory.Create(new Uri(readmeUrl));
            RestRequest request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            return response.Content;
        }
    }
}
