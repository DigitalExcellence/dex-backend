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

    public interface IUserProjectRepository : IRepository<UserProject>
    {
        bool CheckIfUserFollows(int userId,int projectId);
    }

    public class UserProjectRepository : Repository<UserProject>, IUserProjectRepository
    {
        public UserProjectRepository(DbContext dbContext) : base(dbContext) { }

        public override void Add(UserProject userProject)
        {
            DbContext.Add(userProject);
        }

        public override void Remove(UserProject userProject)
        {
            UserProject projectToRemove = GetDbSet<UserProject>()
                .Where
                (s => s.UserId == userProject.User.Id
                && s.Project.Id == userProject.Project.Id)
                .SingleOrDefault();

                GetDbSet<UserProject>()
                .Remove(projectToRemove);
        }

        bool IUserProjectRepository.CheckIfUserFollows(int userId, int projectId)
        {
            UserProject userProject = GetDbSet<UserProject>()
                              .Where(s => (s.UserId == userId)
                              && s.Project.Id == projectId)
                              .SingleOrDefault();

            if(userProject != null)
            {
                return true;
            }
            return false;
        }
    }
}
