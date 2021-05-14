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
    ///     This class contains all the properties for a call to action model.
    /// </summary>
    public class CallToAction
    {

        /// <summary>
        ///     Gets or sets a value for the Id property.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets a value for the OptionValue property.
        ///     For example, this would be the Title on the call to action button.
        /// </summary>
        [Required]
        public string OptionValue { get; set; }

        /// <summary>
        ///     Gets or sets a value for the Value property.
        ///     For example, this would be the redirect url for the call to action button.
        /// </summary>
        [Required]
        public string Value { get; set; }

        /// <summary>
        ///     Gets or sets a value for the ProjectId property.
        ///     For example, this would be the projectId this call to action is linked to.
        /// </summary>
        [Required]
        public int ProjectId { get; set; }

    }

}
