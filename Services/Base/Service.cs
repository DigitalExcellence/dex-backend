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

using Repositories.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Base
{
    /// <summary>
    ///     This is the abstract base class for the services
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class Service<TEntity> : IService<TEntity> where TEntity : class
    {
        /// <summary>
        ///     This is the protected constructor for the abstract class
        /// </summary>
        /// <param name="repository"></param>
        protected Service(IRepository<TEntity> repository)
        {
            Repository = repository;
        }

        /// <summary>
        ///     This is the repository
        /// </summary>
        protected IRepository<TEntity> Repository { get; }

        /// <summary>
        ///     This is the method for finding a single entity by the identifier.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The found entity</returns>
        public virtual async Task<TEntity> FindAsync(int id)
        {
            return await Repository.FindAsync(id).ConfigureAwait(false);
        }

        /// <summary>
        ///     This is the method for adding an entity.
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Add(TEntity entity)
        {
            Repository.Add(entity);
        }

        /// <summary>
        ///     This is the method adding an entity asynchronous
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task AddAsync(TEntity entity)
        {
            await Repository.AddAsync(entity).ConfigureAwait(false);
        }

        /// <summary>
        ///     This is the method for adding multiple entities at once
        /// </summary>
        /// <param name="entities"></param>
        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            Repository.AddRange(entities);
        }

        /// <summary>
        ///     This is the method to update an entity
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(TEntity entity)
        {
            Repository.Update(entity);
        }

        /// <summary>
        ///     This is the method to remove an entity
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Remove(TEntity entity)
        {
            Repository.Remove(entity);
        }

        /// <summary>
        ///     This is the method to remove an entity asynchronous
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Task RemoveAsync(int id)
        {
            return Repository.RemoveAsync(id);
        }

        /// <summary>
        ///     This is the method to get all entities
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            return await Repository.GetAll().ConfigureAwait(false);
        }

        /// <summary>
        ///     This is the method to save changes that were made
        /// </summary>
        public virtual void Save()
        {
            Repository.Save();
        }
    }
}
