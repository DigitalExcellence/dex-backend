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
using System.Security.Claims;
using System.Threading.Tasks;

namespace Services.Services
{
    /// <summary>
    /// The IdentityUser service.
    /// </summary>
    /// <seealso cref="IdentityUser" />
    public interface IIdentityUserService : IService<IdentityUser>
    {
        /// <summary>
        /// Validates the users credentials..
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>true if the credentials are valid.</returns>
        Task<bool> ValidateCredentialsAsync(string username, string password);

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

        /// <summary>
        /// Creates a new IdentityUser from claims.
        /// </summary>
        /// <param name="user">The user object.</param>
        /// <returns>The retrieved IdentityUser</returns>
        Task<IdentityUser> AutoProvisionUser(IdentityUser user);

        /// <summary>
        /// Gets the IdentityUser with the specified subjectIdentifier.
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <returns>The retrieved IdentityUser</returns>
        Task<IdentityUser> FindAsync(string subjectId);

    }

    /// <summary>
    /// The IdentityUser Service.
    /// </summary>
    /// <seealso cref="IdentityUser" />
    /// <seealso cref="IIdentityUserService" />
    public class IdentityUserService : Service<IdentityUser>, IIdentityUserService
    {

        /// <summary>
        ///     This is the constructor of the identity user service
        /// </summary>
        /// <param name="repository"></param>
        public IdentityUserService(IIdentityUserRepository repository) : base(repository) { }

        /// <summary>
        ///     Gets the repository
        /// </summary>
        protected new IIdentityUserRepository Repository => (IIdentityUserRepository) base.Repository;

        /// <summary>
        /// Validates the users credentials.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        /// true if the credentials are valid.
        /// </returns>
        public async Task<bool> ValidateCredentialsAsync(string username, string password)
        {
            IdentityUser user = await Repository.FindByUsername(username);
            if(user?.Password == null)
            {
                return false;
            }
            return LoginHelper.IsValidPassword(password, user.Password);
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
            return await Repository.FindByUsername(username);
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
            return await Repository.FindByExternalProvider(provider, providerUserId);
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
            return await Repository.AutoProvisionUser(provider, providerUserId, claimsList);
        }

        /// <summary>
        /// Creates a new IdentityUser from claims.
        /// </summary>
        /// <param name="user">The user object.</param>
        /// <returns>
        /// The retrieved IdentityUser
        /// </returns>
        public async Task<IdentityUser> AutoProvisionUser(IdentityUser user)
        {
            Repository.Add(user);
            Repository.Save();
            return await Repository.FindByExternalProvider(user.ProviderId, user.ExternalSubjectId);
        }

        /// <summary>
        /// Gets the IdentityUser with the specified subjectIdentifier.
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <returns>
        /// The retrieved IdentityUser
        /// </returns>
        public Task<IdentityUser> FindAsync(string subjectId)
        {
            return Repository.FindAsync(subjectId);
        }
    }
}
