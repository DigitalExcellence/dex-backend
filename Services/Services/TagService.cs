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
using Models;
using Repositories;
using Repositories.Base;
using Services.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface ITagService: IService<Tag>
    {
        Task<Tag> FindByNameAsync(string name);
        Tag FindByName(string name);

    }


    /// <summary>
    ///     This is the tag service
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class TagService : Service<Tag>, ITagService
    {
        private readonly ITagRepository repository;
        /// <summary>
        ///     This is the tag service constructor
        /// </summary>
        /// <param name="repository"></param>
        public TagService(ITagRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        /// <summary>
        ///     Gets the database context
        /// </summary>
        protected DbContext DbContext { get; }

        /// <summary>
        ///     This is the method for finding a single entity by the identifier.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The found entity</returns>
        public virtual async Task<Tag> FindAsync(int id)
        {
            return await Repository.FindAsync(id)
                                   .ConfigureAwait(false);
        }

        /// <summary>
        ///     This is the method for finding a single entity by the name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The found entity</returns>
        public virtual async Task<Tag> FindByNameAsync(string name)
        {
            return await repository.FindByNameAsync(name).ConfigureAwait(false);
        }

        public Tag FindByName(string name)
        {
            return repository.FindByName(name);
        }

        /// <summary>
        ///     This is the method for adding an entity.
        /// </summary>
        /// <param name="entity"></param>
        public virtual void AddToProject(Tag entity)
        {
            Repository.Add(entity);
        }


        /// <summary>
        ///     This is the method adding an entity asynchronous
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task AddAsync(Tag entity)
        {
            await Repository.AddAsync(entity)
                            .ConfigureAwait(false);
        }

        /// <summary>
        ///     This is the method for adding multiple entities at once
        /// </summary>
        /// <param name="entities"></param>
        public virtual void AddRange(IEnumerable<Tag> entities)
        {
            Repository.AddRange(entities);
        }

        /// <summary>
        ///     This is the method for adding multiple entities at once asynchronous.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual async Task AddRangeAsync(IEnumerable<Tag> entities)
        {
            await Repository.AddRangeAsync(entities);
        }

        /// <summary>
        ///     This is the method to update an entity
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(Tag entity)
        {
            Repository.Update(entity);
        }

        /// <summary>
        ///     This is the method to remove an entity
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Remove(Tag entity)
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
        ///     This is the method to remove multiple entities at ones.
        /// </summary>
        /// <param name="entities"></param>
        public virtual void RemoveRange(IEnumerable<Tag> entities)
        {
            Repository.RemoveRange(entities);
        }

        /// <summary>
        ///     This is the method to remove multiple entities at ones asynchronous.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual Task RemoveRangeAsync(IEnumerable<int> ids)
        {
            return Repository.RemoveRangeAsync(ids);
        }

        /// <summary>
        ///     This is the method to get all entities
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IEnumerable<Tag>> GetAll()
        {
            return await Repository.GetAll()
                                   .ConfigureAwait(false);
        }

        /// <summary>
        ///     This is the method to save changes that were made
        /// </summary>
        public virtual void Save()
        {
            Repository.Save();
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
