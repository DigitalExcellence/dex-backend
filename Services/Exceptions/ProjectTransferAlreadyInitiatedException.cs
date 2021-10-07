using System;
using System.Runtime.Serialization;

namespace Services.Services
{
    [Serializable]
    internal class ProjectTransferAlreadyInitiatedException : Exception
    {
        public ProjectTransferAlreadyInitiatedException()
        {
        }

        public ProjectTransferAlreadyInitiatedException(string message) : base(message)
        {
        }

        public ProjectTransferAlreadyInitiatedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProjectTransferAlreadyInitiatedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}