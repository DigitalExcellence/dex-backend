using Models;
using Repositories.Base;
using Services.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Services
{
    public interface IProjectInstitutionLinkingService : IService<ProjectInstitution>
    {
        void InstitutionIsLinkedToProject(int projectId, int institutionId);
    }

    public class ProjectInstitutionLinkingService : Service<ProjectInstitution>, IProjectInstitutionLinkingService
    {
        public ProjectInstitutionLinkingService(IRepository<ProjectInstitution> repository) : base(repository)
        {
        }

        public void InstitutionIsLinkedToProject(int projectId, int institutionId)
        {
            throw new NotImplementedException();
        }
    }
}
