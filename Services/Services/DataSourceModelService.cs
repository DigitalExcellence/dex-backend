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
using Repositories;
using Services.Base;
using System.Threading.Tasks;

namespace Services.Services
{

    /// <summary>
    ///     The Data Source Model interface
    /// </summary>
    /// <seealso cref="IService{TEntity}" />
    public interface IDataSourceModelService : IService<DataSource>
    {

        /// <summary>
        ///     This method finds the data source by the specified guid.
        /// </summary>
        /// <param name="guid">The guid which will be used to find the correct data source.</param>
        /// <returns>This method returns the data source with the specified guid.</returns>
        Task<DataSource> GetDataSourceByGuid(string guid);

        /// <summary>
        ///     This method finds the data source by the specified name.
        /// </summary>
        /// <param name="name">The name which will be used to find the correct data source.</param>
        /// <returns>This method returns the data source with the specified name.</returns>
        Task<DataSource> GetDataSourceByName(string name);

    }

    /// <summary>
    ///     The Data Source Model service
    /// </summary>
    /// <seealso cref="Service{DataSource}" />
    /// <seealso cref="IDataSourceModelService" />
    public class DataSourceModelService : Service<DataSource>, IDataSourceModelService
    {

        /// <summary>
        ///     Initializes a new instance of the <see cref="DataSourceModelService" /> class.
        /// </summary>
        /// <param name="repository">The repository that will be used to communicate with the repository layer.</param>
        public DataSourceModelService(IDataSourceModelRepository repository) : base(repository) { }

        protected new IDataSourceModelRepository Repository => (IDataSourceModelRepository) base.Repository;

        /// <summary>
        ///     This method finds the data source by the specified guid.
        /// </summary>
        /// <param name="guid">The guid which will be used to find the correct data source.</param>
        /// <returns>This method returns the data source with the specified guid.</returns>
        public async Task<DataSource> GetDataSourceByGuid(string guid)
        {
            return await Repository.GetDataSourceByGuid(guid);
        }

        /// <summary>
        ///     This method finds the data source by the specified name.
        /// </summary>
        /// <param name="name">The name which will be used to find the correct data source.</param>
        /// <returns>This method returns the data source with the specified name.</returns>
        public async Task<DataSource> GetDataSourceByName(string name)
        {
            return await Repository.GetDataSourceByName(name);
        }

    }

}
