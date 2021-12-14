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
        Task<ProjectActivityConfig> GetActivityAlgorithmConfig();
        void UpdateActivityAlgorithmConfig(ProjectActivityConfig projectActivityConfig);
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

        public async Task<ProjectActivityConfig> GetActivityAlgorithmConfig()
        {
            ProjectActivityConfig projectActivityConfig = await dbContext.Set<ProjectActivityConfig>().AsNoTracking().FirstOrDefaultAsync();
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
                dbContext.Set<ProjectActivityConfig>().Add(newProjectActivityConfig);
                dbContext.SaveChanges();
                return newProjectActivityConfig;
            }
            return projectActivityConfig;
        }

        public void UpdateActivityAlgorithmConfig(ProjectActivityConfig projectActivityConfig)
        {
            dbContext.Set<ProjectActivityConfig>().Update(projectActivityConfig);
            dbContext.SaveChanges();
        }
    }
}
