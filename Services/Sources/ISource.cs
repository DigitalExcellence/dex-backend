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
using System;

namespace Services.Sources
{

    /// <summary>
    ///     This is the source interface
    /// </summary>
    public interface ISource
    {
        /// <summary>
        ///     This is the interface method which checks if the project uri matches
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>boolean</returns>
        bool ProjectURIMatches(Uri uri);

        /// <summary>
        ///     This is the interface method which searches for the specified term
        /// </summary>
        /// <param name="searchTerm"></param>
        void Search(string searchTerm);

        /// <summary>
        ///     This is the interface method which gets the specified source by URI
        /// </summary>
        /// <param name="uri"></param>
        void GetSource(Uri uri);

        /// <summary>
        ///     This is the interface method which gets the project information by URI
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>Project entity</returns>
        Project GetProjectInformation(Uri uri);

    }

}
