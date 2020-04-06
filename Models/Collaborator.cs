using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Collaborator
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public int ProjectId { get; set; }
    }
}
