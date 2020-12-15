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
using System.Linq;
using System.Threading.Tasks;

namespace Services.DataProviders
{

    public interface IDataProviderAdapter
    {

        Task<IEnumerable<Project>> GetAllProjects(string dataSourceGuid, string accessToken);

        Task<Project> GetProjectByGuid(string dataSourceGuid, string accessToken, int id);

        bool IsExistingDataSourceGuid(string dataSourceGuid);

        string GetOauthUrl(string guid);

        Task<OauthTokens> GetTokens(string code, string guid);

    }

    /// <summary>
    /// The data provider service which communicates with the correct data source. This service
    /// acts as the adapter in the adapter pattern.
    /// </summary>
    /// <seealso cref="IDataProviderAdapter" />
    public class DataProviderAdapter : IDataProviderAdapter
    {
        private readonly IDataProviderLoader dataProviderLoader;

        public DataProviderAdapter(IDataProviderLoader dataProviderLoader)
        {
            this.dataProviderLoader = dataProviderLoader;
        }

        public async Task<IEnumerable<Project>> GetAllProjects(string dataSourceGuid, string accessToken)
        {
            IDataSourceAdaptee dataSourceAdaptee = dataProviderLoader.GetDataSourceByGuid(dataSourceGuid);
            IEnumerable<Project> projects = await dataSourceAdaptee.GetAllProjects(accessToken);
            return projects;
        }

        public async Task<Project> GetProjectByGuid(string dataSourceGuid, string accessToken, int id)
        {
            IEnumerable<Project> projects = await GetAllProjects(dataSourceGuid, accessToken);
            return projects.SingleOrDefault(p => p.Id == id);
        }

        public bool IsExistingDataSourceGuid(string dataSourceGuid)
        {
            return dataProviderLoader.GetDataSourceByGuid(dataSourceGuid) != null;
        }

        public string GetOauthUrl(string guid)
        {
            return dataProviderLoader.GetDataSourceByGuid(guid)
                                     .OauthUrl;
        }

        public async Task<OauthTokens> GetTokens(string code, string guid)
        {
            IDataSourceAdaptee dataProvider = dataProviderLoader.GetDataSourceByGuid(guid);
            OauthTokens tokens = await dataProvider.GetTokens(code);
            return tokens;
        }

    }

}
