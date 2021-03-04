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
using System.Threading.Tasks;

namespace Repositories
{

    /// <summary>
    ///     This is the interface of the project tag repository
    /// </summary>
    public interface IProjectCategoryRepository : IRepository<ProjectCategory>
    {
        Task<ProjectCategory> GetProjectCategory(int projectId, int categoryId);
        Task<ProjectCategory> GetProjectCategory(int categoryId);
    }

    /// <summary>
    ///     This is the project tag repository
    /// </summary>
    public class ProjectCategoryRepository : Repository<ProjectCategory>,
                                             IProjectCategoryRepository
    {

        /// <summary>
        ///     This is the project tag repository constructor
        /// </summary>
        /// <param name="dbContext"></param>
        public ProjectCategoryRepository(DbContext dbContext) :
            base(dbContext) { }

        public Task<ProjectCategory> GetProjectCategory(int projectId, int categoryId)
        {
            return DbSet.Where(p => p.Category.Id == categoryId && p.Project.Id == projectId)
                        .FirstOrDefaultAsync();
        }

        public Task<ProjectCategory> GetProjectCategory(int tagId)
        {
            return DbSet.Where(p => p.Category.Id == tagId)
                        .FirstOrDefaultAsync();
        }
    }
}
