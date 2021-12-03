using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models
{
    public class ProjectTransferRequest
    {
        public ProjectTransferRequest(Project project, User potentialNewOwner)
        {
            Project = project;
            PotentialNewOwner = potentialNewOwner;
            TransferGuid = Guid.NewGuid();
        }

        public ProjectTransferRequest()
        {

        }

        public int Id { get; set; }

        [Required]
        public Project Project { get; set; }
        [Required]
        public User PotentialNewOwner { get; set; }

        public bool CurrentOwnerAcceptedRequest { get; set; }
        public bool PotentialNewOwnerAcceptedRequest { get; set; }
        public ProjectTransferRequestStatus Status { get; set; }
        public Guid TransferGuid { get; set; }


    }

    public enum ProjectTransferRequestStatus
    {
        Pending,
        Denied,
        Completed
    }

}
