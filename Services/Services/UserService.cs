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

    /// <summary>
    ///     This is the interface of the user service
    /// </summary>
    public interface IUserService : IService<User>
    {

        /// <summary>
        ///     This is the interface method which gets a user by identifier asynchronous
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>User entity</returns>
        Task<User> GetUserAsync(int userId);

        /// <summary>
        ///     This is the interface method which gets the user by identity identifier asynchronous
        /// </summary>
        /// <param name="identityId"></param>
        /// <returns>User entity</returns>
        Task<User> GetUserByIdentityIdAsync(string identityId);

        /// <summary>
        ///     This is the interface method which removes a user by identifier asynchronous
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>boolean</returns>
        Task<bool> RemoveUserAsync(int userId);

        /// <summary>
        ///     This is the interface method which gets the user by username
        /// </summary>
        /// <param name="upn"></param>
        /// <returns>User entity</returns>
        User GetUserByUsername(string upn);

        /// <summary>
        ///     This is the interface method which gets the user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>User entity</returns>
        Task<User> GetUserByEmail(string email);

        /// <summary>
        ///     This is the interface method which checks if the user has a certain scope
        /// </summary>
        /// <param name="identityId"></param>
        /// <param name="scope"></param>
        /// <returns>boolean</returns>
        bool UserHasScope(string identityId, string scope);

        /// <summary>
        ///     This is the interface method which checks if the user has the same institution as another user
        /// </summary>
        /// <param name="ownUserId"></param>
        /// <param name="requestUserId"></param>
        /// <returns>boolean</returns>
        Task<bool> HasSameInstitution(int ownUserId, int requestUserId);

        /// <summary>
        ///     This is the interface method which checks if a user with a certain role exists
        /// </summary>
        /// <param name="role"></param>
        /// <returns>boolean</returns>
        bool UserWithRoleExists(Role role);

        List<User> GetAllExpectedGraduatingUsers(int amountOfMonths);

    }

    /// <summary>
    ///     This is the user service
    /// </summary>
    public class UserService : Service<User>, IUserService
    {

        /// <summary>
        ///     This is the constructor of the user service
        /// </summary>
        /// <param name="repository"></param>
        public UserService(IUserRepository repository) : base(repository) { }

        /// <summary>
        ///     Gets the repository
        /// </summary>
        protected new IUserRepository Repository => (IUserRepository) base.Repository;

        /// <summary>
        ///     This is the method which gets a user by identifier asynchronous
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<User> GetUserAsync(int userId)
        {
            return await Repository.GetUserAsync(userId)
                                   .ConfigureAwait(false);
        }

        /// <summary>
        ///     This is the method which gets the user by identity identifier asynchronous
        /// </summary>
        /// <param name="identityId"></param>
        /// <returns></returns>
        public async Task<User> GetUserByIdentityIdAsync(string identityId)
        {
            return await Repository.GetUserByIdentityIdAsync(identityId)
                                   .ConfigureAwait(false);
        }

        /// <summary>
        ///     This is the method which removes a user by identifier asynchronous
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>boolean</returns>
        public async Task<bool> RemoveUserAsync(int userId)
        {
            return await Repository.RemoveUserAsync(userId)
                                   .ConfigureAwait(false);
        }

        /// <summary>
        ///     This is the method which gets the user by username
        /// </summary>
        /// <param name="upn"></param>
        /// <returns>User entity</returns>
        public User GetUserByUsername(string upn)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     This is the method which gets the user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>User entity</returns>
        public async Task<User> GetUserByEmail(string email)
        {
            return await Repository.FindUserByEmailAsync(email);
        }

        /// <summary>
        ///     This is the method which checks if the user has a certain scope
        /// </summary>
        /// <param name="identityId"></param>
        /// <param name="scope"></param>
        /// <returns>boolean</returns>
        public bool UserHasScope(string identityId, string scope)
        {
            return Repository.UserHasScope(identityId, scope);
        }

        /// <summary>
        ///     This is the method which checks if the user has the same institution as another user
        /// </summary>
        /// <param name="ownUserId"></param>
        /// <param name="requestUserId"></param>
        /// <returns>boolean</returns>
        public async Task<bool> HasSameInstitution(int ownUserId, int requestUserId)
        {
            User ownUserInfo = await Repository.FindAsync(ownUserId);
            User userRequestInfo = await Repository.FindAsync(requestUserId);
            return ownUserInfo.Institution == userRequestInfo.Institution && ownUserInfo.Institution != null;
        }

        /// <summary>
        ///     This is the method which checks if a user with a certain role exists
        /// </summary>
        /// <param name="role"></param>
        /// <returns>boolean</returns>
        public bool UserWithRoleExists(Role role)
        {
            return Repository.UserWithRoleExists(role);
        }

        public List<User> GetAllExpectedGraduatingUsers(int amountOfMonths)
        {
            List<User> users = Repository.GetAllExpectedGraduatingUsers(amountOfMonths)
                                         .Result;
            return users;
        }

    }

}
