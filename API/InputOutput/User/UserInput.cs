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

namespace API.Resources
{

    /// <summary>
    ///     The view model of a user
    /// </summary>
    public class UserInput
    {

        /// <summary>
        ///     This gets or sets the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     This gets or sets the email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     This gets or sets the Identity Id from external identity provider
        /// </summary>
        public string IdentityId { get; set; }

        /// <summary>
        ///     This gets or sets the ProfileUrl
        /// </summary>
        public string ProfileUrl { get; set; }

        /// <summary>
        ///     This gets or sets the Institution Id.
        /// </summary>
        public int? InstitutionId { get; set; }

        /// <summary>
        ///     This gets or sets the Expected Graduation DateTime.
        /// </summary>
        public DateTime? ExpectedGraduationDateTime { get; set; }

    }

}
