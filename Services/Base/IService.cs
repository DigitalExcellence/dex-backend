using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Base
{
    public interface IService<TEntity> where TEntity : class
    {
        Task<TEntity> FindAsync(int id);
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        Task RemoveAsync(int id);
        void Remove(TEntity entity);
        Task<IEnumerable<TEntity>> GetAll();
        void Save();
    }
}