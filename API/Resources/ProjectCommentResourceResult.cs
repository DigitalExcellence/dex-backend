using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Resources
{
    public class ProjectCommentResourceResult
    {

        public int Id { get; set; }

        public User User { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public string Content { get; set; }

        public List<ProjectCommentLike> Likes { get; set; }
    }
}
