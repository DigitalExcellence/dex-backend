using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class UserUser
    {
        public UserUser(User user, User followedUser)
        {
            User = user;
            FollowedUser = followedUser;
        }
        public UserUser() { }

        public int Id { get; set; }
        public User User{ get; set; }
        public User FollowedUser { get; set; }
        public int FollowedUserId { get; set; }
    }
}
