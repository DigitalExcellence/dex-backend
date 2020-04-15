using Models;
using Repositories;
using Repositories.Base;
using Services.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Services
{

    public interface IHighlightService : IService<Highlight>
    {
        Task<List<Highlight>> GetHighlightsAsync(bool onlyTheHightlighted);
    }
    public class HighlightService : Service<Highlight>, IHighlightService
    {
        protected new IHighlightRepository Repository => (IHighlightRepository) base.Repository;
        public HighlightService(IHighlightRepository repository) : base(repository) { }

        public async Task<List<Highlight>> GetHighlightsAsync(bool onlyTheHightlighted)
        {
            return await Repository.GetHighlightsAsync(onlyTheHightlighted);
        }

    }
}
