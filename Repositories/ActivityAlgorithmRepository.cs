using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IActivityAlgorithmRepository
    {
        Task<ActivityAlgorithmMultiplier> GetActivityAlgorithmMultiplierAsync();
        void UpdateActivityAlgorithmMultiplierAsync(ActivityAlgorithmMultiplier activityAlgorithmMultiplier);
    }
    public class ActivityAlgorithmRepository : IActivityAlgorithmRepository
    {
        private readonly DbContext dbContext;
        /// <summary>
        ///     Initializes a new instance of the <see cref="ActivityAlgorithmRepository" /> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public ActivityAlgorithmRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ActivityAlgorithmMultiplier> GetActivityAlgorithmMultiplierAsync()
        {
            return await dbContext.Set<ActivityAlgorithmMultiplier>().AsNoTracking().FirstOrDefaultAsync();
        }

        public void UpdateActivityAlgorithmMultiplierAsync(ActivityAlgorithmMultiplier activityAlgorithmMultiplier)
        {
            dbContext.Set<ActivityAlgorithmMultiplier>().Update(activityAlgorithmMultiplier);
            dbContext.SaveChanges();
        }
    }
}
