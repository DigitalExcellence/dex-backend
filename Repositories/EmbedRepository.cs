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

        Task<EmbeddedProject> GetEmbeddedProjectAsync(Guid guid);
        Task<IEnumerable<EmbeddedProject>> GetEmbeddedProjectsAsync();
        Task<bool> IsNonExistingGuid(Guid guid);
    }

    public class EmbedRepository : Repository<EmbeddedProject>, IEmbedRepository
    {

        public EmbedRepository(DbContext dbContext) : base(dbContext) { }

        public async Task<EmbeddedProject> GetEmbeddedProjectAsync(Guid guid)
        {
            return await GetDbSet<EmbeddedProject>()
                .Where(e => e.Guid == guid)
                .Include(p => p.Project)
                .ThenInclude(p => p.Collaborators)
                .Include(u => u.User)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<EmbeddedProject>> GetEmbeddedProjectsAsync()
        {
            return await GetDbSet<EmbeddedProject>()
                .Include(p => p.Project)
                .ThenInclude(p => p.Collaborators)
                .Include(u => u.User)
                .ToListAsync();
        }

        public async Task<bool> IsNonExistingGuid(Guid guid)
        {
            return await GetDbSet<EmbeddedProject>()
                .Where(e => e.Guid == guid)
                .FirstOrDefaultAsync() == null;
        }
    }

}
