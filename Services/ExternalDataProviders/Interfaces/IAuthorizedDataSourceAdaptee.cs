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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.ExternalDataProviders
{

    /// <summary>
    /// The interface for a data source adaptee that supports the auth flow.
    /// </summary>
    public interface IAuthorizedDataSourceAdaptee : IDataSourceAdaptee
    {

        /// <summary>
        /// Gets or sets a value for the OauthUrl property from the data source adaptee.
        /// </summary>
        string OauthUrl { get; }

        /// <summary>
        /// This method is responsible for retrieving Oauth tokens from the external data source adaptee API.
        /// </summary>
        /// <param name="code">The code which is used to retrieve the Oauth tokens.</param>
        /// <returns>This method returns the Oauth tokens.</returns>
        Task<OauthTokens> GetTokens(string code);

        /// <summary>
        /// This method is responsible for retrieving all projects from the user, via the access token, from the Github API.
        /// </summary>
        /// <param name="accessToken">The access token which will be used to retrieve all projects from the user.</param>
        /// <returns>This method returns a collection of projects from the user.</returns>
        Task<IEnumerable<Project>> GetAllProjects(string accessToken);

        /// <summary>
        /// This method is responsible for retrieving a project from the user, via the access token, by id from the external data source adaptee API.
        /// </summary>
        /// <param name="accessToken">The access token which will be used to retrieve the correct project from the user.</param>
        /// <param name="projectId">The identifier of the project that will be used to search the correct project.</param>
        /// <returns>This method returns a project with this specified identifier.</returns>
        Task<Project> GetProjectById(string accessToken, string projectId);

    }

}
