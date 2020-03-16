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
                .Include(p => p.Contributors)
                .SingleOrDefaultAsync();
        }
    }
}
