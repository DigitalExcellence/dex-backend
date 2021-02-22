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
    ///     This exception will get thrown if the functionality does not get supported
    ///     by the external API.
    /// </summary>
    [Serializable]
    public class NotSupportedByExternalApiException : Exception
    {

        /// <summary>
        ///     The constructor of the class to generate the exception with a specified message.
        /// </summary>
        /// <param name="failedDataSource">The name of the data source which does not support the functionality.</param>
        /// <param name="methodName">The name of the method that is not supported by the external API.</param>
        public NotSupportedByExternalApiException(string failedDataSource, string methodName)
            : base($"{failedDataSource} does not support the {methodName} functionality") { }

        /// <summary>
        ///     The constructor of the class to generate the exception with a specified message and inner exception.
        /// </summary>
        /// <param name="failedDataSource">The message of the thrown exception.</param>
        /// ///
        /// <param name="methodName">The name of the method that is not supported by the external API.</param>
        /// <param name="inner">The inner exception of the thrown exception.</param>
        public NotSupportedByExternalApiException(string failedDataSource, string methodName, Exception inner)
            : base($"{failedDataSource} does not support the {methodName} functionality", inner) { }

    }

}
