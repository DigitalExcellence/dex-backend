using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Base
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected DbContext DbContext { get; }
        protected DbSet<TEntity> DbSet => DbContext.Set<TEntity>();

        protected DbSet<T> GetDbSet<T>() where T : class
        {
            return DbContext.Set<T>();
        }

        public Repository(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        public virtual async Task<TEntity> FindAsync(int id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual void Add(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            DbSet.AddRange(entities);
        }

        public virtual void Update(TEntity entity)
        {
            DbSet.Attach(entity);
            DbSet.Update(entity);
        }

        public virtual async Task RemoveAsync(int id)
        {
            TEntity entity = await FindAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Id: {id} not found");
            }

            Remove(entity);
        }

        public virtual void Remove(TEntity entity)
        {
            if (DbContext.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }

            DbSet.Remove(entity);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public virtual void Save()
        {
            DbContext.SaveChanges();
        }
    }
}