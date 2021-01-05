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
    public abstract class Service<TEntity> : IService<TEntity> where TEntity : class
    {
        protected Service(IRepository<TEntity> repository)
        {
            Repository = repository;
        }

        protected IRepository<TEntity> Repository { get; }

        public virtual async Task<TEntity> FindAsync(int id)
        {
            return await Repository.FindAsync(id).ConfigureAwait(false);
        }

        public virtual void Add(TEntity entity)
        {
            Repository.Add(entity);
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            await Repository.AddAsync(entity).ConfigureAwait(false);
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            Repository.AddRange(entities);
        }

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await Repository.AddRangeAsync(entities);
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

        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            Repository.RemoveRange(entities);
        }

        public virtual Task RemoveRangeAsync(IEnumerable<int> ids)
        {
            return Repository.RemoveRangeAsync(ids);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            return await Repository.GetAll().ConfigureAwait(false);
        }

        public virtual void Save()
        {
            Repository.Save();
        }
    }
}
