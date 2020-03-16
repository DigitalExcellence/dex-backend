using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Project
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
    }
}