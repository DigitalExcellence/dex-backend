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

    public interface IWizardPageRepository : IRepository<WizardPage>
    {

        /// <summary>
        /// This method returns a range of wizard pages from the specified collection of wizard ids.
        /// </summary>
        /// <param name="wizardPageIds">The collection of wizard page ids that will get retrieved.</param>
        /// <returns>This method will return a collection of wizard pages from the specified collection of ids.</returns>
        Task<IEnumerable<WizardPage>> GetRange(IEnumerable<int> wizardPageIds);

    }

    public class WizardPageRepository : Repository<WizardPage>, IWizardPageRepository
    {

        public WizardPageRepository(DbContext dbContext) : base(dbContext) { }

        /// <summary>
        /// This method returns the wizard page with the specified id.
        /// </summary>
        /// <returns>This method returns a wizard page with the specified id.</returns>
        public override async Task<WizardPage> FindAsync(int id)
        {
            return await GetDbSet<WizardPage>()
                         .Include(w => w.DataSourceWizardPages)
                         .ThenInclude(dw => dw.DataSource)
                         .SingleOrDefaultAsync(w => w.Id == id);
        }

        /// <summary>
        /// This method returns a range of wizard pages from the specified collection of wizard ids.
        /// </summary>
        /// <param name="wizardPageIds">The collection of wizard page ids that will get retrieved.</param>
        /// <returns>This method will return a collection of wizard pages from the specified collection of ids.</returns>
        public async Task<IEnumerable<WizardPage>> GetRange(IEnumerable<int> wizardPageIds)
        {
            return await GetDbSet<WizardPage>()
                         .Include(w => w.DataSourceWizardPages)
                         .ThenInclude(dw => dw.DataSource)
                         .Where(w => wizardPageIds.Contains(w.Id))
                         .ToListAsync();
        }

    }

}
