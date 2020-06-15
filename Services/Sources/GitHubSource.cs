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
    /// GitHubSource
    /// </summary>
    /// <seealso cref="Services.Sources.ISource" />
    public class GitHubSource : ISource
    {
        /// <summary>
        /// Gets the source.
        /// </summary>
        /// <param name="uri">The URI of the source project.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void GetSource(Uri uri)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the project information.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>The project object.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public Project GetProjectInformation(Uri uri)
        {
            throw new NotImplementedException();

        }

        /// <summary>
        /// Projects the URI matches.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>true if the project uri matches.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool ProjectURIMatches(Uri uri)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Searches the specified search term.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void Search(string searchTerm)
        {
            throw new NotImplementedException();
        }
    }
}
