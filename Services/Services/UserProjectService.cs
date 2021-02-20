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
    ///     This is the interface of the user (following) project service
    /// </summary>
    public interface IUserProjectService : IService<UserProject>
    {
        /// <summary>
        ///     This is the interface method which checks if a user already follows a project
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="projectId"></param>
        /// <returns>boolean</returns>
        bool CheckIfUserFollows(int userId, int projectId);
    }

    /// <summary>
    ///     This is the user (following) project service
    /// </summary>
    public class UserProjectService : Service<UserProject>, IUserProjectService
    {
        /// <summary>
        ///     This is the constructor of the user project service
        /// </summary>
        /// <param name="repository"></param>
        public UserProjectService(IUserProjectRepository repository) : base(repository) { }

        /// <summary>
        ///     Gets the repository
        /// </summary>
        protected new IUserProjectRepository Repository => (IUserProjectRepository) base.Repository;

        /// <summary>
        ///     This is the method which checks if a user already follows a project
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="projectId"></param>
        /// <returns>boolean</returns>
        bool IUserProjectService.CheckIfUserFollows(int userId, int projectId)
        {
            if(Repository.CheckIfUserFollows(userId, projectId))
            {
                return true;
            }
            return false;
        }
    }
}
