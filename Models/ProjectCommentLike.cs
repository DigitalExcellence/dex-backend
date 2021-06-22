using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class ProjectCommentLike
    {

        public ProjectCommentLike(ProjectComment likedComment, User commentLiker)
        {
            LikedComment = likedComment;
            CommentLiker = commentLiker;
            Date = DateTime.Now;
        }

        public ProjectCommentLike() { }

        public int Id { get; set; }
        
        public ProjectComment LikedComment { get; set; }

        public User CommentLiker { get; set; }

        public int UserId { get; set; }

        public DateTime Date { get; set; }
    }
}
