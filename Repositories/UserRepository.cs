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
using System.Linq;
using System.Threading.Tasks;

namespace Repositories
{

    public interface IUserRepository : IRepository<User>
    {

        Task<User> GetUserAsync(int userId);
        Task<User> GetUserByIdentityIdAsync(string userId);

        Task<bool> RemoveUserAsync(int userId);
        bool UserHasScope(string identityId, string scope);

    }

    public class UserRepository : Repository<User>, IUserRepository
    {

        public UserRepository(DbContext dbContext) : base(dbContext) { }

        public async override Task<User> FindAsync(int userId)
        {
            return await GetDbSet<User>()
                             .Where(s => s.Id == userId)
                             .Include(s => s.Role)
                             .ThenInclude(s => s.Scopes)
                             .SingleOrDefaultAsync();
        }
        public async Task<User> GetUserAsync(int userId)
        {
            return await GetDbSet<User>()
                         .Where(s => s.Id == userId)
                         .SingleOrDefaultAsync();
        }
        public async Task<User> GetUserByIdentityIdAsync(string identityId)
        {
            return await GetDbSet<User>()
                         .Where(s => s.IdentityId == identityId)
                         .SingleOrDefaultAsync();
        }

        public async Task<bool> RemoveUserAsync(int userId)
        {
            User user = await GetDbSet<User>()
                              .Where(s => s.Id == userId)
                              .SingleOrDefaultAsync();

            if(user != null)
            {
                GetDbSet<User>()
                    .Remove(user);
                return true;
            }

            return false;
        }

        public bool UserHasScope(string identityId, string scope)
        {
            User user = GetDbSet<User>()
                             .Where(s => s.IdentityId == identityId)
                             .Include(s => s.Role)
                             .ThenInclude(s => s.Scopes)
                             .SingleOrDefault();
            
            if(user == null || user.Role == null)
            {
                return false;
            }
            foreach(RoleScope scp in user.Role.Scopes)
            {
                if(scp.Scope == scope)
                {
                    return true;
                }
            }
            return false;
        }
    }

}
