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
using System.Linq;

namespace Models
{

    public class Project
    {

        public Project()
        {
            Collaborators = new List<Collaborator>();
            LinkedInstitutions = new List<ProjectInstitution>();
        }

        public int Id { get; set; }

        [Required]
        public User User { get; set; }

        public int UserId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public string ShortDescription { get; set; }

        public List<Collaborator> Collaborators { get; set; }

        public List<ProjectInstitution> LinkedInstitutions { get; set; }

        [Required]
        public string Uri { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public DateTime Updated { get; set; }

        public int? ProjectIconId { get; set; }

        public File ProjectIcon { get; set; }

        public CallToAction CallToAction { get; set; }

        public List<ProjectLike> Likes { get; set; }

        public bool InstitutePrivate { get; set; }

        /// <summary>
        /// Checks if the user can access the project based on
        /// if the insitution is private, if the user is part of an institution linked to this project
        /// or if the user is the one who created the project
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool CanAccess(User user)
        {
            if(InstitutePrivate == false)
            {
                return true;
            }

            if(User.Id == user.Id || LinkedInstitutions.Any(li => li.InstitutionId == user.InstitutionId))
            {
                return true;
            }

            return false;
        }

        public bool IsCreator(int userId)
        {
            return this.UserId == userId;
        }

    }

}
