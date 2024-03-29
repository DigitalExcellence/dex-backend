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
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services
{
    public interface IProjectTagService: IService<ProjectTag>
    {
        /// <summary>
        ///     Clear tags by given project
        /// </summary>
        Task ClearProjectTags(Project project);
    }


    /// <summary>
    ///     This is the tag service
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class ProjectTagService : Service<ProjectTag>, IProjectTagService
    {
        private readonly IProjectTagRepository repository;
        /// <summary>
        ///     This is the tag service constructor
        /// </summary>
        /// <param name="repository"></param>
        public ProjectTagService(IProjectTagRepository repository) : base(repository)
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
        public virtual async Task<ProjectTag> FindAsync(int id)
        {
            return await Repository.FindAsync(id)
                                   .ConfigureAwait(false);
        }


        /// <summary>
        ///     This is the method for adding an entity.
        /// </summary>
        /// <param name="entity"></param>
        public virtual void AddToProject(ProjectTag entity)
        {
            Repository.Add(entity);
        }


        /// <summary>
        ///     This is the method adding an entity asynchronous
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task AddAsync(ProjectTag entity)
        {
                await Repository.AddAsync(entity)
                                .ConfigureAwait(false);
        }

        /// <summary>
        ///     This is the method for adding multiple entities at once
        /// </summary>
        /// <param name="entities"></param>
        public virtual void AddRange(IEnumerable<ProjectTag> entities)
        {
            Repository.AddRange(entities);
        }

        /// <summary>
        ///     This is the method for adding multiple entities at once asynchronous.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual async Task AddRangeAsync(IEnumerable<ProjectTag> entities)
        {
            await Repository.AddRangeAsync(entities);
        }

        /// <summary>
        ///     This is the method to update an entity
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(ProjectTag entity)
        {
            Repository.Update(entity);
        }

        /// <summary>
        ///     This methods clears all tags
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public async Task ClearProjectTags(Project project)
        {
            IEnumerable<int> currentProjectTagIds = (await repository.GetProjectTags(project.Id)).Select(t => t.Id);
            await Repository.RemoveRangeAsync(currentProjectTagIds);
            Repository.Save();
        }

        /// <summary>
        ///     This is the method to remove an entity
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Remove(ProjectTag entity)
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
        public virtual void RemoveRange(IEnumerable<ProjectTag> entities)
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
        public virtual async Task<IEnumerable<ProjectTag>> GetAll()
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
