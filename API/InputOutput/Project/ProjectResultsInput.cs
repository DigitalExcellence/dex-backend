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

namespace API.Resources
{

    /// <summary>
    ///     Resource to show for multiple project results
    /// </summary>
    public class ProjectResultsInput
    {

        /// <summary>
        ///     Get or Set with an Array of Project Result Resources
        /// </summary>
        public ProjectResultInput[] Results { get; set; }

        /// <summary>
        ///     Get or Set Query String (Only for search results)
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        ///     Get or Set Count
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        ///     Get or Set Total Count
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        ///     Get or Set Page of Search Result
        /// </summary>
        public int? Page { get; set; }

        /// <summary>
        ///     Get or Set the total amount of pages of Search Result
        /// </summary>
        public int TotalPages { get; set; }

    }

}
