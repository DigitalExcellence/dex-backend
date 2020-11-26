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
    /// This class contains all the properties for a call to action option.
    /// An example would be the title of an call to action. The type would be
    /// 'Title' and the Value is the text on the call to action.
    /// </summary>
    public class CallToActionOptions
    {
        /// <summary>
        /// Gets or sets a value for the Id property.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a value for the Type property.
        /// </summary>
        [Required]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets a value for the Value property.
        /// </summary>
        [Required]
        public string Value { get; set; }

    }

}
