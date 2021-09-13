using Models;
using Repositories;
using Services.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public interface IProjectTransferService : IService<ProjectTransferRequest>
    {
        Task InitiateTransfer(Project project, User potentialNewOwner);
    }
    public class ProjectTransferService : Service<ProjectTransferRequest>, IProjectTransferService
    {
        private readonly IProjectTransferRepository Repository;

        public ProjectTransferService(ProjectTransferRepository repository) :
            base(repository)
        { }

        public async Task InitiateTransfer(Project project, User potentialNewOwner)
        {
            ProjectTransferRequest transferRequest = await Repository.FindTransferByProjectId(project.Id);

            
            if(transferRequest != null || transferRequest.Status == ProjectTransferRequestStatus.Completed)
            {
                throw new Exception("A transfer request is already initiated for this project");
            }
        }


    }
}
