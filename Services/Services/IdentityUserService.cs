using Models;
using Repositories;
using Services.Base;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    /// <summary>
    /// The IdentityUser service.
    /// </summary>
    /// <seealso cref="Services.Base.IService{Models.IdentityUser}" />
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

        Task<IdentityUser> FindBySubjectId(string subjectId);

    }

    /// <summary>
    /// The IdentityUser Service.
    /// </summary>
    /// <seealso cref="Services.Base.Service{Models.IdentityUser}" />
    /// <seealso cref="Services.Services.IIdentityUserService" />
    public class IdentityUserService : Service<IdentityUser>, IIdentityUserService
    {

        public IdentityUserService(IIdentityUserRepository repository) : base(repository) { }

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

        public async Task<IdentityUser> FindBySubjectId(string subjectId)
        {
            return await Repository.FindBySubjectId(subjectId);
        }

    }
}
