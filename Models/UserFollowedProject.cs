using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class UserFollowedProject
    {
        public int Id { get; set; }
        public Project Project { get; set; }
        public User User{ get; set; }
        public int UserId { get; set; }

    }


}
