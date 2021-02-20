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

namespace Services.Services
{
    /// <summary>
    ///     This is the user (follow) user service
    /// </summary>
    public interface IUserUserService : IService<UserUser>
    {
        /// <summary>
        ///     This is the interface method which checks if the user already follows a certain user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="follwedUserId"></param>
        /// <returns>boolean</returns>
        bool CheckIfUserFollows(int userId, int follwedUserId);
    }

    /// <summary>
    ///     This is the user (follows) user service
    /// </summary>
    public class UserUserService : Service<UserUser>, IUserUserService
    {
        /// <summary>
        ///     This is the user (follows) user service constructor
        /// </summary>
        /// <param name="repository"></param>
        public UserUserService(IUserUserRepository repository) : base(repository) { }

        /// <summary>
        ///     Gets the repository
        /// </summary>
        protected new IUserUserRepository Repository => (IUserUserRepository) base.Repository;


        /// <summary>
        ///     This is the interface method which checks if the user already follows a certain user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="follwedUserId"></param>
        /// <returns>boolean</returns>
        bool IUserUserService.CheckIfUserFollows(int userId, int follwedUserId)
        {
            if(Repository.CheckIfUserFollows(userId, follwedUserId))
            {
                return true;
            }
            return false;
        }
    }
}
