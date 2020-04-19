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
    ///     Resource to show single Search Result
    /// </summary>
    public class SearchResultResource
    {

        /// <summary>
        ///     Get or Set the project Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Get or Set associated user from the project
        /// </summary>
        public UserResourceResult User { get; set; }

        /// <summary>
        ///     Get or Set the name of the project
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Get or Set a short description of the project
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        ///     Get or Set the created date from the project
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        ///     Get or Set the last updated date from the project
        /// </summary>
        public DateTime Updated { get; set; }

    }

}
