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

using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories
{

    /// <summary>
    /// The data source adaptee repository interface
    /// </summary>
    /// <seealso cref="IRepository{DataSource}" />
    public interface IDataSourceModelRepository : IRepository<DataSource>
    {

        /// <summary>
        /// This method returns the data source with the specified guid.
        /// </summary>
        /// <returns>This method returns a data source model with the specified guid.</returns>
        Task<DataSource> GetDataSourceByGuid(string guid);

        /// <summary>
        /// This method finds the data source by the specified name.
        /// </summary>
        /// <param name="name">The name which will be used to find the correct data source.</param>
        /// <returns>This method returns the data source with the specified name.</returns>
        Task<DataSource> GetDataSourceByName(string name);

    }

    /// <summary>
    /// The implementation for the data source adaptee repository
    /// </summary>
    /// <seealso cref="IDataSourceModelRepository" />
    /// <seealso cref="Repository{DataSource}" />
    public class DataSourceModelRepository : Repository<DataSource>, IDataSourceModelRepository
    {

        public DataSourceModelRepository(DbContext dbContext) : base(dbContext) { }

        /// <summary>
        /// This method returns the data source with the specified guid.
        /// </summary>
        /// <returns>This method returns a data source model with the specified guid.</returns>
        public async Task<DataSource> GetDataSourceByGuid(string guid)
        {
            return await GetDbSet<DataSource>()
                         .Where(d => d.Guid == guid)
                         .Include(d => d.Icon)
                         .Include(w => w.DataSourceWizardPages)
                         .ThenInclude(dw => dw.WizardPage)
                         .SingleOrDefaultAsync();
        }

        public async Task<DataSource> GetDataSourceByName(string name)
        {
            return await GetDbSet<DataSource>()
                       .Where(d => d.Title == name)
                       .Include(d => d.Icon)
                       .Include(w => w.DataSourceWizardPages)
                       .ThenInclude(dw => dw.WizardPage)
                       .SingleOrDefaultAsync();
        }

        /// <summary>
        /// This method returns all data sources.
        /// </summary>
        /// <returns>This methods returns a collection of all the data sources.</returns>
        public override async Task<IEnumerable<DataSource>> GetAll()
        {
            return await GetDbSet<DataSource>()
                         .Include(d => d.Icon)
                         .Include(w => w.DataSourceWizardPages)
                         .ThenInclude(dw => dw.WizardPage)
                         .ToListAsync();
        }

    }

}
