using Models;
using Repositories;
using Services.Base;
using Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MessageBrokerPublisher.HelperClasses;
using SendGrid;
using SendGrid.Helpers.Mail;
using Services.Resources;

namespace Services.Services
{
    public interface IProjectTransferService : IService<ProjectTransferRequest>
    {
        Task <Response>InitiateTransfer(Project project, User potentialNewOwner);
        Task<ProjectTransferRequest> FindTransferByGuid(Guid guid);
        Task<ProjectTransferRequest> ProcessTransfer(ProjectTransferRequest transferRequest, bool boolisOwnerMail, bool acceptedRequest);
    }
    public class ProjectTransferService : Service<ProjectTransferRequest>, IProjectTransferService
    {
        private readonly IProjectTransferRepository repository;
        private readonly IProjectService projectService;
        private readonly MailClient mailClient;
        

        public ProjectTransferService(IProjectTransferRepository repository, MailClient mailClient, IProjectService projectService) :
            base(repository)
        {
            this.repository = repository;
            this.mailClient = mailClient;
            this.projectService = projectService;
        }

        public async Task<ProjectTransferRequest> FindTransferByGuid(Guid guid)
        {
            return await repository.FindTransferByGuid(guid);
        }

        public async Task <Response> InitiateTransfer(Project project, User potentialNewOwner)
        {
            List <ProjectTransferRequest> existingTransferRequests = await repository.FindTransferByProjectId(project.Id);

            if(existingTransferRequests != null && existingTransferRequests.Count >= 1)
            {
                foreach(ProjectTransferRequest existingRequest in existingTransferRequests)
                {
                    if(existingRequest.Status == ProjectTransferRequestStatus.Pending)
                    {
                        throw new ProjectTransferAlreadyInitiatedException("A transfer request is already initiated for this project");
                    }
                }
            }
            

            ProjectTransferRequest transferRequest = new ProjectTransferRequest(project, potentialNewOwner);

            Response response = await mailClient.SendTemplatedMail(transferRequest.Project.User.Email, transferRequest.TransferGuid, "d-6680df0406bf488e9810802bbaa29f2e",transferRequest.PotentialNewOwner.Name,transferRequest.Project.Name);
            if(response.IsSuccessStatusCode)
            {
                repository.Add(transferRequest);
                repository.Save();
            }

            return response;
        }

        public async Task <ProjectTransferRequest> ProcessTransfer(ProjectTransferRequest transferRequest, bool isOwnerMail, bool acceptedRequest)
        {
            if(transferRequest.Status == ProjectTransferRequestStatus.Pending)
            {
                if(isOwnerMail && acceptedRequest)
                {
                    //Current project owner clicked mail and accepted request
                    transferRequest.CurrentOwnerAcceptedRequest = true;

                    await mailClient.SendTemplatedMail(transferRequest.PotentialNewOwner.Email, transferRequest.TransferGuid, "d-898692df37204f57b31a224e715f4433",transferRequest.Project.User.Name,transferRequest.Project.Name);

                    Repository.Update(transferRequest);
                    Repository.Save();

                    return transferRequest;
                }

                if(isOwnerMail == false && acceptedRequest)
                {
                    //Mail clicked by new owner and accepted transfer request
                    transferRequest.PotentialNewOwnerAcceptedRequest = true;


                    Repository.Update(transferRequest);
                    Repository.Save();


                    //Transfer project to new owner
                    transferRequest.Project.User = transferRequest.PotentialNewOwner;
                    projectService.Update(transferRequest.Project);
                    projectService.Save();


                    //Finish transfer, after project is actually transfered to new user.
                    transferRequest.Status = ProjectTransferRequestStatus.Completed;

                    Repository.Update(transferRequest);
                    Repository.Save();

                    return transferRequest;
                }
                else
                {
                    transferRequest.Status = ProjectTransferRequestStatus.Denied;

                    Repository.Update(transferRequest);
                    Repository.Save();

                    return transferRequest;
                    //TODO: Send mail to owner about denied transfer
                }
            } 

            return transferRequest;
        }
    }
}
