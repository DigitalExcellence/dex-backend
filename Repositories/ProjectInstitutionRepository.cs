using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repositories
{
    public interface IProjectInstitutionRepository : IRepository<ProjectInstitution>
    {
        /// <summary>
        /// Checks if the the given institution is already linked to the project
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="institutionId"></param>
        /// <returns></returns>
        bool InstitutionIsLinkedToProject(int projectId, int institutionId);

        ProjectInstitution FindByInstitutionIdAndProjectId(int projectId, int institutionId);
        IEnumerable<ProjectInstitution> FindByInstitutionsIdAndProjectId(int projectId, int institutionId);
    }

    public class ProjectInstitutionRepository : Repository<ProjectInstitution>, IProjectInstitutionRepository
    {
        public ProjectInstitutionRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public ProjectInstitution FindByInstitutionIdAndProjectId(int projectId, int institutionId)
        {
            return GetDbSet<ProjectInstitution>().FirstOrDefault(pi => pi.ProjectId == projectId && pi.InstitutionId == institutionId);
        }

        public IEnumerable<ProjectInstitution> FindByInstitutionsIdAndProjectId(int projectId, int institutionId)
        {
            return GetDbSet<ProjectInstitution>().Where(pi => pi.ProjectId == projectId && pi.InstitutionId == institutionId);
        }

        /// <summary>
        /// Checks if the the given institution is already linked to the project
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="institutionId"></param>
        /// <returns></returns>
        public bool InstitutionIsLinkedToProject(int projectId, int institutionId)
        {
            return GetDbSet<ProjectInstitution>().Any(pi => pi.ProjectId == projectId && pi.InstitutionId == institutionId);
        }
    }
}
