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

using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Models;
using Serilog;
using Services.Services;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Quickstart
{

    /// <summary>
    ///     The Custom profile service.
    /// </summary>
    /// <seealso cref="IdentityServer4.Services.IProfileService" />
    public class ProfileService : IProfileService
    {

        //services
        private readonly IIdentityUserService userService;

        public ProfileService(IIdentityUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        ///     This method is called whenever claims about the user are requested (e.g. during token creation or via the userinfo
        ///     endpoint)
        /// </summary>
        /// <param name="context">The request context.</param>
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            try
            {
                Claim userId = context.Subject.Claims.FirstOrDefault(x => x.Type == "sub");

                if(!string.IsNullOrEmpty(userId?.Value))
                {
                    //get user from db (find user by user id)
                    IdentityUser user = await userService.FindAsync(userId.Value);

                    // issue the claims for the user
                    if(user != null)
                    {
                        Claim[] claims = GetUserClaims(user);

                        context.IssuedClaims = claims.Where(x => context.RequestedClaimTypes.Contains(x.Type))
                                                     .ToList();
                    }
                }
            } catch(Exception ex)
            {
                Log.Logger.Error("The exception stacktrace: {0}", ex.StackTrace);
            }
        }

        /// <summary>
        ///     This method gets called whenever identity server needs to determine if the user is valid or active (e.g. if the
        ///     user's account has been deactivated since they logged in).
        ///     (e.g. during token issuance or validation).
        /// </summary>
        /// <param name="context">The request context.</param>
        public async Task IsActiveAsync(IsActiveContext context)
        {
            try
            {
                Claim userId = context.Subject.Claims.FirstOrDefault(x => x.Type == "sub");

                if(!string.IsNullOrEmpty(userId?.Value))
                {
                    IdentityUser user = await userService.FindAsync(userId.Value);

                    if(user != null)
                    {
                        if(user.IsActive)
                        {
                            context.IsActive = user.IsActive;
                        }
                    }
                }
            } catch(Exception ex)
            {
                Log.Logger.Error("The exception stacktrace: {0}", ex.StackTrace);
            }
        }

        /// <summary>
        ///     This function returns the claims that will be put in the /connect/userinfo.
        /// </summary>
        /// <param name="user">The current user object.</param>
        /// <returns>The list of created Claims.</returns>
        public static Claim[] GetUserClaims(IdentityUser user)
        {
            return new[]
                   {
                       new Claim("sub", user.SubjectId ?? ""), new Claim(JwtClaimTypes.Name,
                                                                         !string.IsNullOrEmpty(user.Firstname) &&
                                                                         !string.IsNullOrEmpty(user.Lastname)
                                                                             ? user.Firstname + " " + user.Lastname
                                                                             : ""),
                       new Claim(JwtClaimTypes.GivenName, user.Firstname ?? ""),
                       new Claim(JwtClaimTypes.FamilyName, user.Lastname ?? ""),
                       new Claim(JwtClaimTypes.Email, user.Email ?? "")
                   };
        }

    }

}
