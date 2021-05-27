using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Resources
{
    public class ProjectCollaboratorLinkRequestEmail
    {
        public string Recipient { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }

    }
}
