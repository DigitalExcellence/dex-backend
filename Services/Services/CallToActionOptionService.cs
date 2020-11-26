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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Services
{

    public interface ICallToActionOptionService
    {

        /// <summary>
        /// This method gets all the call to action options asynchronous.
        /// </summary>
        /// <returns>This method returns a list of call to action options.</returns>
        Task<IEnumerable<CallToActionOption>> GetCallToActionOptionsAsync();

        /// <summary>
        /// This method gets all the call to action options with the specified type asynchronous.
        /// </summary>
        /// <returns>This method returns a list of call to action options with the specified type id.</returns>
        Task<IEnumerable<CallToActionOption>> GetCallToActionOptionsFromTypeAsync(int typeId);

        /// <summary>
        /// This method gets the call to action option with the specified id asynchronous.
        /// </summary>
        /// <returns>This method returns the found call to action option with the specified id.</returns>
        Task<CallToActionOption> GetCallToActionOptionByIdAsync(int id);

    }

    public class CallToActionOptionService : Service<CallToActionOption>, ICallToActionOptionService
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="EmbedService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public CallToActionOptionService(ICallToActionOptionRepository repository) : base(repository) { }

        protected new ICallToActionOptionRepository Repository => (ICallToActionOptionRepository) base.Repository;

        public Task<IEnumerable<CallToActionOption>> GetCallToActionOptionsAsync()
        {
            return await Repository.GetCallToActionOptionsAsync();
        }

        public Task<IEnumerable<CallToActionOption>> GetCallToActionOptionsFromTypeAsync(int typeId)
        {
            return await Repository.GetCallToActionOptionsFromTypeAsync(typeId);
        }

        public Task<CallToActionOption> GetCallToActionOptionByIdAsync(int id)
        {
            return await Repository.GetCallToActionOptionByIdAsync(id);
        }

    }

}
