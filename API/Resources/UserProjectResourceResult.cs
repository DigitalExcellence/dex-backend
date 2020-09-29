using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Resources
{
    /// <summary>
    /// Object to return to frontend with the UserProject
    /// </summary>
    public class UserProjectResourceResult
    {
        /// <summary>
        /// PK of UserProject
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Set or get Project
        /// </summary>
        public Project Project { get; set; }
        /// <summary>
        /// set or get User
        /// </summary>
        public User User { get; set; }
        /// <summary>
        /// set or get userId
        /// </summary>
        public int UserId { get; set; }
    }
}
