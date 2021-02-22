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
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.ExternalDataProviders
{

    /// <summary>
    ///     The interface of the data provider adapter.
    /// </summary>
    public interface IDataProviderAdapter
    {

        /// <summary>
        ///     This method is responsible for retrieving all projects.
        /// </summary>
        /// <param name="token">The token parameters is used for finding all projects. This can be Oauth tokens, or the username.</param>
        /// <param name="needsAuth">The needsAuth parameter specifies which flow should get used.</param>
        /// <returns>This method returns a collection of projects.</returns>
        Task<IEnumerable<Project>> GetAllProjects(string token, bool needsAuth);

        /// <summary>
        ///     This method is responsible for retrieving a project by guid.
        /// </summary>
        /// <param name="token">The token parameters is used for finding all projects. This can be Oauth tokens, or the username.</param>
        /// <param name="id">The identifier which is used for searching an individual project.</param>
        /// <param name="needsAuth">The needsAuth parameter specifies which flow should get used.</param>
        /// <returns>This method returns a project with the specified identifier.</returns>
        Task<Project> GetProjectByGuid(string token, string id, bool needsAuth);

        /// <summary>
        ///     This method is responsible for retrieving a project by uri.
        /// </summary>
        /// <param name="sourceUri">The uri which is used for searching an individual project.</param>
        /// <returns>This method returns a project from the specified uri.</returns>
        Task<Project> GetProjectByUri(Uri sourceUri);

        /// <summary>
        ///     This method is responsible for return the Oauth url of the data source.
        /// </summary>
        /// <returns>This method returns a string containing the Oauth url.</returns>
        string GetOauthUrl();

        /// <summary>
        ///     This method is responsible for returning Oauth tokens from the specified code.
        /// </summary>
        /// <param name="code">The code which is used to retrieve Oauth tokens from the external data source.</param>
        /// <returns>This method returns the Oauth tokens from the external data source.</returns>
        Task<OauthTokens> GetTokens(string code);

    }

    public class DataProviderAdapter : IDataProviderAdapter
    {

        private readonly IDataSourceAdaptee adaptee;

        public DataProviderAdapter(IDataSourceAdaptee adaptee)
        {
            this.adaptee = adaptee;
        }

        /// <summary>
        ///     This method is responsible for retrieving all projects.
        /// </summary>
        /// <param name="token">The token parameters is used for finding all projects. This can be Oauth tokens, or the username.</param>
        /// <param name="needsAuth">The needsAuth parameter specifies which flow should get used.</param>
        /// <returns>This method returns a collection of projects.</returns>
        public async Task<IEnumerable<Project>> GetAllProjects(string token, bool needsAuth)
        {
            if(!needsAuth)
            {
                return await GetAllProjectsWithoutAccessToken(token);
            }

            return await GetAllProjectWithAccessToken(token);
        }

        /// <summary>
        ///     This method is responsible for retrieving a project by guid.
        /// </summary>
        /// <param name="token">The token parameters is used for finding all projects. This can be Oauth tokens, or the username.</param>
        /// <param name="id">The identifier which is used for searching an individual project.</param>
        /// <param name="needsAuth">The needsAuth parameter specifies which flow should get used.</param>
        /// <returns>This method returns a project with the specified identifier.</returns>
        public async Task<Project> GetProjectByGuid(string token, string id, bool needsAuth)
        {
            if(!needsAuth)
            {
                IPublicDataSourceAdaptee publicDataSource = adaptee as IPublicDataSourceAdaptee;
                if(publicDataSource == null)
                    throw new NotSupportedException("Can not cast specified adaptee to authorized adaptee.");
                return await publicDataSource.GetPublicProjectById(id);
            }
            IAuthorizedDataSourceAdaptee authorizedDataSource = adaptee as IAuthorizedDataSourceAdaptee;
            if(authorizedDataSource == null)
                throw new NotSupportedException("Can not cast specified adaptee to authorized adaptee.");
            return await authorizedDataSource.GetProjectById(token, id);
        }

        /// <summary>
        ///     This method is responsible for retrieving a project by uri.
        /// </summary>
        /// <param name="sourceUri">The uri which is used for searching an individual project.</param>
        /// <returns>This method returns a project from the specified uri.</returns>
        public async Task<Project> GetProjectByUri(Uri sourceUri)
        {
            IPublicDataSourceAdaptee publicDataSource = adaptee as IPublicDataSourceAdaptee;
            if(publicDataSource == null)
                throw new NotSupportedException("Can not cast specified adaptee to authorized adaptee.");
            return await publicDataSource.GetPublicProjectFromUri(sourceUri);
        }


        /// <summary>
        ///     This method is responsible for return the Oauth url of the data source.
        /// </summary>
        /// <returns>This method returns a string containing the Oauth url.</returns>
        public string GetOauthUrl()
        {
            IAuthorizedDataSourceAdaptee authorizedDataSource = adaptee as IAuthorizedDataSourceAdaptee;
            return authorizedDataSource?.OauthUrl;
        }

        /// <summary>
        ///     This method is responsible for returning Oauth tokens from the specified code.
        /// </summary>
        /// <param name="code">The code which is used to retrieve Oauth tokens from the external data source.</param>
        /// <returns>This method returns the Oauth tokens from the external data source.</returns>
        public async Task<OauthTokens> GetTokens(string code)
        {
            IAuthorizedDataSourceAdaptee dataProvider = adaptee as IAuthorizedDataSourceAdaptee;
            if(dataProvider == null)
                throw new NotSupportedException("Can not cast specified adaptee to authorized adaptee.");
            return await dataProvider.GetTokens(code);
        }

        private async Task<IEnumerable<Project>> GetAllProjectWithAccessToken(string accessToken)
        {
            IAuthorizedDataSourceAdaptee authorizedDataSourceAdaptee = adaptee as IAuthorizedDataSourceAdaptee;
            if(authorizedDataSourceAdaptee == null)
                throw new NotSupportedException("Can not cast specified adaptee to authorized adaptee.");
            IEnumerable<Project> projects = await authorizedDataSourceAdaptee.GetAllProjects(accessToken);
            return projects;
        }

        private async Task<IEnumerable<Project>> GetAllProjectsWithoutAccessToken(string username)
        {
            IPublicDataSourceAdaptee publicDataSourceAdaptee = adaptee as IPublicDataSourceAdaptee;
            if(publicDataSourceAdaptee == null)
                throw new NotSupportedException("Can not cast specified adaptee to authorized adaptee.");
            IEnumerable<Project> projects = await publicDataSourceAdaptee.GetAllPublicProjects(username);
            return projects;
        }

    }

}
