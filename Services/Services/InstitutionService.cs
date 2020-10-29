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
using Repositories.Base;
using Services.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Services
{

    public interface IInstitutionService : IService<Institution>
    {

        /// <summary>
        /// This method gets the institution with the specified id asynchronous.
        /// </summary>
        /// <param name="id">The id of the institution used for searching the institution.</param>
        /// <returns>This method returns the found institution with the specified id.</returns>
        Task<IEnumerable<Institution>> GetInstitutionsAsync();

    }

    public class InstitutionService : Service<Institution>, IInstitutionService
    {

        public InstitutionService(IRepository<Institution> repository) : base(repository) { }

        protected new IInstitutionRepository Repository => (IInstitutionRepository) base.Repository;

        public async Task<IEnumerable<Institution>> GetInstitutionsAsync()
        {
            return await Repository.GetInstitutionsAsync();
        }

    }

}
