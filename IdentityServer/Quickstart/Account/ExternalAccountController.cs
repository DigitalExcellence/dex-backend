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

using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Services;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Quickstart.Account
{

    [Route("[controller]")]
    [ApiController]
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    public class ExternalAccountController : ControllerBase
    {

        private readonly IIdentityUserService identityUserService;

        public ExternalAccountController(IIdentityUserService identityUserService)
        {
            this.identityUserService = identityUserService;
        }

        [HttpPut]
        public async Task<IActionResult> ChangeCredentials()
        {
            string pass = Request.Headers.FirstOrDefault(x => x.Key == "password")
                                 .Value.FirstOrDefault();
            string email = Request.Headers.FirstOrDefault(x => x.Key == "email")
                                  .Value.FirstOrDefault();
            string subjectId = Request.Headers.FirstOrDefault(x => x.Key == "subjectId")
                                      .Value.FirstOrDefault();

            IdentityUser identityUser = await identityUserService.FindBySubjectId(subjectId);
            identityUser.Email = email;
            identityUser.Username = email;
            identityUser.Password = LoginHelper.GetHashPassword(pass);

            identityUserService.Update(identityUser);
            identityUserService.Save();

            return Ok();
        }

    }

}
