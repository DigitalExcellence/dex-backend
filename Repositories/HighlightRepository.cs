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

    /// <summary>
    ///     This is the highlight repository interface
    /// </summary>
    public interface IHighlightRepository : IRepository<Highlight>
    {

        /// <summary>
        ///     This interface method gets all highlights asynchronous
        /// </summary>
        /// <returns>List of highlights</returns>
        Task<List<Highlight>> GetHighlightsAsync();

        /// <summary>
        ///     This interface method gets all highlights by project id asynchronous.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>List of highlights</returns>
        Task<List<Highlight>> GetHighlightsByProjectIdAsync(int projectId);

    }

    /// <summary>
    ///     This is the highlight repository implementation
    /// </summary>
    public class HighlightRepository : Repository<Highlight>, IHighlightRepository
    {

        /// <summary>
        ///     This is the highlight repository constructor
        /// </summary>
        /// <param name="dbContext"></param>
        public HighlightRepository(DbContext dbContext) : base(dbContext) { }

        /// <summary>
        ///     find highlight by id.
        /// </summary>
        /// <param name="id">The highlight id.</param>
        /// <returns>the found highlight.</returns>
        public override async Task<Highlight> FindAsync(int id)
        {
            Highlight project = await GetDbSet<Highlight>()
                                      .Where(s => s.Id == id)
                                      .Include(p => p.Project)
                                      .Include(p => p.Project.ProjectIcon)
                                      .SingleOrDefaultAsync();

            return RedactUser(project);
        }

        /// <summary>
        ///     Gets the highlights asynchronous.
        /// </summary>
        /// <returns>list of all the highlights.</returns>
        public async Task<List<Highlight>> GetHighlightsAsync()
        {
            List<Highlight> highlights = await GetDbSet<Highlight>()
                                               .Where(s => s.StartDate <= DateTime.Now || s.StartDate == null)
                                               .Where(s => s.EndDate >= DateTime.Now || s.EndDate == null)
                                               .Include(p => p.Project)
                                               .Include(p => p.Project.ProjectIcon)
                                               .ToListAsync();
            return RedactUser(highlights);
        }

        /// <summary>
        ///     This method gets all highlights by project id asynchronous.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>List of highlights</returns>
        public async Task<List<Highlight>> GetHighlightsByProjectIdAsync(int projectId)
        {
            return await GetDbSet<Highlight>()
                         .Where(s => s.ProjectId == projectId)
                         .Include(p => p.Project)
                         .Include(p => p.Project.ProjectIcon)
                         .ToListAsync();
        }

        /// <summary>
        ///     Redacts the user email property of the highlighted project.
        /// </summary>
        /// <param name="highlight">The highlight object.</param>
        /// <returns>redacted highlight object.</returns>
        private Highlight RedactUser(Highlight highlight)
        {
            if(highlight == null) return highlight;

            if(highlight.Project?.User?.IsPublic == false)
            {
                highlight.Project.User.Email = Defaults.Privacy.RedactedEmail;
            }
            return highlight;
        }

        /// <summary>
        ///     Redacts the user email property of the highlighted project.
        /// </summary>
        /// <param name="highlights">List of highlight objects.</param>
        /// <returns>redacted highlight objects.</returns>
        private List<Highlight> RedactUser(List<Highlight> highlights)
        {
            for(int i = 0; i < highlights.Count; i++)
            {
                highlights[i] = RedactUser(highlights[i]);
            }
            return highlights;
        }

    }

}
