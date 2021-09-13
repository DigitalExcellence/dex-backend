using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories
{
    public interface IProjectTransferRepository : IRepository<ProjectTransferRequest>
    {
        //TODO: METHODS HERE
    }

   public class ProjectTransferRepository : Repository<ProjectTransferRequest>, IProjectTransferRepository
    {
        public ProjectTransferRepository(DbContext dbContext) : base(dbContext) { }

    }
}
