using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Resources
{
    public class ProjectCommentResource
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public int UserId { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public string Content { get; set; }

    }
}
