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
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Services
{
    public interface IUserService : IService<User>
    {
        Task<User> GetUserAsync(int userId);
        Task<User> GetUserByIdentityIdAsync(string identityId);

        Task<bool> RemoveUserAsync(int userId);

        User GetUserByUsername(string upn);

        bool UserHasScope(string identityId, string scope);

        bool UserWithRoleExists(Role role);
        List<User> GetAllExpectedGraduatingUsers();
    }

    public class UserService : Service<User>, IUserService
    {
        public UserService(IUserRepository repository) : base(repository) { }

        protected new IUserRepository Repository => (IUserRepository) base.Repository;

        public async Task<User> GetUserAsync(int userId)
        {
            return await Repository.GetUserAsync(userId).ConfigureAwait(false);
        }

        public async Task<User> GetUserByIdentityIdAsync(string identityId)
        {
            return await Repository.GetUserByIdentityIdAsync(identityId).ConfigureAwait(false);
        }

        public async Task<bool> RemoveUserAsync(int userId)
        {
            return await Repository.RemoveUserAsync(userId).ConfigureAwait(false);
        }

        public User GetUserByUsername(string upn)
        {
            throw new NotImplementedException();
        }

        public bool UserHasScope(string identityId, string scope)
        {
            return Repository.UserHasScope(identityId, scope);
        }

        public bool UserWithRoleExists(Role role)
        {
            return Repository.UserWithRoleExists(role);
        }

        public List <User> GetAllExpectedGraduatingUsers()
        {
            List<User> users = Repository.GetAllExpectedGraduatingUsers().Result;
            return users;
        } 
    }
}
