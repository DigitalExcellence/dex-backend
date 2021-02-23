using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories
{
    public interface IProjectInstitutionLinkingRepository 
    {
        bool InstitutionIsLinkedToProject(int projectId, int institutionId);
    }

    public class ProjectInstitutionLinkingRepository 
    {
    }
}
