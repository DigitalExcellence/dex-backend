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
using System.Threading.Tasks;

namespace Services.Services
{

    /// <summary>
    ///     This is the project category service interface
    /// </summary>
    public interface IProjectCategoryService : IService<ProjectCategory>
    {
        Task<ProjectCategory> GetProjectCategory(int projectId, int categoryId);
        Task<ProjectCategory> GetProjectCategory(int categoryId);
    }

    /// <summary>
    ///     This is the project category service
    /// </summary>
    public class ProjectCategoryService : Service<ProjectCategory>,
                                          IProjectCategoryService
    {

        /// <summary>
        ///     This is the project category constructor
        /// </summary>
        /// <param name="repository"></param>
        public ProjectCategoryService(IProjectCategoryRepository repository) :
            base(repository) { }

        /// <summary>
        ///     Gets the repository
        /// </summary>
        private new IProjectCategoryRepository Repository =>
            (IProjectCategoryRepository) base.Repository;

        public Task<ProjectCategory> GetProjectCategory(int projectId, int categoryId)
        {
            return Repository.GetProjectCategory(projectId, categoryId);
        }

        public Task<ProjectCategory> GetProjectCategory(int categoryId)
        {
            return Repository.GetProjectCategory(categoryId);
        }

    }

}
