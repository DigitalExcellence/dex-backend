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

using Models;
using System.Collections.Generic;

namespace API.Resources
{

    /// <summary>
    ///     the view model result of user.
    /// </summary>
    public class UserResourceResult
    {

        /// <summary>
        ///     This gets or sets the Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     This gets or sets the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     This gets or sets the Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     This gets or sets the Identity Id of an external provider
        /// </summary>
        public string IdentityId { get; set; }

        /// <summary>
        ///     This gets or sets the ProfileUrl
        /// </summary>
        public string ProfileUrl { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
        public Role Role { get; set; }

        /// <summary>
        /// Gets or sets the institution where the user is registered.
        /// </summary>
        public Institution Institution { get; set; }
       
        /// <summary>
        /// Gets or sets the followed projects for user
        /// </summary>
        public List<UserProjectResourceResult> UserProject { get; set; }

        /// <summary>
        /// Gets or set the tasks the user should follow up.
        /// </summary>
        public List<UserTaskResourceResult> UserTask { get; set; }
    }

}
