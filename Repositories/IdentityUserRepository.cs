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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Repositories
{
    /// <summary>
    /// The IdentityUser Repository
    /// </summary>
    /// <seealso cref="Repositories.Base.IRepository{Models.IdentityUser}" />
    public interface IIdentityUserRepository: IRepository<IdentityUser>
    {
        /// <summary>
        /// Gets the IdentityUser with the specified subjectIdentifier.
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <returns>The retrieved IdentityUser object.</returns>
        Task<IdentityUser> FindAsync(string subjectId);

        /// <summary>
        /// Gets the IdentityUser by its username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>The retrieved IdentityUser</returns>
        Task<IdentityUser> FindByUsername(string username);

        /// <summary>
        /// Gets the IdentityUser by its external provider data.
        /// </summary>
        /// <param name="provider">The provider identifier.</param>
        /// <param name="providerUserId">The users unique identifier with that provider.</param>
        /// <returns>The retrieved IdentityUser</returns>
        Task<IdentityUser> FindByExternalProvider(string provider, string providerUserId);

        /// <summary>
        /// Creates a new IdentityUser from claims.
        /// </summary>
        /// <param name="provider">The provider identifier.</param>
        /// <param name="providerUserId">The users unique identifier with that provider.</param>
        /// <param name="claimsList">The claims list.</param>
        /// <returns>The retrieved IdentityUser</returns>
        Task<IdentityUser> AutoProvisionUser(string provider, string providerUserId, List<Claim> claimsList);

        Task<IdentityUser> FindBySubjectId(string subjectId);

    }

    /// <summary>
    /// The IdentityUser Repository
    /// </summary>
    /// <seealso cref="Repositories.Base.Repository{Models.IdentityUser}" />
    /// <seealso cref="Repositories.IIdentityUserRepository" />
    public class IdentityUserRepository : Repository<IdentityUser>, IIdentityUserRepository
    {

        public IdentityUserRepository(DbContext dbContext) : base(dbContext) { }

        public override void Add(IdentityUser entity)
        {
            entity.SubjectId = Guid.NewGuid().ToString();
            base.Add(entity);
        }

        /// <summary>
        /// Gets the IdentityUser with the specified subjectIdentifier.
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <returns>
        /// The retrieved IdentityUser object.
        /// </returns>
        public async Task<IdentityUser> FindAsync(string subjectId)
        {
            return await GetDbSet<IdentityUser>()
                         .Where(u => u.SubjectId == subjectId)
                         .SingleOrDefaultAsync();
        }

        /// <summary>
        /// Gets the IdentityUser by its username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>
        /// The retrieved IdentityUser
        /// </returns>
        public async Task<IdentityUser> FindByUsername(string username)
        {
            return await GetDbSet<IdentityUser>()
                         .Where(u => u.Username == username)
                         .SingleOrDefaultAsync();
        }

        /// <summary>
        /// Gets the IdentityUser by its external provider data.
        /// </summary>
        /// <param name="provider">The provider identifier.</param>
        /// <param name="providerUserId">The users unique identifier with that provider.</param>
        /// <returns>
        /// The retrieved IdentityUser
        /// </returns>
        public async Task<IdentityUser> FindByExternalProvider(string provider, string providerUserId)
        {
            return await GetDbSet<IdentityUser>()
                         .Where(u => u.ProviderId == provider && u.ExternalSubjectId == providerUserId)
                         .SingleOrDefaultAsync();
        }

        /// <summary>
        /// Creates a new IdentityUser from claims.
        /// </summary>
        /// <param name="provider">The provider identifier.</param>
        /// <param name="providerUserId">The users unique identifier with that provider.</param>
        /// <param name="claimsList">The claims list.</param>
        /// <returns>
        /// The retrieved IdentityUser
        /// </returns>
        public async Task<IdentityUser> AutoProvisionUser(string provider, string providerUserId, List<Claim> claimsList)
        {
            IdentityUser user = new IdentityUser()
                                {
                                    ProviderId = provider,
                                    ExternalSubjectId = providerUserId,
                                };
            GetDbSet<IdentityUser>().Add(user);
            DbContext.SaveChanges();
            return await GetDbSet<IdentityUser>()
                .Where(i => i.ProviderId == provider && i.ExternalSubjectId == providerUserId).SingleOrDefaultAsync();
        }

        public async Task<IdentityUser> FindBySubjectId(string subjectId)
        {
            return await GetDbSet<IdentityUser>()
                       .Where(i => i.SubjectId == subjectId).SingleOrDefaultAsync();
        }

    }
}
