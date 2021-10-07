using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services
{
    /// <summary>
    /// Exception for transfer request
    /// </summary>
    public class ProjectTransferAlreadyInitiatedException : Exception
    {
        /// <summary>
        /// Description of the exception
        /// </summary>
        public ProjectTransferAlreadyInitiatedException()
        {
        }

        /// <summary>
        /// Description of the exception
        /// </summary>
        /// <param name="message"></param>

        public ProjectTransferAlreadyInitiatedException(string message)
        : base(message)
        {
        }
    }
}
