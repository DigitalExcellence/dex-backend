using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class UserProject
    {
        public UserProject(Project project, User user)
        {
            Project = project;
            User = user;
        }

        public UserProject()
        {

        }

        public int Id { get; set; }
        public Project Project { get; set; }
        public User User{ get; set; }
        public int UserId { get; set; }
    }


}
