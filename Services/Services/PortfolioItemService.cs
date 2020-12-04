using Models;
using Repositories;
using Services.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace Services.Services
{
    public interface IPortfolioItemService : IService<PortfolioItem>
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

    public class PortfolioItemService : Service<PortfolioItem>, IPortfolioItemService
    {
        public PortfolioItemService(IPortfolioItemRepository repository): base(repository) { }

        protected new IPortfolioItemRepository Repository => (IPortfolioItemRepository) base.Repository;

        public override void Add(PortfolioItem entity)
        {
            base.Add(entity);
        }

        public override Task AddAsync(PortfolioItem entity)
        {
            return base.AddAsync(entity);
        }

        public Task<PortfolioItem> AddPortfolioItem(PortfolioItem portfolioItem)
        {
            throw new NotImplementedException();
        }

        public override void AddRange(IEnumerable<PortfolioItem> entities)
        {
            base.AddRange(entities);
        }

        public Task<PortfolioItem> DeletePortfolioItem(PortfolioItem portfolioItem)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override Task<PortfolioItem> FindAsync(int id)
        {
            return base.FindAsync(id);
        }

        public override Task<IEnumerable<PortfolioItem>> GetAll()
        {
            return base.GetAll();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override void Remove(PortfolioItem entity)
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

        public override void Update(PortfolioItem entity)
        {
            base.Update(entity);
        }

        public Task<PortfolioItem> UpdatePortfolioItem(PortfolioItem portfolioItem)
        {
            throw new NotImplementedException();
        }
    }
}
