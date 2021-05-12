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

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public string Content { get; set; }

        public List<ProjectCommentLike> Likes { get; set; }
    }
}
