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
using System.Linq;
using System.Threading.Tasks;

namespace Services.ExternalDataProviders
{

    /// <summary>
    ///     The interface of the data provider service.
    /// </summary>
    public interface IDataProviderService
    {

        /// <summary>
        ///     This method is responsible for retrieving all projects from a data source.
        /// </summary>
        /// <param name="dataSourceGuid">The data source guid that specifies which data source should get used.</param>
        /// <param name="token">The token which is used for retrieving all the projects. This can be a username or Oauth tokens.</param>
        /// <param name="needsAuth">The needsAuth parameter specifies which flow should get used.</param>
        /// <returns>This method returns a collection of projects.</returns>
        Task<IEnumerable<Project>> GetAllProjects(string dataSourceGuid, string token, bool needsAuth);

        /// <summary>
        ///     This method is responsible for retrieving a project by id.
        /// </summary>
        /// <param name="dataSourceGuid">The data source guid that specifies which data source should get used.</param>
        /// <param name="token">The token which is used for retrieving all the projects. This can be a username or Oauth tokens.</param>
        /// <param name="id">The id of the project which will get used for searching the correct project.</param>
        /// <param name="needsAuth">The needsAuth parameter specifies which flow should get used.</param>
        /// <returns>This method returns a project with the specified identifier.</returns>
        Task<Project> GetProjectById(string dataSourceGuid, string token, int id, bool needsAuth);

        /// <summary>
        ///     This method validates whether a data source with the specified guid exists.
        /// </summary>
        /// <param name="dataSourceGuid">The data source guid that will get checked.</param>
        /// <returns>This method return whether the data source exists or does not exists.</returns>
        bool IsExistingDataSourceGuid(string dataSourceGuid);

        /// <summary>
        ///     This method is responsible for retrieving a project by uri.
        /// </summary>
        /// <param name="dataSourceGuid">The data source guid that specifies which data source should get used.</param>
        /// <param name="sourceUri">The source uri of the project which will get used for retrieving the correct project.</param>
        /// <returns>This method returns a project from the specified uri.</returns>
        Task<Project> GetProjectFromUri(string dataSourceGuid, string sourceUri);

        /// <summary>
        ///     This method is responsible for retrieving the oauth url from the specified data source.
        /// </summary>
        /// <param name="dataSourceGuid">The data source guid that specifies which data source should get used.</param>
        /// <returns>This method returns the oauth url from the specified data source.</returns>
        Task<string> GetOauthUrl(string dataSourceGuid);

        /// <summary>
        ///     This method is responsible for returning the Oauth url from the specified  data source.
        /// </summary>
        /// <param name="code">The code which is used to retrieve Oauth tokens from the external data source.</param>
        /// <param name="dataSourceGuid">The data source guid that specifies which data source should get used.</param>
        /// <returns>This method returns the Oauth tokens from the external data source.</returns>
        Task<OauthTokens> GetTokens(string code, string dataSourceGuid);

        /// <summary>
        ///     This method is responsible for returning the data sources.
        /// </summary>
        /// <param name="needsAuth">
        ///     The needsAuth parameter specifies whether the returned data sources should follow a specific
        ///     auth flow.
        /// </param>
        /// <returns>This method returns a collection of data source adaptees.</returns>
        Task<IEnumerable<IDataSourceAdaptee>> RetrieveDataSources(bool? needsAuth);

        /// <summary>
        ///     This method is responsible for retrieving a data source by guid.
        /// </summary>
        /// <param name="guid">The data source guid that specifies which data source should get retrieved.</param>
        /// <returns>This method returns the data source with the specified guid.</returns>
        Task<IDataSourceAdaptee> RetrieveDataSourceByGuid(string guid);

        /// <summary>
        ///     This method is responsible for retrieving a data source by name.
        /// </summary>
        /// <param name="name">The name of the data source that specified which data source should get retrieved.</param>
        /// <returns>This method returns the data source with the specified name.</returns>
        Task<IDataSourceAdaptee> RetrieveDataSourceByName(string name);

    }

    /// <summary>
    ///     The data provider service which communicates with the correct data source. This service
    ///     acts as the adapter in the adapter pattern.
    /// </summary>
    /// <seealso cref="IDataProviderService" />
    public class DataProviderService : IDataProviderService
    {

        private readonly IDataProviderLoader dataProviderLoader;
        private IDataProviderAdapter dataProviderAdapter;

        /// <summary>
        ///     The data provider service constructor
        /// </summary>
        /// <param name="dataProviderLoader"></param>
        public DataProviderService(IDataProviderLoader dataProviderLoader)
        {
            this.dataProviderLoader = dataProviderLoader;
        }

        /// <summary>
        ///     This method is responsible for retrieving all projects from a data source.
        /// </summary>
        /// <param name="dataSourceGuid">The data source guid that specifies which data source should get used.</param>
        /// <param name="token">The token which is used for retrieving all the projects. This can be a username or Oauth tokens.</param>
        /// <param name="needsAuth">The needsAuth parameter specifies which flow should get used.</param>
        /// <returns>This method returns a collection of projects.</returns>
        public async Task<IEnumerable<Project>> GetAllProjects(string dataSourceGuid, string token, bool needsAuth)
        {
            IDataSourceAdaptee adaptee = await dataProviderLoader.GetDataSourceByGuid(dataSourceGuid);
            dataProviderAdapter = new DataProviderAdapter(adaptee);
            return await dataProviderAdapter.GetAllProjects(token, needsAuth);
        }

        /// <summary>
        ///     This method is responsible for retrieving a project by id.
        /// </summary>
        /// <param name="dataSourceGuid">The data source guid that specifies which data source should get used.</param>
        /// <param name="token">The token which is used for retrieving all the projects. This can be a username or Oauth tokens.</param>
        /// <param name="id">The id of the project which will get used for searching the correct project.</param>
        /// <param name="needsAuth">The needsAuth parameter specifies which flow should get used.</param>
        /// <returns>This method returns a project with the specified identifier.</returns>
        public async Task<Project> GetProjectById(string dataSourceGuid, string token, int id, bool needsAuth)
        {
            IDataSourceAdaptee adaptee = await dataProviderLoader.GetDataSourceByGuid(dataSourceGuid);
            dataProviderAdapter = new DataProviderAdapter(adaptee);
            return await dataProviderAdapter.GetProjectByGuid(token, id.ToString(), needsAuth);
        }

        /// <summary>
        ///     This method validates whether a data source with the specified guid exists.
        /// </summary>
        /// <param name="dataSourceGuid">The data source guid that will get checked.</param>
        /// <returns>This method return whether the data source exists or does not exists.</returns>
        public bool IsExistingDataSourceGuid(string dataSourceGuid)
        {
            return dataProviderLoader.GetDataSourceByGuid(dataSourceGuid) != null;
        }

        /// <summary>
        ///     This method is responsible for retrieving a project by uri.
        /// </summary>
        /// <param name="dataSourceGuid">The data source guid that specifies which data source should get used.</param>
        /// <param name="sourceUri">The source uri of the project which will get used for retrieving the correct project.</param>
        /// <returns>This method returns a project from the specified uri.</returns>
        public async Task<Project> GetProjectFromUri(string dataSourceGuid, string sourceUri)
        {
            IDataSourceAdaptee adaptee = await dataProviderLoader.GetDataSourceByGuid(dataSourceGuid);
            dataProviderAdapter = new DataProviderAdapter(adaptee);

            sourceUri = sourceUri.Replace("%2F", "/");
            if(sourceUri[^1] == '/') sourceUri = sourceUri.Remove(sourceUri.Length - 1);
            Uri serializedUrl;

            try
            {
                Uri.TryCreate(sourceUri, UriKind.RelativeOrAbsolute, out serializedUrl);
            } catch(InvalidOperationException)
            {
                Uri.TryCreate("https://" + sourceUri, UriKind.Absolute, out serializedUrl);
            }

            return await dataProviderAdapter.GetProjectByUri(serializedUrl);
        }

        /// <summary>
        ///     This method is responsible for retrieving the oauth url from the specified data source.
        /// </summary>
        /// <param name="dataSourceGuid">The data source guid that specifies which data source should get used.</param>
        /// <returns>This method returns the oauth url from the specified data source.</returns>
        public async Task<string> GetOauthUrl(string dataSourceGuid)
        {
            IDataSourceAdaptee adaptee = await dataProviderLoader.GetDataSourceByGuid(dataSourceGuid);
            dataProviderAdapter = new DataProviderAdapter(adaptee);
            return dataProviderAdapter.GetOauthUrl();
        }

        /// <summary>
        ///     This method is responsible for returning the Oauth url from the specified  data source.
        /// </summary>
        /// <param name="code">The code which is used to retrieve Oauth tokens from the external data source.</param>
        /// <param name="dataSourceGuid">The data source guid that specifies which data source should get used.</param>
        /// <returns>This method returns the Oauth tokens from the external data source.</returns>
        public async Task<OauthTokens> GetTokens(string code, string dataSourceGuid)
        {
            IDataSourceAdaptee adaptee = await dataProviderLoader.GetDataSourceByGuid(dataSourceGuid);
            dataProviderAdapter = new DataProviderAdapter(adaptee);
            return await dataProviderAdapter.GetTokens(code);
        }

        /// <summary>
        ///     This method is responsible for returning the data sources.
        /// </summary>
        /// <param name="needsAuth">
        ///     The needsAuth parameter specifies whether the returned data sources should follow a specific
        ///     auth flow.
        /// </param>
        /// <returns>This method returns a collection of data source adaptees.</returns>
        public async Task<IEnumerable<IDataSourceAdaptee>> RetrieveDataSources(bool? needsAuth)
        {
            List<IDataSourceAdaptee> sources = await dataProviderLoader.GetAllDataSources();

            if(needsAuth == null) return sources;

            sources = FilterAuthPages(sources, needsAuth.Value);

            if(needsAuth.Value) return sources.Where(s => s is IAuthorizedDataSourceAdaptee);

            return sources.Where(s => s is IPublicDataSourceAdaptee);
        }

        /// <summary>
        ///     This method is responsible for retrieving a data source by guid.
        /// </summary>
        /// <param name="guid">The data source guid that specifies which data source should get retrieved.</param>
        /// <returns>This method returns the data source with the specified guid.</returns>
        public async Task<IDataSourceAdaptee> RetrieveDataSourceByGuid(string guid)
        {
            IDataSourceAdaptee source = await dataProviderLoader.GetDataSourceByGuid(guid);
            return source;
        }

        /// <summary>
        ///     This method is responsible for retrieving a data source by name.
        /// </summary>
        /// <param name="name">The name of the data source that specified which data source should get retrieved.</param>
        /// <returns>This method returns the data source with the specified name.</returns>
        public async Task<IDataSourceAdaptee> RetrieveDataSourceByName(string name)
        {
            IDataSourceAdaptee source = await dataProviderLoader.GetDataSourceByName(name);
            return source;
        }

        private List<IDataSourceAdaptee> FilterAuthPages(List<IDataSourceAdaptee> adaptees, bool authFlow)
        {
            foreach(IDataSourceAdaptee source in adaptees)
            {
                source.DataSourceWizardPages = source.DataSourceWizardPages.Where(page => page.AuthFlow == authFlow)
                                                     .ToList();
            }

            return adaptees;
        }

    }

}
