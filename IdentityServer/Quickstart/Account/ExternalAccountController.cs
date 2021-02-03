using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Services;

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
