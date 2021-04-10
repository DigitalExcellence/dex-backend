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
using Services.ExternalDataProviders.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Services.ExternalDataProviders
{

    /// <summary>
    ///     The interface of the data provider loader.
    /// </summary>
    public interface IDataProviderLoader
    {

        /// <summary>
        ///     This method is responsible for retrieving all the data sources.
        /// </summary>
        /// <returns>This method returns a collection of data sources.</returns>
        Task<List<IDataSourceAdaptee>> GetAllDataSources();

        /// <summary>
        ///     This method is responsible for retrieving a data source by the specified guid.
        /// </summary>
        /// <param name="guid">This guid will get used for searching the correct data source.</param>
        /// <returns>This method returns a data source with the specified guid.</returns>
        Task<IDataSourceAdaptee> GetDataSourceByGuid(string guid);

        /// <summary>
        ///     This method validates whether a data source with the specified guid exists.
        /// </summary>
        /// <param name="dataSourceGuid">The data source guid that will get checked.</param>
        /// <returns>This method return whether the data source exists or does not exists.</returns>

        bool IsExistingDataSource(string guid);

        /// <summary>
        ///     This method is responsible for retrieving a data source by the specified name.
        /// </summary>
        /// <param name="name">This name will get used for searching the correct data source.</param>
        /// <returns>This method returns a data source with the specified guid.</returns>
        Task<IDataSourceAdaptee> GetDataSourceByName(string name);

    }

    /// <summary>
    ///     The implementation of the data provider loader.
    /// </summary>
    public class DataProviderLoader : IDataProviderLoader
    {

        private readonly IDataSourceModelRepository dataSourceModelRepository;
        private readonly IMapper mapper;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IAssemblyHelper assemblyHelper;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DataProviderLoader" /> class.
        /// </summary>
        /// <param name="serviceScopeFactory">
        ///     The service scope factory which is used for creating objects from a class
        ///     dynamically.
        /// </param>
        /// <param name="dataSourceModelRepository">
        ///     The data source model repository which is used for communicating with the
        ///     repository layer.
        /// </param>
        /// <param name="assemblyHelper">The assembly helper retrieves to correct location of the executing assembly.</param>
        /// <param name="mapper">The mapper which is used to convert the adaptees to data source models.</param>
        public DataProviderLoader(
            IServiceScopeFactory serviceScopeFactory,
            IDataSourceModelRepository dataSourceModelRepository,
            IAssemblyHelper assemblyHelper,
            IMapper mapper)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.dataSourceModelRepository = dataSourceModelRepository;
            this.assemblyHelper = assemblyHelper;
            this.mapper = mapper;
        }

        /// <summary>
        ///     This method is responsible for retrieving all the data sources.
        /// </summary>
        /// <returns>This method returns a collection of data sources.</returns>
        public async Task<List<IDataSourceAdaptee>> GetAllDataSources()
        {
            List<IDataSourceAdaptee> dataSources = GetLocalAdapteeImplementations();
            await UpdateDatabaseWithLocalAdapteeImplementations(dataSources);
            List<IDataSourceAdaptee> updatedDataSourceAdaptees = await UpdateModelsWithRepositoryValues(dataSources);
            return updatedDataSourceAdaptees;
        }

        /// <summary>
        ///     This method is responsible for retrieving a data source by the specified guid.
        /// </summary>
        /// <param name="guid">This guid will get used for searching the correct data source.</param>
        /// <returns>This method returns a data source with the specified guid.</returns>
        public async Task<IDataSourceAdaptee> GetDataSourceByGuid(string guid)
        {
            return (await GetAllDataSources())
                .SingleOrDefault(d => d.Guid == guid);
        }

        /// <summary>
        ///     This method validates whether a data source with the specified guid exists.
        /// </summary>
        /// <param name="dataSourceGuid">The data source guid that will get checked.</param>
        /// <returns>This method return whether the data source exists or does not exists.</returns>

        public bool IsExistingDataSource(string guid)
        {
            List<IDataSourceAdaptee> dataSources = GetLocalAdapteeImplementations();
            return dataSources.FirstOrDefault(d => d.Guid == guid) != null;
        }

        /// <summary>
        ///     This method is responsible for retrieving a data source by the specified name.
        /// </summary>
        /// <param name="name">This name will get used for searching the correct data source.</param>
        /// <returns>This method returns a data source with the specified guid.</returns>
        public async Task<IDataSourceAdaptee> GetDataSourceByName(string name)
        {
            return (await GetAllDataSources()).SingleOrDefault(d => d.Title == name);
        }

        private async Task<List<IDataSourceAdaptee>> UpdateModelsWithRepositoryValues(List<IDataSourceAdaptee> sources)
        {
            DataSource[] sourceModels = (await dataSourceModelRepository.GetAll()).ToArray();
            foreach(DataSource sourceModel in sourceModels)
            {
                IDataSourceAdaptee source = sources.SingleOrDefault(s => s.Guid == sourceModel.Guid);
                if(source == null) continue;
                source.Title = sourceModel.Title;
                source.Description = sourceModel.Description;
                source.IsVisible = sourceModel.IsVisible;
                source.Icon = sourceModel.Icon;
                source.DataSourceWizardPages = sourceModel.DataSourceWizardPages.OrderBy(page => page.AuthFlow)
                                                          .ThenBy(page => page.OrderIndex)
                                                          .ToList();
            }

            return sources;
        }

        private List<IDataSourceAdaptee> GetLocalAdapteeImplementations()
        {
            List<IDataSourceAdaptee> dataSources = new List<IDataSourceAdaptee>();
            using IServiceScope scope = serviceScopeFactory.CreateScope();

            Type[] types =
                assemblyHelper.RetrieveTypesFromExecutingAssemblyFolderFolderByInterface(typeof(IDataSourceAdaptee));
            foreach(Type type in types)
            {
                object dataSourceAdaptee = scope.ServiceProvider.GetService(type);
                if(dataSourceAdaptee != null) dataSources.Add(dataSourceAdaptee as IDataSourceAdaptee);
            }

            return dataSources;
        }

        private async Task UpdateDatabaseWithLocalAdapteeImplementations(IEnumerable<IDataSourceAdaptee> sources)
        {
            IEnumerable<DataSource> sourceModels = await dataSourceModelRepository.GetAll();

            // For every adaptee implementation, check if a model in the database is found. Whenever
            // no model in the database is found, this should get added to the database.
            IEnumerable<IDataSourceAdaptee> adapteesWithoutModel =
                sources.Where(s => sourceModels.SingleOrDefault(m => m.Guid == s.Guid) == null);
            if(adapteesWithoutModel.Any())
            {
                await dataSourceModelRepository.AddRangeAsync(
                    mapper.Map<IEnumerable<IDataSourceAdaptee>, IEnumerable<DataSource>>(adapteesWithoutModel));
            }

            // For every model in the database, check if an adaptee is found. Whenever
            // no adaptee is found, this should get removed from the database.
            List<DataSource> modelsWithoutAdaptee =
                sourceModels.Where(m => sources.SingleOrDefault(s => s.Guid == m.Guid) == null)
                            .ToList();
            modelsWithoutAdaptee.ForEach(async m => await dataSourceModelRepository.RemoveAsync(m.Id));

            dataSourceModelRepository.Save();
        }

    }

}
