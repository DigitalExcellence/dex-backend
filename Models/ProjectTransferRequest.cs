using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class ProjectTransferRequest
    {
        public int Id { get; set; }
        public Project Project { get; set; }
        public User PotentialNewOwner { get; set; }

       public bool CurrentOwnerAcceptedRequest { get; set; }
        public bool PotentialNewOwnerAcceptedRequest { get; set; }
        public ProjectTransferRequestStatus Status { get; set; }

    }

    public enum ProjectTransferRequestStatus
    {
        Pending,
        Denied,
        Completed
    }

}
