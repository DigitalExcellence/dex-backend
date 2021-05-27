using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class CollaboratorLinkedUser
    {
        /// <summary>
        ///     Gets or sets a the id of the linked user.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Linked user.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        ///     The hash to allow for the Status of this model to change to: 'Accepted'
        /// </summary>
        public string AcceptanceHash { get; set; }

        /// <summary>
        ///     Status of the linked user.
        /// </summary>
        public LinkedUserStatus Status { get; set; }
    }
}
