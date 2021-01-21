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
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Services.DataProviders
{

    public interface IDataProviderService
    {

        Task<IEnumerable<Project>> GetAllProjects(string dataSourceGuid, string token, bool needsAuth);

        Task<Project> GetProjectByGuid(string dataSourceGuid, string accessToken, int id, bool needsAuth);

        bool IsExistingDataSourceGuid(string dataSourceGuid);

        Task<Project> GetProjectFromUri(string dataSourceGuid, string sourceUri);

        Task<string> GetOauthUrl(string guid);

        Task<OauthTokens> GetTokens(string code, string guid);

        Task<IEnumerable<IDataSourceAdaptee>> RetrieveDataSources(bool? needsAuth);

        Task<IDataSourceAdaptee> RetrieveDataSourceByGuid(string guid);

    }

    /// <summary>
    /// The data provider service which communicates with the correct data source. This service
    /// acts as the adapter in the adapter pattern.
    /// </summary>
    /// <seealso cref="IDataProviderService" />
    public class DataProviderService : IDataProviderService
    {
        private readonly IDataProviderLoader dataProviderLoader;
        private IDataProviderAdapter dataProviderAdapter;

        public DataProviderService(IDataProviderLoader dataProviderLoader)
        {
            this.dataProviderLoader = dataProviderLoader;
        }

        public async Task<IEnumerable<Project>> GetAllProjects(string dataSourceGuid, string token, bool needsAuth)
        {
            IDataSourceAdaptee adaptee = await dataProviderLoader.GetDataSourceByGuid(dataSourceGuid);
            dataProviderAdapter = new DataProviderAdapter(adaptee);
            return await dataProviderAdapter.GetAllProjects(token, needsAuth);
        }

        public async Task<Project> GetProjectByGuid(string dataSourceGuid, string accessToken, int id, bool needsAuth)
        {
            IDataSourceAdaptee adaptee = await dataProviderLoader.GetDataSourceByGuid(dataSourceGuid);
            dataProviderAdapter = new DataProviderAdapter(adaptee);
            return await dataProviderAdapter.GetProjectByGuid(accessToken, id.ToString(), needsAuth);
        }

        public bool IsExistingDataSourceGuid(string dataSourceGuid)
        {
            return dataProviderLoader.GetDataSourceByGuid(dataSourceGuid) != null;
        }

        public async Task<Project> GetProjectFromUri(string dataSourceGuid, string sourceUri)
        {
            IDataSourceAdaptee adaptee = await dataProviderLoader.GetDataSourceByGuid(dataSourceGuid);
            dataProviderAdapter = new DataProviderAdapter(adaptee);


            
            sourceUri = sourceUri.Replace("%2F", "/");
            if(sourceUri[^1] == '/')
                sourceUri = sourceUri.Remove(sourceUri.Length - 1);
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

        public async Task<string> GetOauthUrl(string guid)
        {
            IDataSourceAdaptee adaptee = await dataProviderLoader.GetDataSourceByGuid(guid);
            dataProviderAdapter = new DataProviderAdapter(adaptee);
            return dataProviderAdapter.GetOauthUrl();
        }

        public async Task<OauthTokens> GetTokens(string code, string guid)
        {
            IDataSourceAdaptee adaptee = await dataProviderLoader.GetDataSourceByGuid(guid);
            dataProviderAdapter = new DataProviderAdapter(adaptee);
            return await dataProviderAdapter.GetTokens(code);

        }

        public async Task<IEnumerable<IDataSourceAdaptee>> RetrieveDataSources(bool? needsAuth)
        {
            List<IDataSourceAdaptee> sources =  await dataProviderLoader.GetAllDataSources();

            if(needsAuth == null) return sources;

            sources = FilterAuthPages(sources, needsAuth.Value);

            if(needsAuth.Value)
                return sources.Where(s => s is IAuthorizedDataSourceAdaptee);

            return sources.Where(s => s is IPublicDataSourceAdaptee);
        }

        public async Task<IDataSourceAdaptee> RetrieveDataSourceByGuid(string guid)
        {
            IDataSourceAdaptee source = await dataProviderLoader.GetDataSourceByGuid(guid);
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
