using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Resources
{
    public class UserTaskResourceResult
    {
        public int Id { get; set; }
        public UserResourceResult UserResourceResult { get; set; }

        public string Status { get; set; }
        public string Type { get; set; }
    }
}
