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
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories
{

    /// <summary>
    ///     This is the interface of the role repository
    /// </summary>
    public interface IRoleRepository : IRepository<Role>
    {
        /// <summary>
        ///     This interface method gets all roles asynchronously
        /// </summary>
        /// <returns></returns>
        Task<List<Role>> GetAllAsync();
    }

    /// <summary>
    ///     This is the role repository
    /// </summary>
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        /// <summary>
        ///     This is the role repository constructor
        /// </summary>
        /// <param name="dbContext"></param>
        public RoleRepository(DbContext dbContext) : base(dbContext) { }

        /// <summary>
        /// Updates the specified role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <exception cref="ArgumentNullException">Role argument is null</exception>
        /// <exception cref="DbUpdateConcurrencyException">Cannot update non existing object..</exception>
        public override void Update(Role role)
        {
            if(role == null)
            {
                throw new ArgumentNullException("Role argument is null");
            }
            Role existingRole = GetDbSet<Role>()
                                 .Where(p => p.Id == role.Id)
                                 .Include(p => p.Scopes)
                                 .SingleOrDefault();

            if(existingRole == null)
            {
                throw new DbUpdateConcurrencyException("Cannot update non existing object..");
            };

            DbContext.Entry(existingRole).CurrentValues.SetValues(role);

            foreach(RoleScope existingScope in existingRole.Scopes.ToList())
            {
                // delete children
                if(role.Scopes.All(c => c.Id != existingScope.Id))
                {
                    GetDbSet<RoleScope>()
                        .Remove(existingScope);
                }
                // add or update
                foreach(RoleScope newRoleScope in role.Scopes)
                {
                    RoleScope existingRoleScope = existingRole.Scopes
                                                       .SingleOrDefault(c => c.Id == newRoleScope.Id);
                    //update
                    if(existingRoleScope != null)
                    {
                        DbContext.Entry(existingRoleScope).CurrentValues.SetValues(newRoleScope);
                    }
                    //add
                    else
                    {
                        RoleScope newScope = new RoleScope(newRoleScope.Scope)
                        {
                            RoleId = role.Id
                        };
                        existingRole.Scopes.Add(newScope);
                    }
                }
                DbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Find role by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The retrieved role.</returns>
        public override Task<Role> FindAsync(int id)
        {
            return GetDbSet<Role>()
                   .Where(s => s.Id == id)
                   .Include(p => p.Scopes)
                   .SingleOrDefaultAsync();
        }

        /// <summary>
        /// Gets all the roles with scopes.
        /// </summary>
        /// <returns>A list of all roles and scopes.</returns>
        public Task<List<Role>> GetAllAsync()
        {
            return GetDbSet<Role>()
                   .Include(p => p.Scopes)
                   .ToListAsync();
        }

        /// <summary>
        /// Remove the role with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <exception cref="KeyNotFoundException">Id: {id} not found</exception>
        public override async Task RemoveAsync(int id)
        {
            Role roleToDelete = await GetDbSet<Role>()
                .Where(r => r.Id == id)
                .Include(r => r.Scopes)
                .SingleOrDefaultAsync().ConfigureAwait(false);

            if(roleToDelete == null)
            {
                throw new KeyNotFoundException($"Id: {id} not found");
            }

            if(roleToDelete.Scopes != null)
            {
                GetDbSet<RoleScope>().RemoveRange(roleToDelete.Scopes);
            }

            GetDbSet<Role>().Remove(roleToDelete);
        }
    }
}
