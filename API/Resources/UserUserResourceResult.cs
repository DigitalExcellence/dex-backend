using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Resources
{
    /// <summary>
    /// Object to return to frontend with the UserUser
    /// </summary>
    public class UserUserResourceResult
    {
        /// <summary>
        /// Set or gets id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// User object to follow
        /// </summary>
        public string Name { get; set; }
    }
}
