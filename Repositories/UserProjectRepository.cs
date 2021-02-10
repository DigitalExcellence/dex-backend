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
    ///     This is the user (follows) project repository
    /// </summary>
    public interface IUserProjectRepository : IRepository<UserProject>
    {
        /// <summary>
        ///     This interface method checks if the user already follows the project
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="projectId"></param>
        /// <returns>boolean</returns>
        bool CheckIfUserFollows(int userId, int projectId);
    }

    /// <summary>
    ///     This is the user (follows) project repository
    /// </summary>
    public class UserProjectRepository : Repository<UserProject>, IUserProjectRepository
    {
        /// <summary>
        ///     This is the user (follows) project constructor
        /// </summary>
        /// <param name="dbContext"></param>
        public UserProjectRepository(DbContext dbContext) : base(dbContext) { }

        /// <summary>
        ///     This method removes the project follow from the database
        /// </summary>
        /// <param name="userProject"></param>
        public override void Remove(UserProject userProject)
        {
            UserProject projectToRemove = GetDbSet
                    <UserProject>()
                .SingleOrDefault(s => s.UserId == userProject.User.Id
                                   && s.Project.Id == userProject.Project.Id);

            GetDbSet<UserProject>()
            .Remove(projectToRemove);
        }

        /// <summary>
        ///     This method checks if the user already follows the project
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="projectId"></param>
        /// <returns>boolean</returns>
        bool IUserProjectRepository.CheckIfUserFollows(int userId, int projectId)
        {
            UserProject userProject = GetDbSet
                    <UserProject>()
                .SingleOrDefault(s => s.UserId == userId
                                   && s.Project.Id == projectId);

            if(userProject != null)
            {
                return true;
            }
            return false;
        }
    }
}
