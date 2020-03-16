using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Repositories.Base;

namespace Services.Base
{
    public abstract class Service<TEntity> : IService<TEntity> where TEntity : class
    {
        protected IRepository<TEntity> Repository { get; }

        protected Service(IRepository<TEntity> repository)
        {
            Repository = repository;
        }

        public virtual async Task<TEntity> FindAsync(int id)
        {
            return await Repository.FindAsync(id);
        }

        public virtual void Add(TEntity entity)
        {
            Repository.Add(entity);
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            Repository.AddRange(entities);
        }

        public virtual void Update(TEntity entity)
        {
            Repository.Update(entity);
        }

        public virtual void Remove(TEntity entity)
        {
            Repository.Remove(entity);
        }

        public virtual Task RemoveAsync(int id)
        {
            return Repository.RemoveAsync(id);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            return await Repository.GetAll();
        }

        public virtual void Save()
        {
            Repository.Save();
        }
    }
}