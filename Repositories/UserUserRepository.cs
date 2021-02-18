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

namespace Repositories
{
    /// <summary>
    ///     This is the interface for the user (follows) user repository
    /// </summary>
    public interface IUserUserRepository : IRepository<UserUser>
    {
        /// <summary>
        ///     This interface method checks if the user is already followed
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="followedUserId"></param>
        /// <returns>boolean</returns>
        bool CheckIfUserFollows(int userId, int followedUserId);
    }

    /// <summary>
    ///     This the user (follows) user repository
    /// </summary>
    public class UserUserRepository : Repository<UserUser>, IUserUserRepository
    {
        /// <summary>
        ///     This the user (follows) user repository constructor
        /// </summary>
        /// <param name="dbContext"></param>
        public UserUserRepository(DbContext dbContext) : base(dbContext) { }

        /// <summary>
        ///     This is the overridden remove method which unfollows the user
        /// </summary>
        /// <param name="userUser"></param>
        public override void Remove(UserUser userUser)
        {
            UserUser userUnfollow = GetDbSet
                    <UserUser>()
                .SingleOrDefault(s => s.User.Id == userUser.User.Id
                                   && s.FollowedUser.Id == userUser.FollowedUser.Id);

            GetDbSet<UserUser>()
            .Remove(userUnfollow);
        }

        /// <summary>
        ///     This method checks if the user is already followed
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="followedUserId"></param>
        /// <returns>boolean</returns>
        bool IUserUserRepository.CheckIfUserFollows(int userId, int followedUserId)
        {
            UserUser userUser = GetDbSet
                    <UserUser>()
                .SingleOrDefault(s => s.User.Id == userId
                                   && s.FollowedUser.Id == followedUserId);

            if(userUser != null)
            {
                return true;
            }
            return false;
        }
    }
}
