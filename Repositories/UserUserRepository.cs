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

    public interface IUserUserRepository : IRepository<UserUser>
    {
        bool CheckIfUserFollows(int userId,int followedUserId);
    }

    public class UserUserRepository : Repository<UserUser>, IUserUserRepository
    {
        public UserUserRepository(DbContext dbContext) : base(dbContext) { }

        public override void Add(UserUser userUser)
        {
            DbContext.Add(userUser);
        }

        public override void Remove(UserUser userUser)
        {
            UserUser userUnfollow = GetDbSet<UserUser>()
                .Where
                (s => s.User.Id == userUser.User.Id
                && s.FollowedUser.Id == userUser.FollowedUser.Id)
                .SingleOrDefault();

                GetDbSet<UserUser>()
                .Remove(userUnfollow);
        }

        bool IUserUserRepository.CheckIfUserFollows(int userId, int followedUserId)
        {
            UserUser userUser = GetDbSet<UserUser>()
                              .Where(s => (s.User.Id == userId)
                              && s.FollowedUser.Id == followedUserId)
                              .SingleOrDefault();

            if(userUser != null)
            {
                return true;
            }
            return false;
        }
    }
}
