using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.InputOutput.ProjectTransferRequest
{
    /// <summary>
    ///     Project transfer request input
    /// </summary>
    public class ProjectTransferRequestInput
    {
        /// <summary>
        ///     id of the project
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        ///     id of the potential new owner
        /// </summary>
        public int PotentialNewOwnerId { get; set; }
    }
}
