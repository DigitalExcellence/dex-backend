using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Resources
{
    public class LinkedUserResource
    {
        /// <summary>
        /// Linked user id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Status of the linked user
        /// </summary>
        public LinkedUserStatus Status { get; set; }
    }
}
