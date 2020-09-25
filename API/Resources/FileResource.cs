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

using Microsoft.AspNetCore.Http;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Resources
{

    /// <summary>
    /// Embedded project resource result
    /// </summary>
    /// <seealso cref="API.Resources.FileResource" />
    public class FileResource
    {
        /// <summary>
        /// IFormFile 
        /// </summary>
        public IFormFile File { get; set; }
        /// <summary>
        /// Date and time of uploading
        /// </summary>
        public DateTime UploadDateTime { get; set; }

        /// <summary>
        /// UploaderId of file
        /// </summary>
        public User Uploader { get; set; }
        /// <summary>
        /// Id of project
        /// </summary>
        public int ProjectId { get; set; }
    }
}
