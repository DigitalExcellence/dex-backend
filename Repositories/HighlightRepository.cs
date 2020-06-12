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

    public interface IHighlightRepository : IRepository<Highlight>
    {

        Task<List<Highlight>> GetHighlightsAsync();
        Task<List<Highlight>> GetHighlightsByProjectIdAsync(int projectId);

    }

    public class HighlightRepository : Repository<Highlight>, IHighlightRepository
    {

        public HighlightRepository(DbContext dbContext) : base(dbContext) { }

        public async Task<List<Highlight>> GetHighlightsAsync()
        {
            return await GetDbSet<Highlight>()
                         .Where(s => s.StartDate <= DateTime.Now || s.StartDate == null)
                         .Where(s => s.EndDate >= DateTime.Now || s.EndDate == null)
                         .Include(p => p.Project)
                         .ToListAsync();
        }

        public async Task<List<Highlight>> GetHighlightsByProjectIdAsync(int projectId)
        {
            return await GetDbSet<Highlight>()
                         .Where(s => s.ProjectId == projectId)
                         .Include(p => p.Project)
                         .ToListAsync();
        }
    }

}
