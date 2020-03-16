using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class User
    {
        public int Id { get; set; }

        public User()
        {
        }

        public User(int userId) : this()
        {
            Id = userId;
        }

        public string Username { get; set; }
        public string ProfileUrl { get; set; }
    }
}