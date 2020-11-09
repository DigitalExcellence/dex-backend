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

using Ganss.XSS;
using Models;
using Repositories;
using Services.Base;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Services.Services
{
    public interface IUserUserService : IService<UserUser>
    {
        bool CheckIfUserFollows(int userId,int follwedUserId);
    }

    public class UserUserService : Service<UserUser>, IUserUserService
    {
        public UserUserService(IUserUserRepository repository) : base(repository) { }

        protected new IUserUserRepository Repository => (IUserUserRepository) base.Repository;

        public override void Add(UserUser entity)
        {
            Repository.Add(entity);
        }

        public override void Remove(UserUser userUser)
        {
            Repository.Remove(userUser);
        }

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
