using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Role
    {

        public int Id { get; set; }

        public string Name { get; set; }
        public List<RoleScope> Scopes { get; set; }

    }
}
