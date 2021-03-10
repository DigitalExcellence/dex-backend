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
        bool InstitutionIsLinkedToProject(int projectId, int institutionId);

        ProjectInstitution FindByInstitutionIdAndProjectId(int projectId, int institutionId);

        void RemoveByProjectIdAndInstitutionId(int projectId, int institutionId);

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



        public void RemoveByProjectIdAndInstitutionId(int projectId, int institutionId)
        {
            ProjectInstitution projectInstitution = GetDbSet<ProjectInstitution>()
                .FirstOrDefault(p => p.ProjectId == projectId && p.InstitutionId == institutionId);

            if(projectInstitution != null)
                GetDbSet<ProjectInstitution>()
                    .Remove(projectInstitution);
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
