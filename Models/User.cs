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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{

    public class User
    {

        public User()
        {
            Projects = new List<Project>();
            Services = new List<LinkedService>();
            UserProject = new List<UserProject>();
            FollowedUsers = new List<UserUser>();
            LikedProjectsByUsers = new List<ProjectLike>();
            UserTasks = new List<UserTask>();
            AccountCreationDate = DateTime.Now;
            ExpectedGraduationDate = DateTime.Now.AddYears(4);
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public Role Role { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string IdentityId { get; set; }

        public List<Project> Projects { get; set; }

        public List<LinkedService> Services { get; set; }

        public List<UserProject> UserProject { get; set; }

        [InverseProperty("FollowedUser")]
        public List<UserUser> FollowedUsers { get; set; }

        public string ProfileUrl { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the user profile is public.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is public; otherwise, <c>false</c>.
        /// </value>
        public bool IsPublic { get; set; }

        public DateTime? AccountCreationDate { get; set; }

        public DateTime? ExpectedGraduationDate { get; set; }

        /// <summary>
        ///     Gets or sets the value of the list object which keeps project list
        ///     who liked by an individual user.
        /// </summary>
        /// <value>
        ///     The list object that keeps projects.
        /// </value>
        public List<ProjectLike> LikedProjectsByUsers { get; set; }


        /// <summary>
        ///     Gets or sets a value for the Id of the institution where the user is registered.
        /// </summary>
        public int? InstitutionId { get; set; }

        /// <summary>
        ///     Gets or sets the institution where the user is registered.
        /// </summary>
        public Institution Institution { get; set; }

        /// <summary>
        ///     Gets or sets the user tasks
        /// </summary>
        public List<UserTask> UserTasks { get; set; }

    }

}
