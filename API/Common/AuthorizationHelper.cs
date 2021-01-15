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
using Services.Services;
using System.Threading.Tasks;

namespace API.Common
{
    /// <summary>
    /// The implementation for the authorization helper.
    /// </summary>
    public class AuthorizationHelper : IAuthorizationHelper
    {

        private readonly IUserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationHelper"/> class.
        /// </summary>
        /// <param name="userService">The user service for communicating with the logic layer.</param>
        public AuthorizationHelper(IUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// This method checks if a user has the correct scope to use the endpoint.
        /// This method checks for a normal scope and the data officer scope within the
        /// same institution.
        /// </summary>
        /// <param name="loggedInUser">The user model of the logged in user.</param>
        /// <param name="scope">The required scope for accessing this endpoint.</param>
        /// <param name="dataOfficerScope">The required scope for accessing this
        /// endpoint for data officers within the same institution.</param>
        /// <param name="propertyOfUserId">The id of the user owner of the property
        /// which the logged in user wants to access.</param>
        /// <returns>bool: true if the user is allowed, false if the user is not allowed.</returns>
        public async Task<bool> UserIsAllowed(User loggedInUser, string scope, string dataOfficerScope, int propertyOfUserId)
        {
            bool hasUserWriteScope = userService.UserHasScope(loggedInUser.IdentityId, scope);
            bool hasCorrectDataOfficerRights =
                await SameInstitutionAndInstitutionScope(loggedInUser, dataOfficerScope, propertyOfUserId);
            bool isAllowed = hasUserWriteScope || hasCorrectDataOfficerRights;
            return isAllowed;
        }

        /// <summary>
        /// This method checks if a user has the same institution, and both should not have null. It
        /// also checks if the user has the correct institution scope that allows changes in the
        /// same institution.
        /// </summary>
        /// <param name="loggedInUser">The user model of the logged in user.</param>
        /// <param name="institutionScope">The required scope for accessing this
        /// endpoint for data officers within the same institution.</param>
        /// <param name="propertyOfUserId">The id of the user owner of the property
        /// which the logged in user wants to access.</param>
        /// <returns>Bool: true if the user is allowed, false if the user is not allowed.</returns>
        public async Task<bool> SameInstitutionAndInstitutionScope(User loggedInUser, string institutionScope, int propertyOfUserId)
        {
            return userService.UserHasScope(loggedInUser.IdentityId, institutionScope) &&
                   await userService.HasSameInstitution(loggedInUser.Id, propertyOfUserId);
        }

    }

}
