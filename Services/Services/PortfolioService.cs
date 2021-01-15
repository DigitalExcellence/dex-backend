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

    }

    public class PortfolioService : Service<Portfolio>, IPortfolioService
    {
        public PortfolioService(IPortfolioRepository repository) : base(repository) { }

        protected new IPortfolioRepository Repository => (IPortfolioRepository) base.Repository;
    }
}
