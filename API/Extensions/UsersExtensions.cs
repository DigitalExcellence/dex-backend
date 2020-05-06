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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Models.Defaults;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace API.Extensions
{

    internal static class UsersExtensions
    {

        /// <summary>
        ///     Gets the student identifier asynchronous.
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal.</param>
        /// <param name="actionContext">The action context.</param>
        /// <returns></returns>
        /// <exception cref="Exception">The back-end header isn't added!</exception>
        /// <exception cref="NotSupportedException">The jwt doesn't have a sub</exception>
        /// <exception cref="System.Exception">The back-end header isn't added!</exception>
        public static string GetStudentId(this ClaimsPrincipal claimsPrincipal, HttpContext actionContext)
        {
            string studentId;

            if(claimsPrincipal.Identities.Any(i => !i.IsAuthenticated))
            {
                throw new Exception("User is not authenticated!");
            }

            if(claimsPrincipal.IsInRole(Defaults.Roles.BackendApplication))
            {
                string studentIdHeader = actionContext.Request.Headers.SingleOrDefault(h => h.Key == "StudentId")
                                                      .Value
                                                      .FirstOrDefault();

                if(string.IsNullOrWhiteSpace(studentIdHeader))
                {
                    throw new Exception("The back-end header isn't added!");
                }

                studentId = studentIdHeader;
            } else
            {
                string sub = claimsPrincipal.Claims.FirstOrDefault(c => c.Type.Equals("sub"))
                                            .Value;
                if(sub == null)
                {
                    throw new NotSupportedException("The jwt doesn't have a sub");
                }

                return sub;
            }

            return studentId;
        }


        public static string GetUserInformation(this HttpContext actionContext)
        {
            var x = actionContext.Request.Headers.GetCommaSeparatedValues("Authorization");
            return "";
        }

        /// <summary>
        ///     Gets the name of the student.
        /// </summary>
        /// <param name="iUserPrincipal">The i user principal.</param>
        /// <returns></returns>
        public static string GetStudentName(this IPrincipal iUserPrincipal)
        {
            return iUserPrincipal.Identity.Name;

            //return Student.ConvertStudentPcnToCompatibleVersion(iUserPrincipal.Identity.Name);
        }

    }

}
