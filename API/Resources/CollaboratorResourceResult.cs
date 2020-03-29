using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Resources
{
    public class CollaboratorResourceResult
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
    }
}
