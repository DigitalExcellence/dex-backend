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
       public Task<ProjectTransferRequest> FindTransferByProjectId(int projectId);
    }

   public class ProjectTransferRepository : Repository<ProjectTransferRequest>, IProjectTransferRepository
    {
        public ProjectTransferRepository(DbContext dbContext) : base(dbContext) { }

        protected new IHighlightRepository Repository => (IHighlightRepository) base.Repository;

        public async Task<ProjectTransferRequest> FindTransferByProjectId(int projectId)
        {
            return await GetDbSet<ProjectTransferRequest>()
                         .Where(transfer => transfer.Project.Id == projectId)
                         .SingleOrDefaultAsync();
        }
    }
}
