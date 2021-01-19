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
    public interface IPortfolioRepository : IRepository<Portfolio>
    {

    }

    public class PortfolioRepository : Repository<Portfolio>, IPortfolioRepository
    {
        public PortfolioRepository(DbContext dbContext) : base(dbContext) { }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public override void Update(Portfolio entity)
        {
            entity = UpdateUpdatedField(entity);

            DbSet.Attach(entity);
            if(entity.User != null)
            {
                DbContext.Entry(entity.User)
                         .Property(x => x.Email)
                         .IsModified = false;

                DbContext.Entry(entity.User)
                         .State = EntityState.Unchanged;
            }

            if(entity.Icon == null)
            {
                DbContext.Entry(entity)
                         .Entity.Icon = null;
            }

            DbSet.Update(entity);
        }

        /// <summary>
        /// Find the portfolio async by portfolio id
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        public override async Task<Portfolio> FindAsync(int id)
        {
            Portfolio portfolio = await GetDbSet<Portfolio>()
           .Where(s => s.Id == id)
           .Include(s => s.PortfolioItem)
           .SingleOrDefaultAsync();

            return portfolio;
        }
    }
}
