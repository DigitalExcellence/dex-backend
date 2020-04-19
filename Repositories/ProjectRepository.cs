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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Base;

namespace Repositories
{
	public interface IProjectRepository : IRepository<Project>
	{
		Task<List<Project>> GetAllWithUsersAsync();

		Task<IEnumerable<Project>> SearchAsync(String query);
		
		Task<IEnumerable<Project>> SearchSkipTakeAsync(String query, int skip, int take);
		
		Task<int> SearchCountAsync(String query);

		Task<Project> FindWithUserAndCollaboratorsAsync(int id);
	}

	public class ProjectRepository : Repository<Project>, IProjectRepository
	{
		public ProjectRepository(DbContext dbContext) : base(dbContext)
		{
		}

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

		private IQueryable<Project> SearchQuery(string query)
		{
			return DbSet
				.Include(p => p.User)
				.Where(p =>
				p.Name.Contains(query) ||
				p.Description.Contains(query) ||
				p.ShortDescription.Contains(query) ||
				p.Uri.Contains(query) ||
				p.Id.ToString().Equals(query) ||
				p.User.Name.Contains(query)
				);
		}

		public virtual async Task<IEnumerable<Project>> SearchAsync(string query)
		{
			return await SearchQuery(query)
				.ToListAsync();
		}
		
		// Search for projects
		// @param query The search query
		// @param skip The amount of results to skip
		// @param take The amount of results to return
		public virtual async Task<IEnumerable<Project>> SearchSkipTakeAsync(string query, int skip, int take)
		{
			return await SearchQuery(query)
				.Skip(skip)
				.Take(take)
				.ToListAsync();
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
	}
}
