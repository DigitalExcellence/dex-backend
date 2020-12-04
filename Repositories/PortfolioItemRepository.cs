using Repositories.Base;
using Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace Repositories
{
    public interface IPortfolioItemRepository : IRepository<PortfolioItem>
    {

    }

    public class PortfolioItemRepository : Repository<PortfolioItem>, IPortfolioItemRepository
    {
        public PortfolioItemRepository(DbContext dbContext) : base(dbContext) { }
    }
}
