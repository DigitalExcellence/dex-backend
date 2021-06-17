using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Resources
{
    public class CollaboratorLinkedUserResourceResult
    {
        /// <summary>
        ///     Gets or sets a the id of the linked user.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Linked user.
        /// </summary>
        public UserResourceResult User { get; set; }

        /// <summary>
        ///     Status of the linked user.
        /// </summary>
        public LinkedUserStatus Status { get; set; }
    }
}
