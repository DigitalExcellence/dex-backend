/*
* Digital Excellence Copyright (C) 2020 Brend Smits
*
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU Lesser General Public License as published
* by the Free Software Foundation version 3 of the License.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty
* of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
* See the GNU Lesser General Public License for more details.
*
* You can find a copy of the GNU Lesser General Public License
* along with this program, in the LICENSE.md file in the root project directory.
* If not, see https://www.gnu.org/licenses/lgpl-3.0.txt
*/

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
