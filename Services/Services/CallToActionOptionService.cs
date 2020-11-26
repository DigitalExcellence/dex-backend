using Models;
using Repositories;
using Services.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Services
{

    public interface ICallToActionOptionService
    {

        Task<IEnumerable<CallToActionOption>> GetCallToActionOptionsAsync();

        Task<IEnumerable<CallToActionOption>> GetCallToActionOptionsFromTypeAsync(int typeId);

        Task<CallToActionOption> GetCallToActionOptionByIdAsync(int id);

    }

    public class CallToActionOptionService : Service<CallToActionOption>, ICallToActionOptionService
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="EmbedService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public CallToActionOptionService(ICallToActionOptionRepository repository) : base(repository) { }

        public Task<IEnumerable<CallToActionOption>> GetCallToActionOptionsAsync()
        {
            throw new NotImplementedException();
        }

    }

}
