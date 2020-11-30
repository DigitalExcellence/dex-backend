using Repositories.Base;
using Models;
using Models.Defaults;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public interface IPortfolioRepository : IRepository<Portfolio>
    {

    }

    class PortfolioRepository : Repository<Portfolio>, IPortfolioRepository
    {
        public PortfolioRepository(DbContext dbContext) : base(dbContext) { }
    }

}
