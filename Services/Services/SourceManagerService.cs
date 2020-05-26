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
using Services.Sources;

namespace Services.Services
{
    /// <summary>
    /// SourceManagerService
    /// </summary>
    public class SourceManagerService
    {
        /// <summary>
        /// The git lab source
        /// </summary>
        private readonly ISource gitLabSource;
        /// <summary>
        /// The git hub source
        /// </summary>
        private readonly ISource gitHubSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceManagerService"/> class.
        /// </summary>
        /// <param name="gitlabSource">The gitlab source.</param>
        /// <param name="githubSource">The github source.</param>
        public SourceManagerService(GitLabSource gitlabSource, GitHubSource githubSource)
        {
            this.gitLabSource = gitlabSource;
            this.gitHubSource = githubSource;

        }
        /// <summary>
        /// Fetches the project.
        /// </summary>
        /// <param name="repoUrl">The repo URL.</param>
        /// <returns></returns>
        public Project FetchProject(string repoUrl)
        {

            if(repoUrl.StartsWith("https://github.com"))
            {
                return gitHubSource.GetProjectInformation(repoUrl);
            } else if(repoUrl.StartsWith("https://gitlab.com"))
            {
                return gitLabSource.GetProjectInformation(repoUrl);
            } else
            {
                return null;
            }
        }

    }

    }

