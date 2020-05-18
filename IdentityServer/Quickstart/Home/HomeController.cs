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

using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IdentityServer
{

    [SecurityHeaders]
    [AllowAnonymous]
    public class HomeController : Controller
    {

        private readonly IWebHostEnvironment environment;
        private readonly IIdentityServerInteractionService interaction;
        private readonly ILogger logger;

        public HomeController(IIdentityServerInteractionService interaction,
                              IWebHostEnvironment environment,
                              ILogger<HomeController> logger)
        {
            this.interaction = interaction;
            this.environment = environment;
            this.logger = logger;
        }

        public IActionResult Index()
        {
            if(environment.IsDevelopment())
            {
                // only show in development
                return View();
            }

            logger.LogInformation("Homepage is disabled in production. Returning 404.");
            return NotFound();
        }

        /// <summary>
        ///     Shows the error page
        /// </summary>
        public async Task<IActionResult> Error(string errorId)
        {
            ErrorViewModel vm = new ErrorViewModel();

            // retrieve error details from identityserver
            ErrorMessage message = await interaction.GetErrorContextAsync(errorId);
            if(message != null)
            {
                vm.Error = message;

                if(!environment.IsDevelopment())
                {
                    // only show in development
                    message.ErrorDescription = null;
                }
            }

            return View("Error", vm);
        }

        public async Task<IActionResult> CallApi()
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string content = await client.GetStringAsync("http://localhost:5001/identity");

            ViewBag.Json = JArray.Parse(content)
                                 .ToString();
            return View("json");
        }

    }

}
