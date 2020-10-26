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
        /// gets or sets Id of the followed project
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Set or get Project
        /// </summary>
        public string Name{ get; set; }
        /// <summary>
        /// set or get User
        /// </summary>
        public string ShortDescription { get; set; }
        /// <summary>
        /// set or get userId
        /// </summary>
        public string Description{ get; set; }

        /// <summary>
        /// Uri project
        /// </summary>
        public string Uri { get; set; } 
    }
}
