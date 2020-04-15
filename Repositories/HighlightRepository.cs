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

        Task<List<Highlight>> GetHighlightsAsync(bool onlyTheHightlighted);

    }
    public class HighlightRepository : Repository<Highlight>, IHighlightRepository
    {
        public HighlightRepository(DbContext dbContext) : base(dbContext) { }

        public async Task<List<Highlight>> GetHighlightsAsync(bool onlyTheHightlighted)
        {
            return await GetDbSet<Highlight>()
                         .Where(h => h.IsHighlighted == onlyTheHightlighted)
                         .Include(p => p.Project)
                         .ToListAsync();
        }

    }
}
