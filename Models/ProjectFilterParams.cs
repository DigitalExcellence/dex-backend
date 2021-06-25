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

using System.Collections.Generic;

namespace Models
{

    /// <summary>
    ///     This class contains all the parameters used to filter, sort and paginate projects
    /// </summary>
    public class ProjectFilterParams
    {

        /// <summary>
        ///     The page of the results to return (a subset of the results)
        /// </summary>
        public int? Page { get; set; }

        /// <summary>
        ///     The amount of results on a page
        /// </summary>
        public int? AmountOnPage { get; set; }

        /// <summary>
        ///     By which property the results should be sorted
        ///     Possible values are: name, created, updated, likes
        /// </summary>
        public string SortBy { get; set; }

        /// <summary>
        ///     The direction to sort the results
        ///     Possible values are: asc (Ascending), desc (Descending)
        /// </summary>
        public string SortDirection { get; set; }

        // <summary>
        //      Get or set the array of category id's
        // </summary>
        public ICollection<int> Categories { get; set; }

        /// <summary>
        ///     This property filter the projects on the highlighted state
        ///     Possible value:
        ///     - null (Return all results)
        ///     - true (Only return highlighted results)
        ///     - false (Only return not highlighted results)
        /// </summary>
        public bool? Highlighted { get; set; }

    }

}
