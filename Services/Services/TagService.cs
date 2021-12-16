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

namespace Services.Services
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
    public class TagService : Service<Tag>, ITagService
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
        ///     This is the method for finding a single entity by the name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The found entity</returns>
        public virtual async Task<Tag> FindByNameAsync(string name)
        {
            return await repository.FindByNameAsync(name).ConfigureAwait(false);
        }

        /// <summary>
        /// This is the method for finding a single entity by the name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The found entity</returns>
        public Tag FindByName(string name)
        {
            return repository.FindByName(name);
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
