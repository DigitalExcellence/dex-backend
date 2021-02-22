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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.ExternalDataProviders
{

    /// <summary>
    /// The interface for a data source adaptee that supports the public flow.
    /// </summary>
    public interface IPublicDataSourceAdaptee : IDataSourceAdaptee
    {

        /// <summary>
        /// This method is responsible for retrieving all public projects from a user, via the username, from the external data
        /// source adaptee API.
        /// </summary>
        /// <param name="username">The username which will be used to search to retrieve all public projects from the user.</param>
        /// <returns>This method returns a collections of public projects from the user</returns>
        public Task<IEnumerable<Project>> GetAllPublicProjects(string username);

        /// <summary>
        /// This method is responsible for retrieving a public project from a uri, from the external data source adaptee API.
        /// </summary>
        /// <param name="sourceUri">The source uri which will be used to retrieve the correct project.</param>
        /// <returns>This method returns a public project from the specified source uri.</returns>
        public Task<Project> GetPublicProjectFromUri(Uri sourceUri);

        /// <summary>
        /// This method is responsible for retrieving a public project from the user, by id from the external data source
        /// adaptee API.
        /// </summary>
        /// <param name="identifier">The identifier which will be used to retrieve the correct project.</param>
        /// <returns>This method returns a public project with the specified identifier.</returns>
        public Task<Project> GetPublicProjectById(string identifier);

    }

}
