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

using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories
{

    public interface IEmbedRepository : IRepository<EmbeddedProject>
    {
        /// <summary>
        /// Gets the embedded project asynchronous.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        Task<EmbeddedProject> GetEmbeddedProjectAsync(Guid guid);
        /// <summary>
        /// Gets the embedded projects asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<EmbeddedProject>> GetEmbeddedProjectsAsync();
        /// <summary>
        /// Determines whether [is non existing unique identifier] [the specified unique identifier].
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        Task<bool> IsNonExistingGuidAsync(Guid guid);
    }
    /// <summary>
    /// EmbedRepository
    /// </summary>
    /// <seealso cref="Repositories.Base.Repository{Models.EmbeddedProject}" />
    /// <seealso cref="Repositories.IEmbedRepository" />
    public class EmbedRepository : Repository<EmbeddedProject>, IEmbedRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmbedRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public EmbedRepository(DbContext dbContext) : base(dbContext) { }

        /// <summary>
        /// Gets the embedded project asynchronous.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        public async Task<EmbeddedProject> GetEmbeddedProjectAsync(Guid guid)
        {
            return await GetDbSet<EmbeddedProject>()
                .Where(e => e.Guid == guid)
                .Include(p => p.Project)
                .ThenInclude(p => p.Collaborators)
                .Include(u => u.User)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets the embedded projects asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<EmbeddedProject>> GetEmbeddedProjectsAsync()
        {
            return await GetDbSet<EmbeddedProject>()
                .Include(p => p.Project)
                .ThenInclude(p => p.Collaborators)
                .Include(u => u.User)
                .ToListAsync();
        }

        /// <summary>
        /// Determines whether [is non existing unique identifier] [the specified unique identifier].
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>
        ///   <c>true</c> if [is non existing unique identifier] [the specified unique identifier]; otherwise, <c>false</c>.
        /// </returns>
        public async Task<bool> IsNonExistingGuidAsync(Guid guid)
        {
            return await GetDbSet<EmbeddedProject>()
                .Where(e => e.Guid == guid)
                .FirstOrDefaultAsync() == null;
        }
    }

}
