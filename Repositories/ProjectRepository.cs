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
        Task<Project> GetProjectAsync(int projectId);
        void RemoveProjectAsync(int projectId);
        IEnumerable<Project> SearchProject(string value);
    }

    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Project> GetProjectAsync(int projectId)
        {
            return await GetDbSet<Project>()
                .Where(s => s.Id == projectId)
                .SingleOrDefaultAsync();
        }

        public async void RemoveProjectAsync(int projectId)
        {
            Project project = await GetDbSet<Project>()
                 .Where(s => s.Id == projectId)
                 .SingleOrDefaultAsync();

            if (project != null)
            {
                GetDbSet<Project>().Remove(project);
            }
        }

        public IEnumerable<Project> SearchProject(string value)
        {
            IEnumerable<Project> found = GetDbSet<Project>()
                .Where(p => p.Title.Contains($"/{value}/", StringComparison.OrdinalIgnoreCase));
            return found;
        }
    }
}