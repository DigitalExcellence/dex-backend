/*
* Digital Excellence Copyright (C) 2020 Brend Smits
*
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU Lesser General Public License as published
* by the Free Software Foundation version 3 of the License.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty
* of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
* See the GNU Lesser General Public License for more details.
*
* You can find a copy of the GNU Lesser General Public License
* along with this program, in the LICENSE.md file in the root project directory.
* If not, see https://www.gnu.org/licenses/lgpl-3.0.txt
*/

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Repositories.Base
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected Repository(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        protected DbContext DbContext { get; }

        protected DbSet<TEntity> DbSet => DbContext.Set<TEntity>();

        public virtual async Task<TEntity> FindAsync(int id)
        {
            return await DbSet.FindAsync(id).ConfigureAwait(false);
        }

        public virtual TEntity UpdateCreatedField(TEntity entity)
        {
            if(entity == null)
            {
                return entity;

            }
            PropertyInfo createdProperty = entity.GetType().GetProperty("Created", BindingFlags.Public | BindingFlags.Instance);
            if(createdProperty != null && createdProperty.CanWrite)
            {
                createdProperty.SetValue(entity, DateTime.Now, null);
            }
            return entity;
        }

        public virtual TEntity UpdateUpdatedField(TEntity entity)
        {
            if(entity == null)
            {
                return entity;

            }
            PropertyInfo updatedProperty = entity.GetType().GetProperty("Updated", BindingFlags.Public | BindingFlags.Instance);
            if(updatedProperty != null && updatedProperty.CanWrite)
            {
                updatedProperty.SetValue(entity, DateTime.Now, null);
            }
            return entity;
        }

        public virtual void Add(TEntity entity)
        {
            entity = UpdateCreatedField(entity);
            entity = UpdateUpdatedField(entity);
            
            DbSet.Add(entity);
        }
        public virtual async Task AddAsync(TEntity entity)
        {
            entity = UpdateCreatedField(entity);
            entity = UpdateUpdatedField(entity);

            await DbSet.AddAsync(entity).ConfigureAwait(false);
        }
        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            List<TEntity> entityList = entities.ToList();
            for(int i = 0; i < entityList.Count; i++)
            {
                entityList[i] = UpdateCreatedField(entityList[i]);
                entityList[i] = UpdateUpdatedField(entityList[i]);
            }
            DbSet.AddRange(entityList);
        }

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            List<TEntity> entityList = entities.ToList();
            for(int i = 0; i < entityList.Count; i++)
            {
                entityList[i] = UpdateCreatedField(entityList[i]);
                entityList[i] = UpdateUpdatedField(entityList[i]);
            }
            await DbSet.AddRangeAsync(entityList).ConfigureAwait(false);
        }

        public virtual void Update(TEntity entity)
        {
            entity = UpdateUpdatedField(entity);

            DbSet.Attach(entity);
            DbSet.Update(entity);
        }

        public virtual async Task RemoveAsync(int id)
        {
            TEntity entity = await FindAsync(id).ConfigureAwait(false);
            if(entity == null)
            {
                throw new KeyNotFoundException($"Id: {id} not found");
            }

            Remove(entity);
        }

        public virtual void Remove(TEntity entity)
        {
            if(DbContext.Entry(entity)
                        .State ==
               EntityState.Detached)
            {
                DbSet.Attach(entity);
            }

            DbSet.Remove(entity);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            return await DbSet.ToListAsync().ConfigureAwait(false);
        }

        public virtual void Save()
        {
            DbContext.SaveChanges();
        }

        protected DbSet<T> GetDbSet<T>() where T : class
        {
            return DbContext.Set<T>();
        }
    }
}
