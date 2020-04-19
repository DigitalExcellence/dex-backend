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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repositories
{

    public interface IProjectRepository : IRepository<Project>
    {

        Task<List<Project>> GetAllWithUsersAsync();

        Task<IEnumerable<Project>> SearchAsync(
            string query,
            int? skip = null,
            int? take = null,
            Expression<Func<Project, object>> orderBy = null,
            bool orderByAsc = true
        );

        Task<int> SearchCountAsync(string query);

        Task<Project> FindWithUserAndCollaboratorsAsync(int id);

    }

    public class ProjectRepository : Repository<Project>, IProjectRepository
    {

        public ProjectRepository(DbContext dbContext) : base(dbContext) { }

        public override Task<Project> FindAsync(int id)
        {
            return GetDbSet<Project>()
                   .Where(s => s.Id == id)
                   .Include(p => p.Collaborators)
                   .SingleOrDefaultAsync();
        }

        public Task<List<Project>> GetAllWithUsersAsync()
        {
            return GetDbSet<Project>()
                   .Include(p => p.User)
                   .ToListAsync();
        }

        /// <summary>
        ///     Search the database for projects matching the search query and parameters
        /// </summary>
        /// <param name="query">The search query</param>
        /// <param name="skip">The number of projects to skip</param>
        /// <param name="take">The number of projects to return</param>
        /// <param name="orderBy">The property to order the projects by</param>
        /// <param name="orderByAsc">The order direction (True: asc, False: desc)</param>
        /// <returns>The projects matching the search query and parameters</returns>
        public virtual async Task<IEnumerable<Project>> SearchAsync(
            string query,
            int? skip = null,
            int? take = null,
            Expression<Func<Project, object>> orderBy = null,
            bool orderByAsc = true
        )
        {
            //SQL LIKE isn't case sensitive, toLowerCase() is not necessary in the query
            IQueryable<Project> queryable = SearchQuery(query);
            if(orderBy != null)
            {
                if(orderByAsc)
                {
                    queryable = queryable.OrderBy(orderBy);
                } else
                {
                    queryable = queryable.OrderByDescending(orderBy);
                }
            }
            if(skip.HasValue) queryable = queryable.Skip(skip.Value);
            if(take.HasValue) queryable = queryable.Take(take.Value);
            return await queryable.ToListAsync();
        }

        public virtual async Task<int> SearchCountAsync(string query)
        {
            return await SearchQuery(query)
                         .CountAsync();
        }

        public Task<Project> FindWithUserAndCollaboratorsAsync(int id)
        {
            return GetDbSet<Project>()
                   .Include(project => project.User)
                   .Include(project => project.Collaborators)
                   .Where(project => project.Id == id)
                   .FirstOrDefaultAsync();
        }

     private IQueryable<Project> SearchQuery(string query)
        {
            return DbSet
                .Include(project => project.User)
                .Where(project =>
                    project.Name.Contains(query) ||
                    project.Description.Contains(query) ||
                    project.ShortDescription.Contains(query) ||
                    project.Uri.Contains(query) ||
                    project.Id.ToString().Equals(query) ||
                    project.User.Name.Contains(query)
                );
        }

    }
}
