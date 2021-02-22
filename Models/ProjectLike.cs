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

namespace Models
{

    /// <summary>
    ///     The Model class that represents
    ///     individual projects liked by users.
    /// </summary>
    public class ProjectLike
    {

        public ProjectLike(Project likedProject, User projectLiker)
        {
            LikedProject = likedProject;
            ProjectLiker = projectLiker;
            Date = DateTime.Now;
        }

        public ProjectLike() { }

        /// <summary>
        ///     Gets or set the identifier.
        /// </summary>
        /// <value>
        ///     The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        ///     Gets or set the individual project that
        ///     liked by a user.
        /// </summary>
        /// <value>
        ///     The Project instance.
        /// </value>
        public Project LikedProject { get; set; }

        /// <summary>
        ///     Gets or set the project liker.
        /// </summary>
        /// <value>
        ///     The User instance of the one who likes the project.
        /// </value>
        public User ProjectLiker { get; set; }

        /// <summary>
        ///     Gets or sets the user who liked the project.
        /// </summary>
        /// <value>
        ///     The user identifier.
        /// </value>
        public int UserId { get; set; }

        public DateTime Date { get; set; }

    }

}
