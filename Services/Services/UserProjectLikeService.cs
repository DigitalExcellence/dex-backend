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
    ///     This is the project like service interface
    /// </summary>
    public interface IUserProjectLikeService : IService<ProjectLike>
    {
        /// <summary>
        ///     This is the interface method which checks if the user already like a project
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="projectId"></param>
        /// <returns>Boolean</returns>
        bool CheckIfUserAlreadyLiked(int userId, int projectId);
    }

    /// <summary>
    ///     This is the project like service
    /// </summary>
    public class UserProjectLikeService : Service<ProjectLike>,
                                          IUserProjectLikeService
    {

        /// <summary>
        ///     This is the project like constructor
        /// </summary>
        /// <param name="repository"></param>
        public UserProjectLikeService(IUserProjectLikeRepository repository) :
            base(repository)
        { }

        /// <summary>
        ///     Gets the repository
        /// </summary>
        private new IUserProjectLikeRepository Repository =>
            (IUserProjectLikeRepository) base.Repository;

        /// <summary>
        ///     This is the interface method which checks if the user already like a project
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        bool IUserProjectLikeService.CheckIfUserAlreadyLiked(int userId, int projectId)
        {
            if(Repository.CheckIfUserAlreadyLiked(userId, projectId))
            {
                return true;
            }

            return false;
        }

    }

}
