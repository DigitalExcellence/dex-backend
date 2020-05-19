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

using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer
{

    /// <summary>
    ///     This sample controller allows a user to revoke grants given to clients
    /// </summary>
    [SecurityHeaders]
    [Authorize]
    public class GrantsController : Controller
    {

        private readonly IClientStore clients;
        private readonly IEventService events;
        private readonly IIdentityServerInteractionService interaction;
        private readonly IResourceStore resources;

        public GrantsController(IIdentityServerInteractionService interaction,
                                IClientStore clients,
                                IResourceStore resources,
                                IEventService events)
        {
            this.interaction = interaction;
            this.clients = clients;
            this.resources = resources;
            this.events = events;
        }

        /// <summary>
        ///     Show list of grants
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View("Index", await BuildViewModelAsync());
        }

        /// <summary>
        ///     Handle postback to revoke a client
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Revoke(string clientId)
        {
            await interaction.RevokeUserConsentAsync(clientId);
            await events.RaiseAsync(new GrantsRevokedEvent(User.GetSubjectId(), clientId));

            return RedirectToAction("Index");
        }

        private async Task<GrantsViewModel> BuildViewModelAsync()
        {
            IEnumerable<Consent> grants = await interaction.GetAllUserConsentsAsync();

            List<GrantViewModel> list = new List<GrantViewModel>();
            foreach(Consent grant in grants)
            {
                Client client = await clients.FindClientByIdAsync(grant.ClientId);
                if(client != null)
                {
                    Resources resources = await this.resources.FindResourcesByScopeAsync(grant.Scopes);

                    GrantViewModel item = new GrantViewModel
                                          {
                                              ClientId = client.ClientId,
                                              ClientName = client.ClientName ?? client.ClientId,
                                              ClientLogoUrl = client.LogoUri,
                                              ClientUrl = client.ClientUri,
                                              Created = grant.CreationTime,
                                              Expires = grant.Expiration,
                                              IdentityGrantNames = resources
                                                                   .IdentityResources
                                                                   .Select(x => x.DisplayName ?? x.Name)
                                                                   .ToArray(),
                                              ApiGrantNames = resources
                                                              .ApiResources.Select(x => x.DisplayName ?? x.Name)
                                                              .ToArray()
                                          };

                    list.Add(item);
                }
            }

            return new GrantsViewModel
                   {
                       Grants = list
                   };
        }

    }

}
