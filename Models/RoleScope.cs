using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class RoleScope
    {
        public RoleScope(int id, string scope)
        {
            Id = id;
            Scope = scope;
        }
        public RoleScope(string scope)
        {
            Scope = scope;
        }
        public int Id { get; set; }
        public string Scope { get; set; }
    }
}
