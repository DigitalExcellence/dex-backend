using Models;
using Repositories;
using Services.Base;
using Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MessageBrokerPublisher.HelperClasses;

namespace Services.Services
{
    public interface IProjectTransferService : IService<ProjectTransferRequest>
    {
        Task InitiateTransfer(Project project, User potentialNewOwner);
    }
    public class ProjectTransferService : Service<ProjectTransferRequest>, IProjectTransferService
    {
        private readonly IProjectTransferRepository repository;
        private readonly IEmailSender emailSender;

        public ProjectTransferService(IProjectTransferRepository repository, IEmailSender emailSender) :
            base(repository)
        {
            this.repository = repository;
            this.emailSender = emailSender;
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
                ProjectTransferRequest transferRequest = new ProjectTransferRequest(project,potentialNewOwner);

                repository.Add(transferRequest);
                repository.Save();
                emailSender.Send("whbivisd@sharklasers.com", "Test", null);
            }
        }


    }
}
