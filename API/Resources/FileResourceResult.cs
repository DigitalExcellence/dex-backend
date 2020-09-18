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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Resources
{
    /// <summary>
    /// File resource resource result
    /// </summary>
    /// <seealso cref="API.Resources.FileResourceResult" />
    public class FileResourceResult
    {
        /// <summary>
        /// Id of File
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Path of file
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// Upload Date and time
        /// </summary>
        public DateTime UploadDateTime { get; set; }
        /// <summary>
        /// File name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///  Id of uploader
        /// </summary>
        public UserResourceResult Uploader { get; set; }
    }
}
