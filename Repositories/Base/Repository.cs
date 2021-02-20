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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Repositories.Base
{

    /// <summary>
    ///     This is the abstract base class of the repositories
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        ///     This is the protected constructor of the abstract base class
        /// </summary>
        /// <param name="dbContext"></param>
        protected Repository(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        /// <summary>
        ///     Gets the database context
        /// </summary>
        protected DbContext DbContext { get; }

        /// <summary>
        ///     Sets the database context to the right context
        /// </summary>
        protected DbSet<TEntity> DbSet => DbContext.Set<TEntity>();

        /// <summary>
        ///     This method finds an entity by identifier asynchronous
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Entity</returns>
        public virtual async Task<TEntity> FindAsync(int id)
        {
            return await DbSet.FindAsync(id).ConfigureAwait(false);
        }

        /// <summary>
        ///     This method sets the created date
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Entity</returns>
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

        /// <summary>
        ///     This method sets / updates the datetime of the updated field
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Entity</returns>
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

        /// <summary>
        ///     This method adds the entity to the database
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Add(TEntity entity)
        {
            entity = UpdateCreatedField(entity);
            entity = UpdateUpdatedField(entity);

            DbSet.Add(entity);
        }

        /// <summary>
        ///     This method adds the entity to the database asynchronously
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task AddAsync(TEntity entity)
        {
            entity = UpdateCreatedField(entity);
            entity = UpdateUpdatedField(entity);

            await DbSet.AddAsync(entity).ConfigureAwait(false);
        }

        /// <summary>
        ///     This method adds multiple entities at once to the database
        /// </summary>
        /// <param name="entities"></param>
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

        /// <summary>
        ///     This method updates an entity which is already in the database.
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(TEntity entity)
        {
            entity = UpdateUpdatedField(entity);

            DbSet.Attach(entity);
            DbSet.Update(entity);
        }

        /// <summary>
        ///     This method removes an entity from the database by identifier asynchronously.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task RemoveAsync(int id)
        {
            TEntity entity = await FindAsync(id).ConfigureAwait(false);
            if(entity == null)
            {
                throw new KeyNotFoundException($"Id: {id} not found");
            }

            Remove(entity);
        }

        /// <summary>
        ///     This method removes an entity from the database
        /// </summary>
        /// <param name="entity"></param>
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

        /// <summary>
        ///     This method gets all entities from the database
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            return await DbSet.ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        ///     This method saves the changes that were made
        /// </summary>
        public virtual void Save()
        {
            DbContext.SaveChanges();
        }

        /// <summary>
        ///     This method gets the database set
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Database set of entity T</returns>
        protected DbSet<T> GetDbSet<T>() where T : class
        {
            return DbContext.Set<T>();
        }
    }
}
