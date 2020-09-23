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

    public interface IHighlightService : IService<Highlight>
    {

        Task<List<Highlight>> GetHighlightsAsync();
        Task<List<Highlight>> GetHighlightsByProjectIdAsync(int projectId);

    }

    public class HighlightService : Service<Highlight>, IHighlightService
    {

        public HighlightService(IHighlightRepository repository) : base(repository) { }

        protected new IHighlightRepository Repository => (IHighlightRepository) base.Repository;

        public async Task<List<Highlight>> GetHighlightsAsync()
        {
            return await Repository.GetHighlightsAsync();
        }

        public async Task<List<Highlight>> GetHighlightsByProjectIdAsync(int projectId)
        {
            return await Repository.GetHighlightsByProjectIdAsync(projectId).ConfigureAwait(false);
        }
    }

}
