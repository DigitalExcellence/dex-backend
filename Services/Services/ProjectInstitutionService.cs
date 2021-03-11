using Models;
using Repositories;
using Repositories.Base;
using Services.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Services
{
    /// <summary>
    ///     This is the interface of the project institution service that is used to get and mutate project institutions
    /// </summary>
    public interface IProjectInstitutionService : IService<ProjectInstitution>
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
    public class ProjectInstitutionService : Service<ProjectInstitution>, IProjectInstitutionService
    {
        public ProjectInstitutionService(IProjectInstitutionRepository repository) : base(repository)
        {
        }

        /// <summary>
        /// The project institution repository
        /// </summary>
        protected new IProjectInstitutionRepository Repository => (IProjectInstitutionRepository) base.Repository;

        /// <inheritdoc/>
        public ProjectInstitution FindByInstitutionIdAndProjectId(int projectId, int institutionId)
        {
            return Repository.FindByInstitutionIdAndProjectId(projectId, institutionId);
        }

        /// <inheritdoc/>
        public void RemoveByProjectIdAndInstitutionId(int projectId, int institutionId)
        {
            Repository.RemoveByProjectIdAndInstitutionId(projectId, institutionId);
        }

        /// <inheritdoc/>
        public bool InstitutionIsLinkedToProject(int projectId, int institutionId)
        {
            if(institutionId == default)
                return false;

            return Repository.InstitutionIsLinkedToProject(projectId, institutionId);
        }
    }
}
