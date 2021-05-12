using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{

    public interface IProjectCommentRepository : IRepository<ProjectComment>
    {
        Task<List<ProjectComment>> GetProjectComments(int projectId);
    }

    public class ProjectCommentRepository : Repository<ProjectComment>, IProjectCommentRepository
    {

        public ProjectCommentRepository(DbContext dbContext) : base(dbContext)
        {}

        public Task<List<ProjectComment>> GetProjectComments(int projectId)
        {
            return DbSet.Where(p => p.ProjectId == projectId)
                        .ToListAsync();
        }

        
    }
}
