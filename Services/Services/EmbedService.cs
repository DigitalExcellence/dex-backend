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
using Repositories;
using Services.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Services
{

    /// <summary>
    ///     This is the interface for the embed service
    /// </summary>
    public interface IEmbedService : IService<EmbeddedProject>
    {

        /// <summary>
        ///     Finds the asynchronous.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>The found embeddedProject.</returns>
        Task<EmbeddedProject> FindAsync(Guid guid);

        /// <summary>
        ///     Gets the embedded projects asynchronous.
        /// </summary>
        /// <returns>A list of all the embeddedProjects.</returns>
        Task<IEnumerable<EmbeddedProject>> GetEmbeddedProjectsAsync();

        /// <summary>
        ///     Determines whether [is non existing unique identifier] [the specified unique identifier].
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>true if there are no embedded projects with the given guid.</returns>
        Task<bool> IsNonExistingGuidAsync(Guid guid);

    }

    /// <summary>
    ///     EmbedService
    /// </summary>
    /// <seealso cref="EmbeddedProject" />
    /// <seealso cref="IEmbedService" />
    public class EmbedService : Service<EmbeddedProject>, IEmbedService
    {

        /// <summary>
        ///     Initializes a new instance of the <see cref="EmbedService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public EmbedService(IEmbedRepository repository) : base(repository) { }

        /// <summary>
        ///     Gets the repository.
        /// </summary>
        /// <value>
        ///     The repository.
        /// </value>
        protected new IEmbedRepository Repository => (IEmbedRepository) base.Repository;

        /// <summary>
        ///     Finds the asynchronous.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>The found embeddedProject.</returns>
        public async Task<EmbeddedProject> FindAsync(Guid guid)
        {
            return await Repository.GetEmbeddedProjectAsync(guid);
        }

        /// <summary>
        ///     Gets the embedded projects asynchronous.
        /// </summary>
        /// <returns>A list of all embeddedProjects.</returns>
        public async Task<IEnumerable<EmbeddedProject>> GetEmbeddedProjectsAsync()
        {
            return await Repository.GetEmbeddedProjectsAsync();
        }

        /// <summary>
        ///     Determines whether [is non existing unique identifier] [the specified unique identifier].
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>true if there are no embedded projects with the given guid.</returns>
        public async Task<bool> IsNonExistingGuidAsync(Guid guid)
        {
            return await Repository.IsNonExistingGuidAsync(guid);
        }

    }

}
