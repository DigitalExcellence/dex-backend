using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Resources
{
    public class UserProjectResourceResult
    {
        public int Id { get; set; }
        public Project Project { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}
