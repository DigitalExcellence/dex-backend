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
using Models;
using Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.DataProviders
{

    public interface IDataProviderService
    {

        Task<IEnumerable<Project>> GetAllProjects(string dataSourceGuid, string token, bool needsAuth);

        Task<Project> GetProjectByGuid(string dataSourceGuid, string accessToken, int id, bool needsAuth);

        bool IsExistingDataSourceGuid(string dataSourceGuid);

        string GetOauthUrl(string guid);

        Task<OauthTokens> GetTokens(string code, string guid);

        Task<IEnumerable<IDataSourceAdaptee>> RetrieveDataSources(bool? needsAuth);

    }

    /// <summary>
    /// The data provider service which communicates with the correct data source. This service
    /// acts as the adapter in the adapter pattern.
    /// </summary>
    /// <seealso cref="IDataProviderService" />
    public class DataProviderService : IDataProviderService
    {
        private readonly IDataProviderLoader dataProviderLoader;
        private readonly IDataSourceAdapteeRepository dataSourceAdapteeRepository;
        private readonly IMapper mapper;
        private IDataProviderAdapter dataProviderAdapter;

        public DataProviderService(
            IDataProviderLoader dataProviderLoader,
            IDataSourceAdapteeRepository dataSourceAdapteeRepository,
            IMapper mapper)
        {
            this.dataProviderLoader = dataProviderLoader;
            this.dataSourceAdapteeRepository = dataSourceAdapteeRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<Project>> GetAllProjects(string dataSourceGuid, string token, bool needsAuth)
        {
            IDataSourceAdaptee adaptee = dataProviderLoader.GetDataSourceByGuid(dataSourceGuid);
            dataProviderAdapter = new DataProviderAdapter(adaptee);
            return await dataProviderAdapter.GetAllProjects(token, needsAuth);
        }

        public async Task<Project> GetProjectByGuid(string dataSourceGuid, string accessToken, int id, bool needsAuth)
        {
            IDataSourceAdaptee adaptee = dataProviderLoader.GetDataSourceByGuid(dataSourceGuid);
            dataProviderAdapter = new DataProviderAdapter(adaptee);
            return await dataProviderAdapter.GetProjectByGuid(accessToken, id.ToString(), needsAuth);
        }

        public bool IsExistingDataSourceGuid(string dataSourceGuid)
        {
            return dataProviderLoader.GetDataSourceByGuid(dataSourceGuid) != null;
        }

        public string GetOauthUrl(string guid)
        {
            IDataSourceAdaptee adaptee = dataProviderLoader.GetDataSourceByGuid(guid);
            dataProviderAdapter = new DataProviderAdapter(adaptee);
            return dataProviderAdapter.GetOauthUrl();
        }

        public async Task<OauthTokens> GetTokens(string code, string guid)
        {
            IDataSourceAdaptee adaptee = dataProviderLoader.GetDataSourceByGuid(guid);
            dataProviderAdapter = new DataProviderAdapter(adaptee);
            return await dataProviderAdapter.GetTokens(code);

        }

        public async Task<IEnumerable<IDataSourceAdaptee>> RetrieveDataSources(bool? needsAuth)
        {
            IEnumerable<DataSource> sourceModels = await dataSourceAdapteeRepository.GetAll();
            IEnumerable<IDataSourceAdaptee> sources =  dataProviderLoader.GetAllDataSources().ToArray();

            AddNewDataSources(sourceModels, sources);

            mapper.Map(sourceModels, sources);
            sources = sources.Where(d => d.IsVisible);

            if(needsAuth == null) return sources;

            if(needsAuth.Value)
                return sources.Where(s => s is IAuthorizedDataSourceAdaptee);

            return sources.Where(s => s is IPublicDataSourceAdaptee);
        }

        private void AddNewDataSources(IEnumerable<DataSource> sourceModels, IEnumerable<IDataSourceAdaptee> sources)
        {
            // For every adaptee implementation, check if a model in the database is found. Whenever
            // no model in the database is found, this should get added to the database.
            IEnumerable<IDataSourceAdaptee> adapteesWithoutModel =
                sources.Where(s => sourceModels.SingleOrDefault(m => m.Guid == s.Guid) == null);
            dataSourceAdapteeRepository.AddRange(mapper.Map<IEnumerable<IDataSourceAdaptee>, IEnumerable<DataSource>>(adapteesWithoutModel));
        }

    }

}
