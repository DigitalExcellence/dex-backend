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

using Models;
using Repositories;
using Services.Base;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using static Models.Defaults.Defaults;

namespace Services.Services
{

    /// <summary>
    ///     This is the interface for the category service
    /// </summary>
    public interface ICategoryService : IService<Category>
    {

        /// <summary>
        /// This is the interface method to get all categories ansynchronous
        /// </summary>
        /// <returns>A list of categories</returns>
        Task<List<Category>> GetAllAsync();

    }

    /// <summary>
    ///     This is the category service
    /// </summary>
    public class CategoryService : Service<Category>, ICategoryService
    {

        /// <summary>
        ///     This is the category service constructor
        /// </summary>
        /// <param name="repository"></param>
        public CategoryService(ICategoryRepository repository) : base(repository) { }

        /// <summary>
        ///     Gets the repository
        /// </summary>
        protected new ICategoryRepository Repository => (ICategoryRepository) base.Repository;

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>A list of all categories.</returns>
        public Task<List<Category>> GetAllAsync()
        {
            return Repository.GetAllAsync();
        }

    }

}
