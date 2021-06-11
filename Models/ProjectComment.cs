using System;
using System.Collections.Generic;

namespace Models
{

    public class ProjectComment
    {

        public int Id { get; set; }

        public int ProjectId { get; set; }
        
        public User User { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public string Content { get; set; }

        public List<ProjectCommentLike> Likes { get; set; }
        public ProjectComment()
        {

        }

    }

}
