using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IProjectTransferRepository : IRepository<ProjectTransferRequest>
    {
       public Task<List<ProjectTransferRequest>> FindTransferByProjectId(int projectId);
       public Task<ProjectTransferRequest> FindTransferByGuid(Guid guid);
    }

   public class ProjectTransferRepository : Repository<ProjectTransferRequest>, IProjectTransferRepository
    {
        public ProjectTransferRepository(DbContext dbContext) : base(dbContext) { }

        protected new IProjectTransferRepository Repository => (IProjectTransferRepository) base.DbContext;

        public async Task<ProjectTransferRequest> FindTransferByGuid(Guid guid)
        {
            return await GetDbSet<ProjectTransferRequest>()
                .Where(transfer => transfer.TransferGuid == guid)
                .Include(p => p.Project)
                .Include(u => u.PotentialNewOwner)
                .Include(c => c.Project.User)
                .FirstOrDefaultAsync();
        }

        public Task<List<ProjectTransferRequest>> FindTransferByProjectId(int projectId)
        {
            return GetDbSet<ProjectTransferRequest>()
                         .Where(transfer => transfer.Project.Id == projectId)
                         .ToListAsync();
        }
    }
}
