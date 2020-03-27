using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<List<Project>> GetAllWithUsersAsync();
    }

    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public Task<List<Project>> GetAllWithUsersAsync()
        {
            return GetDbSet<Project>()
                .Include(p => p.User)
                .ToListAsync();
        }
    }
}