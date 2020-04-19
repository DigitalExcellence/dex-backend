using Models;
using Repositories;
using Services.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Services
{

    public interface IHighlightService : IService<Highlight>
    {

        Task<List<Highlight>> GetHighlightsAsync();

    }

    public class HighlightService : Service<Highlight>, IHighlightService
    {

        public HighlightService(IHighlightRepository repository) : base(repository) { }

        protected new IHighlightRepository Repository => (IHighlightRepository) base.Repository;

        public async Task<List<Highlight>> GetHighlightsAsync()
        {
            return await Repository.GetHighlightsAsync();
        }

    }

}
