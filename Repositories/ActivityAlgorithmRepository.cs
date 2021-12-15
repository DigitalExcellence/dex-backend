using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IActivityAlgorithmRepository : IRepository<ProjectActivityConfig>
    {
        Task<ProjectActivityConfig> GetActivityAlgorithmConfig();
        void UpdateActivityAlgorithmConfig(ProjectActivityConfig projectActivityConfig);
    }
    public class ActivityAlgorithmRepository : Repository<ProjectActivityConfig>, IActivityAlgorithmRepository
    {
        private readonly DbContext dbContext;
        /// <summary>
        ///     Initializes a new instance of the <see cref="ActivityAlgorithmRepository" /> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public ActivityAlgorithmRepository(DbContext dbContext) : base(dbContext) { }

        /// <summary>
        /// Gets the config for the activity algorithm
        /// </summary>
        /// <returns>The project activity config</returns>
        public async Task<ProjectActivityConfig> GetActivityAlgorithmConfig()
        {
            DbSet<ProjectActivityConfig> dbSet = GetDbSet<ProjectActivityConfig>();
            ProjectActivityConfig projectActivityConfig = await  dbSet.AsNoTracking().FirstOrDefaultAsync();
            if(projectActivityConfig == null)
            {
                ProjectActivityConfig newProjectActivityConfig = new ProjectActivityConfig()
                {
                    AverageLikeDateMultiplier = 1,
                    ConnectedCollaboratorsMultiplier = 1,
                    RecentCreatedDataMultiplier = 1,
                    InstitutionMultiplier = 1,
                    LikeDataMultiplier = 1,
                    MetaDataMultiplier = 1,
                    RepoScoreMultiplier = 1,
                    UpdatedTimeMultiplier = 1
                };
                dbSet.Add(newProjectActivityConfig);
                base.Save();
                return newProjectActivityConfig;
            }
            return projectActivityConfig;
        }

        public void UpdateActivityAlgorithmConfig(ProjectActivityConfig projectActivityConfig)
        {
            GetDbSet<ProjectActivityConfig>().Update(projectActivityConfig);
            base.Save();
        }
    }
}
