using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class ProjectMember
    {
        public int Id { get; set; }
//        public Project Project { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
    }
}
