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

        public override void Add(Portfolio entity)
        {
            base.Add(entity);
        }

        public override Task AddAsync(Portfolio entity)
        {
            return base.AddAsync(entity);
        }

        public Task<PortfolioItem> AddPortfolioItem(PortfolioItem portfolioItem)
        {
            throw new NotImplementedException();
        }
        public Task<PortfolioItem> UpdatePortfolioItem(PortfolioItem portfolioItem)
        {
            throw new NotImplementedException();
        }

        public Task<PortfolioItem> DeletePortfolioItem(PortfolioItem portfolioItem)
        {
            throw new NotImplementedException();
        }

        public override void AddRange(IEnumerable<Portfolio> entities)
        {
            base.AddRange(entities);
        }



        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override Task<Portfolio> FindAsync(int id)
        {
            return base.FindAsync(id);
        }

        public override Task<IEnumerable<Portfolio>> GetAll()
        {
            return base.GetAll();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override void Remove(Portfolio entity)
        {
            base.Remove(entity);
        }

        public override Task RemoveAsync(int id)
        {
            return base.RemoveAsync(id);
        }

        public override void Save()
        {
            base.Save();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override void Update(Portfolio entity)
        {
            base.Update(entity);
        }
    }
}
