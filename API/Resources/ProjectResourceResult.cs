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
    ///     The view model result of project.
    /// </summary>
    public class ProjectResourceResult
    {
        /// <summary>
        ///     Get or Set Id of a Project Resource Result
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     This gets or sets the user.
        /// </summary>
        public LimitedUserResourceResult User { get; set; }

        /// <summary>
        ///     This gets or sets the userId.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        ///     This gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     This gets or sets the Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     This gets or sets the Short Description.
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        ///     This gets or sets the Uri.
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        ///     This gets or sets the collaborators.
        /// </summary>
        public ICollection<CollaboratorResourceResult> Collaborators { get; set; }

        /// <summary>
        ///     This gets or sets the Created time of the project.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        ///     This gets or sets the Updated time of the project.
        /// </summary>
        public DateTime Updated { get; set; }
        /// <summary>
        /// This gets or set the file of the project.
        /// </summary>
        public FileResourceResult ProjectIcon { get; set; }
        /// <summary>
        /// This gets or sets the call to action.
        /// </summary>
        public CallToActionResourceResult CallToAction { get; set; }
        /// <summary>
        /// This gets or sets the likes of the project.
        /// </summary>
        public List<ProjectLikesResourceResult> Likes { get; set; }

        /// <summary>
        /// Sets or gets if project is visible to institute members only or not
        /// </summary>
        public bool InstitutePrivate { get; set; }
    }
}
