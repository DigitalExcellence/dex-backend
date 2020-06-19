using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class IdentityUser
    {

        public string Password { get; set; }

        public object UserId { get; set; }

        public string Email { get; set; }

        public string Lastname { get; set; }

        public string Firstname { get; set; }

        public bool IsActive { get; set; }

    }
}
