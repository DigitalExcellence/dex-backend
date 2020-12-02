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

namespace API.Resources
{
    /// <summary>
    ///     The view model of a project
    /// </summary>
    public class ProjectResource
    {
        /// <summary>
        ///     This gets or sets the Title
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     This gets or sets the Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     This gets or sets the Short description of the project
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        ///     This gets or sets the Uri
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        ///     This gets or sets the collaborators
        /// </summary>
        public ICollection<CollaboratorResource> Collaborators { get; set; }

        /// <summary>
        /// This gets or sets the file id
        /// </summary>
        public int FileId { get; set; }

        /// <summary>
        /// This gets or sets the call to action
        /// </summary>
        public CallToActionResource CallToAction { get; set; }
    }
}
