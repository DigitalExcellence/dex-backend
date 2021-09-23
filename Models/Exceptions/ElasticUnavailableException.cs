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
    ///     This exception will be thrown if Elastic is not available
    /// </summary>
    [Serializable]
    public class ElasticUnavailableException : Exception
    {
        /// <summary>
        ///     Elastic Search unavailable exception constructor
        /// </summary>
        public ElasticUnavailableException()
            : base($"ElasticSearch service is unavailable.") { }
    }
}
