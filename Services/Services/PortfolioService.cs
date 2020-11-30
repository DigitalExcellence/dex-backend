using Models;
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
        public override void Add(Portfolio entity)
        {
            base.Add(entity);
        }

        public override Task AddAsync(Portfolio entity)
        {
            return base.AddAsync(entity);
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
