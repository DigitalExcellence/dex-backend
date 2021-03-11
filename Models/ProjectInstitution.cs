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

using System.ComponentModel.DataAnnotations;

namespace Models
{
    /// <summary>
    ///     This class contains an institution linked to a project
    /// </summary>
    public class ProjectInstitution
    {
        public ProjectInstitution()
        {

        }

        public ProjectInstitution(int projectId, int institutionId)
        {
            this.ProjectId = projectId;
            this.InstitutionId = institutionId;
        }
        /// <summary>
        ///     Gets or set the identifier.
        /// </summary>
        /// <value>
        ///     The identifier.
        /// </value>
        public int Id { get; set; }
        /// <summary>
        ///     Gets or set the project identifier.
        /// </summary>
        /// <value>
        ///     The project identifier.
        /// </value>
        public int ProjectId { get; set; }
        /// <summary>
        ///     Gets or set the project.
        /// </summary>
        /// <value>
        ///     The project.
        /// </value>
        public Project Project { get; set; }
        /// <summary>
        ///     Gets or set the institution identifier.
        /// </summary>
        /// <value>
        ///     The institution identifier.
        /// </value>
        public int InstitutionId { get; set; }
        /// <summary>
        ///     Gets or set the institution.
        /// </summary>
        /// <value>
        ///     The institution.
        /// </value>
        public Institution Institution { get; set; }
        
    }
}
