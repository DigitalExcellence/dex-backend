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
using System.Linq;
using System.Threading.Tasks;

namespace Repositories
{

    public interface ICallToActionOptionRepository : IRepository<CallToActionOption>
    {
        /// <summary>
        /// This method gets all the call to action options with the specified type asynchronous.
        /// </summary>
        /// <returns>This method returns a list of call to action options with the specified type id.</returns>
        Task<IEnumerable<CallToActionOption>> GetCallToActionOptionsFromTypeAsync(string typeName);

        /// <summary>
        /// This method gets all the call to action options with the specified value asynchronous.
        /// </summary>
        /// <returns>This method returns a list of call to action options with the specified value id.</returns>
        Task<IEnumerable<CallToActionOption>> GetCallToActionOptionFromValueAsync(string value);

    }

    public class CallToActionOptionRepository : Repository<CallToActionOption>, ICallToActionOptionRepository
    {

        public CallToActionOptionRepository(DbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<CallToActionOption>> GetCallToActionOptionsFromTypeAsync(string typeName)
        {
            return await GetDbSet<CallToActionOption>()
                         .Where(o => o.Type == typeName)
                         .ToListAsync();
        }

        public async Task<IEnumerable<CallToActionOption>> GetCallToActionOptionFromValueAsync(string value)
        {
            return await GetDbSet<CallToActionOption>()
                         .Where(o => o.Value == value)
                         .ToListAsync();
        }

    }

}
