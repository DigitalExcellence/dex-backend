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
    }
    public class ProjectTransferService : Service<ProjectTransferRequest>, IProjectTransferService
    {
        private readonly IProjectTransferRepository repository;
        private readonly MailClient mailClient;
        

        public ProjectTransferService(IProjectTransferRepository repository, MailClient mailClient) :
            base(repository)
        {
            this.repository = repository;
            this.mailClient = mailClient;
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
                string subject = "DeX project ownership transfer";

                //TODO Change email to project owner
                string to = "email here";
                string plainTextContent = "We inform you...";
                string htmlContent = "<Button>Confirm transfer</Button> <Button>Cancel transfer</Button>";

                Response response = await mailClient.SendMail(to, plainTextContent, subject, htmlContent);

                ProjectTransferRequest transferRequest = new ProjectTransferRequest(project, potentialNewOwner);

                //repository.Add(transferRequest);
               // repository.Save();
            }
        }


    }
}
