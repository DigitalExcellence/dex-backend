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

using API.InputOutput.Tag;
using System;
using System.Collections.Generic;

namespace API.Resources
{

    /// <summary>
    ///     Resource to show single project result
    /// </summary>
    public class ProjectResultInput
    {

        /// <summary>
        ///     Get or Set the project Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Get or Set the name of the project
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Get or Set a short description of the project
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        ///     This gets or sets the collaborators
        /// </summary>
        public IEnumerable<CollaboratorOutput> Collaborators { get; set; }

        /// <summary>
        ///     Get or Set the created date from the project
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        ///     Get or Set the last updated date from the project
        /// </summary>
        public DateTime Updated { get; set; }

        /// <summary>
        ///     Get or Set the owner of the project
        /// </summary>
        public LimitedUserOutput User { get; set; }

        /// <summary>
        ///     This gets or set the file of the project
        /// </summary>
        public FileOutput ProjectIcon { get; set; }

        /// <summary>
        ///     This gets or sets the call to actions of the project.
        /// </summary>
        public List<CallToActionOutput> CallToActions { get; set; }

        /// <summary>
        ///     This gets or sets the likes of the project.
        /// </summary>
        public List<ProjectLikesOutput> Likes { get; set; }

        /// <summary>
        ///     This gets or sets the categories of the project.
        /// </summary>
        public List<ProjectCategoryOutput> Categories { get; set; }

        /// <summary>
        ///     This gets or sets the tags of the project.
        /// </summary>
        public List<TagOutput> Tags { get; set; }
    }

}
