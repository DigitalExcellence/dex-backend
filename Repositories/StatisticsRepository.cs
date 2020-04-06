using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Base;

namespace Repositories
{
    public interface IStatisticsRepository : IRepository<Statistic>
    {
    }

    public class StatisticsRepository : Repository<Statistic>, IStatisticsRepository
    {
        public StatisticsRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}