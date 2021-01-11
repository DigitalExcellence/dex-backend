using Models;
using Repositories;
using Services.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public interface IPortfolioService : IService<Portfolio>
    {
        /// <summary>
        /// Add a portfolio item
        /// </summary>
        /// <param name="portfolioItem"></param>
        /// <returns></returns>
        Task<PortfolioItem> AddPortfolioItem(PortfolioItem portfolioItem);

        /// <summary>
        /// Update a portfolio item
        /// </summary>
        /// <param name="portfolioItem"></param>
        /// <returns></returns>
        Task<PortfolioItem> UpdatePortfolioItem(PortfolioItem portfolioItem);

        /// <summary>
        /// Delete a portfolio item
        /// </summary>
        /// <param name="portfolioItem"></param>
        /// <returns></returns>
        Task<PortfolioItem> DeletePortfolioItem(PortfolioItem portfolioItem);
    }

    public class PortfolioService : Service<Portfolio>, IPortfolioService
    {
        public PortfolioService(IPortfolioRepository repository) : base(repository) { }

        protected new IPortfolioRepository Repository => (IPortfolioRepository) base.Repository;

        Task<PortfolioItem> IPortfolioService.AddPortfolioItem(PortfolioItem portfolioItem)
        {
            throw new NotImplementedException();
        }

        Task<PortfolioItem> IPortfolioService.DeletePortfolioItem(PortfolioItem portfolioItem)
        {
            throw new NotImplementedException();
        }

        Task<PortfolioItem> IPortfolioService.UpdatePortfolioItem(PortfolioItem portfolioItem)
        {
            throw new NotImplementedException();
        }
    }
}
