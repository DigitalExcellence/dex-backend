using Models;
using Repositories;
using Repositories.Base;
using Services.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Services
{
    public interface IProjectInstitutionService : IService<ProjectInstitution>
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

        void RemoveByProjectIdAndInstitutionId(int projectId, int institutionId);

    }

    public class ProjectInstitutionService : Service<ProjectInstitution>, IProjectInstitutionService
    {
        public ProjectInstitutionService(IProjectInstitutionRepository repository) : base(repository)
        {
        }

        protected new IProjectInstitutionRepository Repository => (IProjectInstitutionRepository) base.Repository;

        public ProjectInstitution FindByInstitutionIdAndProjectId(int projectId, int institutionId)
        {
            return Repository.FindByInstitutionIdAndProjectId(projectId, institutionId);
        }

        public IEnumerable<ProjectInstitution> FindByInstitutionsIdAndProjectId(int projectId, int institutionId)
        {
            return Repository.FindByInstitutionsIdAndProjectId(projectId, institutionId);
        }

        public void RemoveByProjectIdAndInstitutionId(int projectId, int institutionId)
        {
            Repository.RemoveByProjectIdAndInstitutionId(projectId, institutionId);
        }

        /// <summary>
        /// Checks if the the given institution is already linked to the project
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="institutionId"></param>
        /// <returns></returns>
        public bool InstitutionIsLinkedToProject(int projectId, int institutionId)
        {
            if(institutionId == default)
                return false;

            return Repository.InstitutionIsLinkedToProject(projectId, institutionId);
        }
    }
}
