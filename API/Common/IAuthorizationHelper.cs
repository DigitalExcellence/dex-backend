using Models;
using System.Threading.Tasks;

namespace API.Common
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAuthorizationHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggedInUser"></param>
        /// <param name="scope"></param>
        /// <param name="dataOfficerScope"></param>
        /// <param name="propertyOfUserId"></param>
        /// <returns></returns>
        public Task<bool> UserIsAllowed(User loggedInUser,
                                              string scope,
                                              string dataOfficerScope,
                                              int propertyOfUserId);

    }

}
