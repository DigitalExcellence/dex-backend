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

using Services.Base;
using Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Repositories;
using System.Threading.Tasks;

namespace Services.Services
{
    public interface ILinkedCollaboratorService : IService<CollaboratorLinkedUser>
    {
        Task<bool> AcceptCollaboratorAsync(string requestHash);
    }

    /// <summary>
    ///     This is the linked collaborator service
    /// </summary>
    public class LinkedCollaboratorService : Service<CollaboratorLinkedUser>, ILinkedCollaboratorService
    {
        /// <summary>
        ///     This is the linked collaborator service constructor
        /// </summary>
        /// <param name="repository"></param>
        public LinkedCollaboratorService(ILinkedCollaboratorRepository repository) : base(repository) { }

        /// <summary>
        ///     Gets the repository
        /// </summary>
        protected new ILinkedCollaboratorRepository Repository => (ILinkedCollaboratorRepository) base.Repository;

        /// <summary>
        ///     Link the collaborator request
        /// </summary>
        /// <param name="requestHash">The hash to confirm the linking process.</param>
        /// <exception cref="ArgumentNullException">Argument error</exception>
        /// <exception cref="KeyNotFoundException">Not Found in DB</exception>
        /// <exception cref="Exception">State error</exception>
        /// <returns>
        ///     boolean
        /// </returns>
        public async Task<bool> AcceptCollaboratorAsync(string requestHash)
        {
            if(requestHash == null)
                throw new ArgumentNullException("Null value passed in.", nameof(requestHash));

            CollaboratorLinkedUser linkedUser =(await base.GetAll()).Where(c => c.AcceptanceHash == requestHash).FirstOrDefault();

            if(linkedUser == null)
                throw new KeyNotFoundException("Request Hash not found in any record, or is not valid.");

            if(linkedUser.Status == LinkedUserStatus.ACCEPTED)
                throw new Exception("The request has already been accepted.");

            linkedUser.Status = LinkedUserStatus.ACCEPTED;

            Repository.Update(linkedUser);
            base.Save();

            return true;
        }
    }
}
