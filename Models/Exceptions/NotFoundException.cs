using System;

namespace Models.Exceptions
{

    /// <summary>
    /// This exception will get thrown if an object or record can't be found.
    /// </summary>
    [Serializable]
    public class NotFoundException : Exception
    {

        /// <summary>
        /// The constructor of the class to generate the exception with a specified message.
        /// </summary>
        /// <param name="notFoundObject">The name of the objects that could not get found.</param>
        public NotFoundException(string notFoundObject)
            : base($"{notFoundObject} could not get found.") { }

        /// <summary>
        /// The constructor of the class to generate the exception with a specified message.
        /// </summary>
        /// <param name="notFoundObject">The name of the objects that could not get found.</param>
        /// <param name="inner">The inner exception of the thrown exception.</param>
        public NotFoundException(string notFoundObject, Exception inner)
            : base($"{notFoundObject} could not get found.", inner) { }

    }

}
