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

using MessageBrokerPublisher;
using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Base;
using System.Linq;

namespace Repositories
{

    /// <summary>
    ///     This is the interface of the project like repository
    /// </summary>
    public interface IUserProjectLikeRepository : IRepository<ProjectLike>
    {

        /// <summary>
        ///     This interface method checks if the user already liked a certain project
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        bool CheckIfUserAlreadyLiked(int userId, int projectId);

    }

    /// <summary>
    ///     This is the project like repository
    /// </summary>
    public class UserProjectLikeRepository : Repository<ProjectLike>,
                                             IUserProjectLikeRepository
    {
        private ITaskPublisher taskPublisher;

        /// <summary>
        ///     This is the project like repository constructor
        /// </summary>
        /// <param name="dbContext"></param>
       public UserProjectLikeRepository(DbContext dbContext, ITaskPublisher taskPublisher) :
            base(dbContext) {
            this.taskPublisher = taskPublisher;
        }

        /// <summary>
        ///     This is the override add method.
        /// </summary>
        /// <param name="projectLike"></param>
        public override void Add(ProjectLike projectLike)
        {
            DbContext.Add(projectLike);
        }


        /// <summary>
        ///     This method removes the project like
        /// </summary>
        /// <param name="projectLike"></param>
        public override void Remove(ProjectLike projectLike)
        {
            ProjectLike projectToRemove = GetDbSet
                    <ProjectLike>()
                .SingleOrDefault(project => project.UserId ==
                                            projectLike.ProjectLiker.Id &&
                                            project.LikedProject.Id ==
                                            projectLike.LikedProject.Id);

            GetDbSet<ProjectLike>()
                .Remove(projectToRemove);
        }

        /// <summary>
        ///     This method checks if the user already liked a certain project
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        bool IUserProjectLikeRepository
            .CheckIfUserAlreadyLiked(int userId, int projectId)
        {
            ProjectLike projectLike = GetDbSet<ProjectLike>()
                .SingleOrDefault(project =>
                                     project.ProjectLiker.Id == userId && project.LikedProject.Id == projectId);

            if(projectLike != null)
            {
                return true;
            }
            return false;
        }

    }

}
