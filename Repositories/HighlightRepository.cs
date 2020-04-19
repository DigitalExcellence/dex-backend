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

    }

}
