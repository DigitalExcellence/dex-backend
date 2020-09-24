using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class UserFollowedProject
    {
        public UserFollowedProject(Project project, User user)
        {
            Project = project;
            User = user;
        }

        public UserFollowedProject()
        {

        }

        public int Id { get; set; }
        public Project Project { get; set; }
        public User User{ get; set; }
        public int UserId { get; set; }
    }


}
