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
using System.Linq;
using System.Threading.Tasks;

namespace Repositories
{
    /// <summary>
    /// IUserRepository
    /// </summary>
    /// <seealso cref="Repositories.Base.IRepository{Models.User}" />
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Gets the user asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The task that will get the User object.</returns>
        Task<User> GetUserAsync(int userId);
        /// <summary>
        /// Gets the user by identity identifier asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>The task that will return the user object with that specified external identifier.</returns>
        Task<User> GetUserByIdentityIdAsync(string userId);

        /// <summary>
        /// Removes the user asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Returns if the user is removed</returns>
        Task<bool> RemoveUserAsync(int userId);
        /// <summary>
        /// Users the has scope.
        /// </summary>
        /// <param name="identityId">The identity identifier.</param>
        /// <param name="scope">The scope.</param>
        /// <returns>true if the user that has this scope.</returns>
        bool UserHasScope(string identityId, string scope);
        /// <summary>
        /// Users the with role exists.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <returns>Returns true if a user has the given role, else false.</returns>
        bool UserWithRoleExists(Role role);

    }
    /// <summary>
    /// UserRepository
    /// </summary>
    /// <seealso cref="Repositories.Base.Repository{Models.User}" />
    /// <seealso cref="Repositories.IUserRepository" />
    public class UserRepository : Repository<User>, IUserRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public UserRepository(DbContext dbContext) : base(dbContext) { }

        /// <summary>
        /// Finds the asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Returns the user object.</returns>
        public override async Task<User> FindAsync(int userId)
        {
            return await GetDbSet<User>()
                             .Where(s => s.Id == userId)
                             .Include(s => s.Role)
                             .ThenInclude(s => s.Scopes)
                             .SingleOrDefaultAsync();
        }
        /// <summary>
        /// Gets the user asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Returns the found user object.</returns>
        public async Task<User> GetUserAsync(int userId)
        {
            return await GetDbSet<User>()
                         .Where(s => s.Id == userId)
                         .Include(u => u.Role)
                         .ThenInclude(u => u.Scopes)
                         .SingleOrDefaultAsync();
        }
        /// <summary>
        /// Gets the user by identity identifier asynchronous.
        /// </summary>
        /// <param name="identityId">The identity identifier.</param>
        /// <returns>Returns the found user object.</returns>
        public async Task<User> GetUserByIdentityIdAsync(string identityId)
        {
            return await GetDbSet<User>()
                         .Where(s => s.IdentityId == identityId)
                         .Include(u => u.Role)
                         .ThenInclude(u => u.Scopes)
                         .SingleOrDefaultAsync();
        }

        /// <summary>
        /// Removes the user asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>if the user is removed.</returns>
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

        /// <summary>
        /// Users the has scope.
        /// </summary>
        /// <param name="identityId">The identity identifier.</param>
        /// <param name="scope">The scope.</param>
        /// <returns>true if a user has the given scope.</returns>
        public bool UserHasScope(string identityId, string scope)
        {
            User user = GetDbSet<User>()
                             .Where(s => s.IdentityId == identityId)
                             .Include(s => s.Role)
                             .ThenInclude(s => s.Scopes)
                             .SingleOrDefault();
            if(user?.Role == null)
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

        /// <summary>
        /// Users the with role exists.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <returns>true if a user exists with the given role.</returns>
        public bool UserWithRoleExists(Role role)
        {
            return GetDbSet<User>()
                .Include(s => s.Role)
                .SingleOrDefault(r => r.Role.Id == role.Id) != null;
        }


    }

}
