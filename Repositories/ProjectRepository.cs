using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Base;

namespace Repositories
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<IEnumerable<Project>> SearchAsync(String query);
        
        Task<IEnumerable<Project>> SearchSkipTakeAsync(String query, int skip, int take);
        
        Task<int> SearchCountAsync(String query);
        
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

        public virtual async Task<IEnumerable<Project>> SearchAsync(string query)
        {
            return await DbSet
                .Include(p => p.User)
                .Where(p =>
                p.Name.Contains(query) ||
                p.Description.Contains(query) ||
                p.ShortDescription.Contains(query) ||
                p.Uri.Contains(query) ||
                p.Id.ToString().Equals(query) ||
                p.User.Name.Contains(query)
                ).ToListAsync();
        }
        
        // Search for projects
        // @param query The search query
        // @param skip The amount of results to skip
        // @param take The amount of results to return
        public virtual async Task<IEnumerable<Project>> SearchSkipTakeAsync(string query, int skip, int take)
        {
            return await DbSet
                .Include(p => p.User)
                .Where(p =>
                    p.Name.Contains(query) ||
                    p.Description.Contains(query) ||
                    p.ShortDescription.Contains(query) ||
                    p.Uri.Contains(query) ||
                    p.Id.ToString().Equals(query) ||
                    p.User.Name.Contains(query)
                )
                .Skip(skip)
                .Take(take)
                .ToListAsync();
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
                    p.Id.ToString().Equals(query) ||
                    p.User.Name.Contains(query)
                ).CountAsync();
        }
        
    }
}
