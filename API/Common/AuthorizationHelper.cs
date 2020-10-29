using Models;
using Services.Services;
using System.Threading.Tasks;

namespace API.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthorizationHelper : IAuthorizationHelper
    {

        private readonly IUserService userService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userService"></param>
        public AuthorizationHelper(IUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggedInUser"></param>
        /// <param name="scope"></param>
        /// <param name="dataOfficerScope"></param>
        /// <param name="propertyOfUserId"></param>
        /// <returns></returns>
        public async Task<bool> UserIsAllowed(User loggedInUser, string scope, string dataOfficerScope, int propertyOfUserId)
        {
            bool hasUserWriteScope = userService.UserHasScope(loggedInUser.IdentityId, scope);
            bool hasCorrectDataOfficerRights =
                userService.UserHasScope(loggedInUser.IdentityId, dataOfficerScope) &&
                await userService.HasSameInstitution(loggedInUser.Id, propertyOfUserId);
            bool isAllowed = hasUserWriteScope || hasCorrectDataOfficerRights;
            return isAllowed;
        }

    }

}
