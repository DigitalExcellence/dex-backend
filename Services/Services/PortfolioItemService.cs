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

    }

    public class PortfolioItemService : Service<PortfolioItem>, IPortfolioItemService
    {
        public PortfolioItemService(IPortfolioItemRepository repository): base(repository) { }

        protected new IPortfolioItemRepository Repository => (IPortfolioItemRepository) base.Repository;
    }
}
