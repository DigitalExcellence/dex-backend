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
    ///     This is the project tag service interface
    /// </summary>
    public interface IProjectTagService : IService<ProjectTag>
    {
        Task<ProjectTag> GetProjectTag(int projectId, int tagId);
        Task<ProjectTag> GetProjectTag(int tagId);
    }

    /// <summary>
    ///     This is the project tag service
    /// </summary>
    public class ProjectTagService : Service<ProjectTag>,
                                          IProjectTagService
    {

        /// <summary>
        ///     This is the project like constructor
        /// </summary>
        /// <param name="repository"></param>
        public ProjectTagService(IProjectTagRepository repository) :
            base(repository) { }

        /// <summary>
        ///     Gets the repository
        /// </summary>
        private new IProjectTagRepository Repository =>
            (IProjectTagRepository) base.Repository;

        public Task<ProjectTag> GetProjectTag(int projectId, int tagId)
        {
            return Repository.GetProjectTag(projectId, tagId);
        }

        public Task<ProjectTag> GetProjectTag(int tagId)
        {
            return Repository.GetProjectTag(tagId);
        }

    }

}
