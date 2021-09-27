using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Resources
{
    public class SendGridParameters
    {
        public Guid  Guid{ get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string ProjectTitle { get; set; }

        public SendGridParameters(Guid guid, string name, string userName, string projectTitle)
        {
            Guid = guid;
            Name = name;
            UserName = userName;
            ProjectTitle = projectTitle;
        }
    }

}
