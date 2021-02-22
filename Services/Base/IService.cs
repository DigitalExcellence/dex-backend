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

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Base
{
    /// <summary>
    ///     This is the base service class
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IService<TEntity> where TEntity : class
    {

        /// <summary>
        ///     This is the interface method for finding a single entity by the identifier.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>entity</returns>
        Task<TEntity> FindAsync(int id);

        /// <summary>
        ///     This is the interface method for adding an entity.
        /// </summary>
        /// <param name="entity"></param>
        void Add(TEntity entity);

        /// <summary>
        ///     This is the interface method adding an entity asynchronous
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task AddAsync(TEntity entity);

        /// <summary>
        ///     This is the interface method for adding multiple entities at once
        /// </summary>
        /// <param name="entities"></param>
        void AddRange(IEnumerable<TEntity> entities);
        Task AddRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        ///     This is the interface method to update an entity
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);

        /// <summary>
        ///     This is the interface method to remove an entity asynchronous
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task RemoveAsync(int id);

        /// <summary>
        ///     This is the interface method to remove an entity
        /// </summary>
        /// <param name="entity"></param>
        void Remove(TEntity entity);

        /// <summary>
        ///     This is the interface method which removes a range of entities.
        /// </summary>
        /// <param name="entities"></param>
        void RemoveRange(IEnumerable<TEntity> entities);

        /// <summary>
        ///     This is the interface method which removes an entity asynchronous.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task RemoveRangeAsync(IEnumerable<int> ids);

        /// <summary>
        ///     This is the interface method to get all entities
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAll();

        /// <summary>
        ///     This is the interface method to save changes that were made
        /// </summary>
        void Save();

    }

}
