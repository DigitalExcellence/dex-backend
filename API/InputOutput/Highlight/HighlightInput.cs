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
using System.IO;

namespace API.Resources
{

    /// <summary>
    ///     The view model of a highlight
    /// </summary>
    public class HighlightInput
    {

        /// <summary>
        ///     This gets or sets the id of the project that this highlight is associated with
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        ///     This gets or sets the description of the project that this highlight is associated with
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     This gets or sets the start date that the highlight should start
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        ///     This gets or sets the end date that highlight should end
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        ///     This gets or sets the image id of the project that this highlight is associated with
        /// </summary>
        public int? ImageId { get; set; }

    }

}
