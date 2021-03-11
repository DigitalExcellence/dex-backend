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
    ///     This is the interface of the project category repository
    /// </summary>
    public interface IProjectCategoryRepository : IRepository<ProjectCategory>
    {
        /// <summary>
        ///     Gets project category by given project and category id
        /// </summary>
        Task<ProjectCategory> GetProjectCategory(int projectId, int categoryId);

        /// <summary>
        ///     Gets project category by given category id
        /// </summary>
        Task<ProjectCategory> GetProjectCategory(int categoryId);
    }

    /// <summary>
    ///     This is the project category repository
    /// </summary>
    public class ProjectCategoryRepository : Repository<ProjectCategory>,
                                             IProjectCategoryRepository
    {

        /// <summary>
        ///     This is the project category repository constructor 
        /// </summary>
        /// <param name="dbContext"></param>
        public ProjectCategoryRepository(DbContext dbContext) :
            base(dbContext) { }

        /// <summary>
        ///     Gets project category by given project and category id
        /// </summary>
        public Task<ProjectCategory> GetProjectCategory(int projectId, int categoryId)
        {
            return DbSet.Where(p => p.Category.Id == categoryId && p.Project.Id == projectId)
                        .FirstOrDefaultAsync();
        }

        /// <summary>
        ///     Gets project category by given category id
        /// </summary>
        public Task<ProjectCategory> GetProjectCategory(int categoryId)
        {
            return DbSet.Where(p => p.Category.Id == categoryId)
                        .FirstOrDefaultAsync();
        }
    }
}
