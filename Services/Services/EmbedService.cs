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
using System.Threading.Tasks;

namespace Services.Services
{

    public interface IEmbedService : IService<EmbeddedProject>
    {
        Task<EmbeddedProject> FindAsync(Guid guid);
        Task<IEnumerable<EmbeddedProject>> GetEmbeddedProjectsAsync();
        Task<bool> IsNonExistingGuid(Guid guid);
    }

    public class EmbedService : Service<EmbeddedProject>, IEmbedService
    {
        public EmbedService(IEmbedRepository repository) : base(repository) { }

        protected new IEmbedRepository Repository => (IEmbedRepository) base.Repository;

        public async Task<EmbeddedProject> FindAsync(Guid guid)
        {
            return await Repository.GetEmbeddedProjectAsync(guid);
        }

        public async Task<IEnumerable<EmbeddedProject>> GetEmbeddedProjectsAsync()
        {
            return await Repository.GetEmbeddedProjectsAsync();
        }
        public async Task<bool> IsNonExistingGuid(Guid guid)
        {
            return await Repository.IsNonExistingGuid(guid);
        }
    }

}
