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
        Task InitiateTransfer(Project project, User potentialNewOwner);
        Task<ProjectTransferRequest> FindTransferByGuid(Guid guid);
        Task ProcessTransfer(ProjectTransferRequest transferRequest, bool boolisOwnerMail, bool acceptedRequest);
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

        public async Task InitiateTransfer(Project project, User potentialNewOwner)
        {
            ProjectTransferRequest existingTransferRequest = await repository.FindTransferByProjectId(project.Id);


            if(existingTransferRequest != null)
            {
                throw new ProjectTransferAlreadyInitiatedException("A transfer request is already initiated for this project");
            }
            else
            {
                ProjectTransferRequest transferRequest = new ProjectTransferRequest(project, potentialNewOwner);

                string subject = "DeX project ownership transfer";
                string to = transferRequest.Project.User.Email;

                Response response = await mailClient.SendTemplatedMail(to, transferRequest.TransferGuid, subject, "d-6680df0406bf488e9810802bbaa29f2e");

                repository.Add(transferRequest);
                repository.Save();
            }
        }

        public async Task ProcessTransfer(ProjectTransferRequest transferRequest, bool isOwnerMail, bool acceptedRequest)
        {
            if(transferRequest.Status == ProjectTransferRequestStatus.Pending)
            {
                if(isOwnerMail && acceptedRequest)
                {
                    //Current project owner clicked mail and accepted request
                    transferRequest.CurrentOwnerAcceptedRequest = true;

                    string subject = "DeX somebody wants to transfer project ownership to you.";
                    Response response = await mailClient.SendTemplatedMail(transferRequest.PotentialNewOwner.Email, transferRequest.TransferGuid, subject, "d-898692df37204f57b31a224e715f4433");

                    Repository.Update(transferRequest);
                    Repository.Save();
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
                }
            
            } else
            {
                transferRequest.Status = ProjectTransferRequestStatus.Denied;

                Repository.Update(transferRequest);
                Repository.Save();
                //TODO: Send mail to owner about denied transfer
            }
        }
    }
}
