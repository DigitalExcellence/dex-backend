using System;
using System.Collections.Generic;

namespace Models
{

    public class ProjectComment
    {

        public int Id { get; set; }

        public int ProjectId { get; set; }

        public User User { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public string Content { get; set; }

        public List<ProjectCommentLike> Likes { get; set; }


    }

}
