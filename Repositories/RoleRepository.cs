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
        Task<List<Role>> GetAllAsync();
    }

    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(DbContext dbContext) : base(dbContext) { }

        public override void Update(Role role)
        {
            Role existingRole = GetDbSet<Role>()
                                 .Where(p => p.Id == role.Id)
                                 .Include(p => p.Scopes)
                                 .SingleOrDefault();
            if(existingRole == null) return;

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
        public override Task<Role> FindAsync(int id)
        {
            return GetDbSet<Role>()
                   .Where(s => s.Id == id)
                   .Include(p => p.Scopes)
                   .SingleOrDefaultAsync();
        }

        public Task<List<Role>> GetAllAsync()
        {
            return GetDbSet<Role>()
                   .Include(p => p.Scopes)
                   .ToListAsync();
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
