using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Role
    {

        public int id { get; set; }
        public List<RoleScope> Scopes { get; set; }

    }
}
