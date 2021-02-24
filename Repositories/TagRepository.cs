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
using Repositories.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories
{

    /// <summary>
    ///     This is the interface of the tag repository
    /// </summary>
    /// <seealso cref="IRepository{Tag}"/>
    public interface ITagRepository : IRepository<Tag> {

        /// <summary>
        ///     This interface method gets all tags asynchronously
        /// </summary>
        /// <returns></returns>
        Task<List<Tag>> GetAllAsync();

    }

    /// <summary>
    ///     This is the tag repository
    /// </summary>
    /// <seealso cref="Tag" />
    /// <seealso cref="ITagRepository" />
    public class TagRepository : Repository<Tag>, ITagRepository
    {

        /// <summary>
        ///     Initializes a new instance of the <see cref="TagRepository" /> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public TagRepository(DbContext dbContext) : base(dbContext) { }

        public Task<List<Tag>> GetAllAsync()
        {
            return GetDbSet<Tag>()
                   .ToListAsync();
        }

    }

}
