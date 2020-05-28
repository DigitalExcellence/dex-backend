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
using Models.Defaults;
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
        /// Redacts the user.
        /// </summary>
        /// <param name="embeddedProject">The embedded project.</param>
        /// <returns></returns>
        private EmbeddedProject RedactUser(EmbeddedProject embeddedProject)
        {
            if(embeddedProject?.Project?.User?.IsPublic == false)
            {
                embeddedProject.Project.User.Email = Defaults.Privacy.RedactedEmail;
            }
            if(embeddedProject?.User?.IsPublic == false)
            {
                embeddedProject.User.Email = Defaults.Privacy.RedactedEmail;
            }

            return embeddedProject;
        }

        /// <summary>
        /// Redacts the user.
        /// </summary>
        /// <param name="embeddedProjects">The embedded projects.</param>
        /// <returns></returns>
        private List<EmbeddedProject> RedactUser(List<EmbeddedProject> embeddedProjects)
        {
            for(int i = 0; i < embeddedProjects.Count; i++)
            {
                embeddedProjects[i] = RedactUser(embeddedProjects[i]);
            }
            return embeddedProjects;
        }

        /// <summary>
        /// Finds the embeddedproject with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public override async Task<EmbeddedProject> FindAsync(int id)
        {
            EmbeddedProject embeddedProject = await GetDbSet<EmbeddedProject>()
                                    .Where(s => s.Id == id)
                                    .Include(p => p.User)
                                    .SingleOrDefaultAsync();

            return RedactUser(embeddedProject);
        }
        /// <summary>
        /// Gets the embedded embeddedProject asynchronous.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        public async Task<EmbeddedProject> GetEmbeddedProjectAsync(Guid guid)
        {
            EmbeddedProject embeddedProject = await GetDbSet<EmbeddedProject>()
                .Where(e => e.Guid == guid)
                .Include(p => p.Project)
                .ThenInclude(p => p.Collaborators)
                .Include(u => u.User)
                .FirstOrDefaultAsync();

            return RedactUser(embeddedProject);
        }

        /// <summary>
        /// Gets the embedded embeddedProjects asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<EmbeddedProject>> GetEmbeddedProjectsAsync()
        {
            List<EmbeddedProject> embeddedProjects = await GetDbSet<EmbeddedProject>()
                .Include(u => u.User)
                .Include(p => p.Project).ThenInclude(p => p.Collaborators)
                .Include(p => p.Project).ThenInclude(p => p.User)
                .ToListAsync();
            return RedactUser(embeddedProjects);
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
