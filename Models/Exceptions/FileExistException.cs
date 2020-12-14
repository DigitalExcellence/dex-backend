using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Exceptions
{
    /// <summary>
    /// File already exists exception
    /// </summary>
    [Serializable]
    public class FileExistException : Exception
    {
        /// <summary>
        /// File already exist constructor
        /// </summary>
        /// <param name="name"></param>
        public FileExistException(string name)
            : base($"File {name} already exists")
        { }
    }
}
