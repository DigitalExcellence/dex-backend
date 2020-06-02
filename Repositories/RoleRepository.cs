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
    public interface IRoleRepository : IRepository<Role>
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<List<Role>> GetAllAsync();
        /// <summary>
        /// Finds the name of the by.
        /// </summary>
        /// <returns></returns>
        Task<Role> FindByName(string roleName);

    }
    /// <summary>
    /// Role Repository
    /// </summary>
    /// <seealso cref="Repositories.Base.Repository{Models.Role}" />
    /// <seealso cref="Repositories.IRoleRepository" />
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public RoleRepository(DbContext dbContext) : base(dbContext) { }
        /// <summary>
        /// Finds the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public override Task<Role> FindAsync(int id)
        {
            return GetDbSet<Role>()
                   .Where(s => s.Id == id)
                   .Include(p => p.Scopes)
                   .SingleOrDefaultAsync();
        }
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns></returns>
        public Task<List<Role>> GetAllAsync()
        {
            return GetDbSet<Role>()
                   .Include(p => p.Scopes)
                   .ToListAsync();
        }
        /// <summary>
        /// Finds the name of the by.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        public Task<Role> FindByName(string roleName)
        {
            return GetDbSet<Role>()
                   .Include(p => p.Scopes)
                   .Where(r => r.Name == roleName)
                   .SingleOrDefaultAsync();
        }


        public override async Task RemoveAsync(int id)
        {
            Role roleToDelete = await GetDbSet<Role>()
                .Where(r => r.Id == id)
                .Include(r => r.Scopes)
                .SingleOrDefaultAsync().ConfigureAwait(false);

            GetDbSet<RoleScope>().RemoveRange(roleToDelete.Scopes);
            GetDbSet<Role>().Remove(roleToDelete);
        }
    }
}
