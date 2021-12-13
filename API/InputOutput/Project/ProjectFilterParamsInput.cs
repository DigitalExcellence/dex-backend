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

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace API.Resources
{

    /// <summary>
    ///     This resource contains all the query parameters used to filter, sort and paginate projects
    /// </summary>
    public class ProjectFilterParamsInput
    {

        /// <summary>
        ///     Get or Set the page in query parameter
        /// </summary>
        [FromQuery(Name = "page")]
        public int? Page { get; set; }

        /// <summary>
        ///     Get or Set the amount of Search Results on a single page
        /// </summary>
        [FromQuery(Name = "amountOnPage")]
        public int? AmountOnPage { get; set; }

        /// <summary>
        ///     Get or Set the sort by query parameter
        ///     Possible sorts are: name, created, updated, likes
        /// </summary>
        [FromQuery(Name = "sortBy")]
        public string SortBy { get; set; }

        /// <summary>
        ///     Get or Set the direction of sorting by query parameter.
        ///     Possible options are : asc, desc
        /// </summary>
        [FromQuery(Name = "sortDirection")]
        public string SortDirection { get; set; }

        /// <summary>
        ///      Get or set the array of category id's
        ///
        /// </summary>
        [FromQuery(Name = "categories")]
        public ICollection<int> Categories { get; set; }

        /// <summary>
        ///     This property filter the projects on the highlighted state
        ///     Possible value:
        ///     - null (Return all results)
        ///     - true (Only return highlighted results)
        ///     - false (Only return not highlighted results)
        /// </summary>
        [FromQuery(Name = "highlighted")]
        public bool? Highlighted { get; set; }

        /// <summary>
        ///     Get or set the array of tag id's
        /// </summary>
        [FromQuery(Name = "tags")]
        public ICollection<int> Tags { get; set; }

    }

}
