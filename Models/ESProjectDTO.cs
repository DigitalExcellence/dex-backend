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
using System.Collections.Generic;

namespace Models
{

    /// <summary>
    /// This model is the model used to create, update or delete documents in Elastic Search.
    /// </summary>
    public class ESProjectDTO
    {

        /// <summary>
        /// This gets or sets the created date of the project 
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// This gets or sets the Id of the project
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// This gets or sets the name of the project 
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// This gets or sets the description of the project 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// This gets or sets the likes of the project. Likes are stored as an array of user id's who like the project.
        /// </summary>
        public List<int> Likes { get; set; }

    }

}
