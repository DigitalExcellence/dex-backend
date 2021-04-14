using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repositories
{
    /// <summary>
    ///     This is the interface of the project institution repository that is used to get and mutate project institutions
    /// </summary>
    public interface IProjectInstitutionRepository : IRepository<ProjectInstitution>
    {
        /// <summary>
        /// Checks if the the given institution is already linked to the project
        /// </summary>
        /// <param name="projectId">Project identifier</param>
        /// <param name="institutionId">Institution identifier</param>
        /// <returns></returns>
        bool InstitutionIsLinkedToProject(int projectId, int institutionId);

        /// <summary>
        /// Finds ProjectInstitution by project and institution Id
        /// </summary>
        /// <param name="projectId">Project identifier</param>
        /// <param name="institutionId">Institution identifier</param>
        /// <returns></returns>
        ProjectInstitution FindByInstitutionIdAndProjectId(int projectId, int institutionId);

        /// <summary>
        /// Removes institution from project by project id and institution Id
        /// </summary>
        /// <param name="projectId">Project identifier</param>
        /// <param name="institutionId">Institution identifier</param>
        void RemoveByProjectIdAndInstitutionId(int projectId, int institutionId);

    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public class ProjectInstitutionRepository : Repository<ProjectInstitution>, IProjectInstitutionRepository
    {
        public ProjectInstitutionRepository(DbContext dbContext) : base(dbContext)
        {
        }

        /// <inheritdoc/>
        public ProjectInstitution FindByInstitutionIdAndProjectId(int projectId, int institutionId)
        {
            return GetDbSet<ProjectInstitution>().FirstOrDefault(pi => pi.ProjectId == projectId && pi.InstitutionId == institutionId);
        }


        /// <inheritdoc/>
        public void RemoveByProjectIdAndInstitutionId(int projectId, int institutionId)
        {
            ProjectInstitution projectInstitution = GetDbSet<ProjectInstitution>()
                .FirstOrDefault(p => p.ProjectId == projectId && p.InstitutionId == institutionId);

            if(projectInstitution != null)
                GetDbSet<ProjectInstitution>()
                    .Remove(projectInstitution);
        }

        /// <inheritdoc/>
        public bool InstitutionIsLinkedToProject(int projectId, int institutionId)
        {
            return GetDbSet<ProjectInstitution>().Any(pi => pi.ProjectId == projectId && pi.InstitutionId == institutionId);
        }
    }
}
