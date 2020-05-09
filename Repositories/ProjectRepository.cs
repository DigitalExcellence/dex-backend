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
            bool orderByAsc = true,
            bool? highlighted = null
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
        /// <param name="highlighted">Filter highlighted projects</param>
        /// <returns>The projects matching the search query and parameters</returns>
        public virtual async Task<IEnumerable<Project>> SearchAsync(
            string query,
            int? skip = null,
            int? take = null,
            Expression<Func<Project, object>> orderBy = null,
            bool orderByAsc = true,
            bool? highlighted = null
        )
        {
            IQueryable<Project> queryable = DbSet
                                            .Include(p => p.User)
                                            .Where(p =>
                                                       p.Name.Contains(query) ||
                                                       p.Description.Contains(query) ||
                                                       p.ShortDescription.Contains(query) ||
                                                       p.Uri.Contains(query) ||
                                                       p.Id.ToString()
                                                        .Equals(query) ||
                                                       p.User.Name.Contains(query));
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
            if(highlighted.HasValue)
            {
                IEnumerable<int> highlightedQueryable = DbContext.Set<Highlight>()
                                                       .Where(h => h.StartDate <= DateTime.Now ||
                                                                   h.StartDate == null)
                                                       .Where(h => h.EndDate >= DateTime.Now ||
                                                                   h.EndDate == null)
                                                       .Select(h => h.ProjectId)
                                                       .ToList();
                if(highlighted.Value)
                {
                    queryable = queryable.Where(p => highlightedQueryable.Contains(p.Id));
                } else
                {
                    queryable = queryable.Where(p => !highlightedQueryable.Contains(p.Id));
                }
            }
            return await queryable.ToListAsync();
        }

        public virtual async Task<int> SearchCountAsync(string query)
        {
            return await DbSet
                         .Include(p => p.User)
                         .Where(p =>
                                    p.Name.Contains(query) ||
                                    p.Description.Contains(query) ||
                                    p.ShortDescription.Contains(query) ||
                                    p.Uri.Contains(query) ||
                                    p.Id.ToString()
                                     .Equals(query) ||
                                    p.User.Name.Contains(query))
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

    }

}
