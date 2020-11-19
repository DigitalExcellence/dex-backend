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
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories
{

    /// <summary>
    /// The institution repository interface
    /// </summary>
    /// <seealso cref="IRepository{Institution}" />
    public interface IInstitutionRepository : IRepository<Institution>
    {

        /// <summary>
        /// This method returns all the institutions.
        /// </summary>
        /// <returns>This method returns a collection of institutions.</returns>
        Task<IEnumerable<Institution>> GetInstitutionsAsync();

        Task<Institution> GetInstitutionByInstitutionIdentityId(string institutionIdentityId);

    }

    /// <summary>
    /// The implementation for the institution repository
    /// </summary>
    /// <seealso cref="IInstitutionRepository" />
    /// <seealso cref="Repository{Institution}" />
    public class InstitutionRepository : Repository<Institution>, IInstitutionRepository
    {

        public InstitutionRepository(DbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<Institution>> GetInstitutionsAsync()
        {
            return await GetDbSet<Institution>()
                       .ToListAsync();
        }

        public async Task<Institution> GetInstitutionByInstitutionIdentityId(string institutionIdentityId)
        {
            return await GetDbSet<Institution>()
                       .FirstOrDefaultAsync(i => i.IdentityId == institutionIdentityId);
        }

    }

}
