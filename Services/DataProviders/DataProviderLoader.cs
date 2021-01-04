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
using Microsoft.Extensions.DependencyInjection;
using Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Services.DataProviders
{

    public interface IDataProviderLoader
    {

        Task<IEnumerable<IDataSourceAdaptee>> GetAllDataSources();

        Task<IDataSourceAdaptee> GetDataSourceByGuid(string guid);

    }

    public class DataProviderLoader : IDataProviderLoader
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IDataSourceAdapteeRepository dataSourceAdapteeRepository;
        private readonly IMapper mapper;

        public DataProviderLoader(
            IServiceScopeFactory serviceScopeFactory,
            IDataSourceAdapteeRepository dataSourceAdapteeRepository,
            IMapper mapper)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.dataSourceAdapteeRepository = dataSourceAdapteeRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<IDataSourceAdaptee>> GetAllDataSources()
        {
            List<IDataSourceAdaptee> dataSources = GetLocalAdapteeImplementations();
            await UpdateDatabaseWithLocalAdapteeImplementations(dataSources);
            IEnumerable<IDataSourceAdaptee> updatedDataSourceAdaptees = await UpdateModelsWithRepositoryValues(dataSources.ToArray());
            return updatedDataSourceAdaptees.Where(d => d.IsVisible);
        }

        public async Task<IDataSourceAdaptee> GetDataSourceByGuid(string guid)
        {
            return (await GetAllDataSources())
                .SingleOrDefault(d => d.Guid == guid);
        }

        private async Task<IEnumerable<IDataSourceAdaptee>> UpdateModelsWithRepositoryValues(IDataSourceAdaptee[] sources)
        {
            IEnumerable<DataSource> sourceModels = (await dataSourceAdapteeRepository.GetAll()).ToArray();
            mapper.Map(sourceModels, sources);
            return sources;
        }

        private List<IDataSourceAdaptee> GetLocalAdapteeImplementations()
        {
            List<IDataSourceAdaptee> dataSources = new List<IDataSourceAdaptee>();
            using IServiceScope scope = serviceScopeFactory.CreateScope();
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            string folder = Path.GetDirectoryName(executingAssembly.Location);

            foreach(string dll in Directory.GetFiles(folder, "*.dll"))
            {
                Assembly assembly = Assembly.LoadFrom(dll);
                foreach(Type type in assembly.GetTypes())
                {
                    if(type.GetInterface("IDataSourceAdaptee") != typeof(IDataSourceAdaptee)) continue;
                    object dataSourceAdaptee = scope.ServiceProvider.GetService(type);
                    if(dataSourceAdaptee != null)
                        dataSources.Add(dataSourceAdaptee as IDataSourceAdaptee);
                }
            }

            return dataSources;
        }

        private async Task UpdateDatabaseWithLocalAdapteeImplementations(IEnumerable<IDataSourceAdaptee> sources)
        {
            IEnumerable<DataSource> sourceModels = await dataSourceAdapteeRepository.GetAll();

            // For every adaptee implementation, check if a model in the database is found. Whenever
            // no model in the database is found, this should get added to the database.
            IEnumerable<IDataSourceAdaptee> adapteesWithoutModel =
                sources.Where(s => sourceModels.SingleOrDefault(m => m.Guid == s.Guid) == null);
            dataSourceAdapteeRepository.AddRange(mapper.Map<IEnumerable<IDataSourceAdaptee>, IEnumerable<DataSource>>(adapteesWithoutModel));

            // For every model in the database, check if an adaptee is found. Whenever
            // no adaptee is found, this should get removed from the database.
            List<DataSource> modelsWithoutAdaptee =
                sourceModels.Where(m => sources.SingleOrDefault(s => s.Guid == m.Guid) == null).ToList();
            modelsWithoutAdaptee.ForEach(async m => await dataSourceAdapteeRepository.RemoveAsync(m.Id));
        }

    }

}
