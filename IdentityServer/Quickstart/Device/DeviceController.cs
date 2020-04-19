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
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Device
{

    [Authorize]
    [SecurityHeaders]
    public class DeviceController : Controller
    {

        private readonly IClientStore _clientStore;
        private readonly IEventService _events;
        private readonly IDeviceFlowInteractionService _interaction;
        private readonly ILogger<DeviceController> _logger;
        private readonly IResourceStore _resourceStore;

        public DeviceController(
            IDeviceFlowInteractionService interaction,
            IClientStore clientStore,
            IResourceStore resourceStore,
            IEventService eventService,
            ILogger<DeviceController> logger)
        {
            _interaction = interaction;
            _clientStore = clientStore;
            _resourceStore = resourceStore;
            _events = eventService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery(Name = "user_code")] string userCode)
        {
            if(string.IsNullOrWhiteSpace(userCode)) return View("UserCodeCapture");

            DeviceAuthorizationViewModel vm = await BuildViewModelAsync(userCode);
            if(vm == null) return View("Error");

            vm.ConfirmUserCode = true;
            return View("UserCodeConfirmation", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserCodeCapture(string userCode)
        {
            DeviceAuthorizationViewModel vm = await BuildViewModelAsync(userCode);
            if(vm == null) return View("Error");

            return View("UserCodeConfirmation", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Callback(DeviceAuthorizationInputModel model)
        {
            if(model == null) throw new ArgumentNullException(nameof(model));

            ProcessConsentResult result = await ProcessConsent(model);
            if(result.HasValidationError) return View("Error");

            return View("Success");
        }

        private async Task<ProcessConsentResult> ProcessConsent(DeviceAuthorizationInputModel model)
        {
            ProcessConsentResult result = new ProcessConsentResult();

            DeviceFlowAuthorizationRequest request = await _interaction.GetAuthorizationContextAsync(model.UserCode);
            if(request == null) return result;

            ConsentResponse grantedConsent = null;

            // user clicked 'no' - send back the standard 'access_denied' response
            if(model.Button == "no")
            {
                grantedConsent = ConsentResponse.Denied;

                // emit event
                await _events.RaiseAsync(
                    new ConsentDeniedEvent(User.GetSubjectId(), request.ClientId, request.ScopesRequested));
            }

            // user clicked 'yes' - validate the data
            else if(model.Button == "yes")
            {
                // if the user consented to some scope, build the response model
                if(model.ScopesConsented != null &&
                   model.ScopesConsented.Any())
                {
                    IEnumerable<string> scopes = model.ScopesConsented;
                    if(ConsentOptions.EnableOfflineAccess == false)
                    {
                        scopes = scopes.Where(x => x != IdentityServerConstants.StandardScopes.OfflineAccess);
                    }

                    grantedConsent = new ConsentResponse
                                     {
                                         RememberConsent = model.RememberConsent,
                                         ScopesConsented = scopes.ToArray()
                                     };

                    // emit event
                    await _events.RaiseAsync(new ConsentGrantedEvent(User.GetSubjectId(),
                                                                     request.ClientId,
                                                                     request.ScopesRequested,
                                                                     grantedConsent.ScopesConsented,
                                                                     grantedConsent.RememberConsent));
                } else
                {
                    result.ValidationError = ConsentOptions.MustChooseOneErrorMessage;
                }
            } else
            {
                result.ValidationError = ConsentOptions.InvalidSelectionErrorMessage;
            }

            if(grantedConsent != null)
            {
                // communicate outcome of consent back to identityserver
                await _interaction.HandleRequestAsync(model.UserCode, grantedConsent);

                // indicate that's it ok to redirect back to authorization endpoint
                result.RedirectUri = model.ReturnUrl;
                result.ClientId = request.ClientId;
            } else
            {
                // we need to redisplay the consent UI
                result.ViewModel = await BuildViewModelAsync(model.UserCode, model);
            }

            return result;
        }

        private async Task<DeviceAuthorizationViewModel> BuildViewModelAsync(
            string userCode,
            DeviceAuthorizationInputModel model = null)
        {
            DeviceFlowAuthorizationRequest request = await _interaction.GetAuthorizationContextAsync(userCode);
            if(request != null)
            {
                Client client = await _clientStore.FindEnabledClientByIdAsync(request.ClientId);
                if(client != null)
                {
                    Resources resources =
                        await _resourceStore.FindEnabledResourcesByScopeAsync(request.ScopesRequested);
                    if(resources != null &&
                       (resources.IdentityResources.Any() || resources.ApiResources.Any()))
                    {
                        return CreateConsentViewModel(userCode, model, client, resources);
                    }
                    _logger.LogError("No scopes matching: {0}",
                                     request.ScopesRequested.Aggregate((x, y) => x + ", " + y));
                } else
                {
                    _logger.LogError("Invalid client id: {0}", request.ClientId);
                }
            }

            return null;
        }

        private DeviceAuthorizationViewModel CreateConsentViewModel(string userCode,
                                                                    DeviceAuthorizationInputModel model,
                                                                    Client client,
                                                                    Resources resources)
        {
            DeviceAuthorizationViewModel vm = new DeviceAuthorizationViewModel
                                              {
                                                  UserCode = userCode,

                                                  RememberConsent = model?.RememberConsent ?? true,
                                                  ScopesConsented =
                                                      model?.ScopesConsented ?? Enumerable.Empty<string>(),

                                                  ClientName = client.ClientName ?? client.ClientId,
                                                  ClientUrl = client.ClientUri,
                                                  ClientLogoUrl = client.LogoUri,
                                                  AllowRememberConsent = client.AllowRememberConsent
                                              };

            vm.IdentityScopes = resources
                                .IdentityResources
                                .Select(x => CreateScopeViewModel(x,
                                                                  vm.ScopesConsented.Contains(x.Name) || model == null))
                                .ToArray();
            vm.ResourceScopes = resources.ApiResources.SelectMany(x => x.Scopes)
                                         .Select(x => CreateScopeViewModel(x,
                                                                           vm.ScopesConsented.Contains(x.Name) ||
                                                                           model == null))
                                         .ToArray();
            if(ConsentOptions.EnableOfflineAccess &&
               resources.OfflineAccess)
            {
                vm.ResourceScopes = vm.ResourceScopes.Union(new[]
                                                            {
                                                                GetOfflineAccessScope(
                                                                    vm.ScopesConsented.Contains(
                                                                        IdentityServerConstants
                                                                            .StandardScopes.OfflineAccess) ||
                                                                    model == null)
                                                            });
            }

            return vm;
        }

        private ScopeViewModel CreateScopeViewModel(IdentityResource identity, bool check)
        {
            return new ScopeViewModel
                   {
                       Name = identity.Name,
                       DisplayName = identity.DisplayName,
                       Description = identity.Description,
                       Emphasize = identity.Emphasize,
                       Required = identity.Required,
                       Checked = check || identity.Required
                   };
        }

        public ScopeViewModel CreateScopeViewModel(Scope scope, bool check)
        {
            return new ScopeViewModel
                   {
                       Name = scope.Name,
                       DisplayName = scope.DisplayName,
                       Description = scope.Description,
                       Emphasize = scope.Emphasize,
                       Required = scope.Required,
                       Checked = check || scope.Required
                   };
        }

        private ScopeViewModel GetOfflineAccessScope(bool check)
        {
            return new ScopeViewModel
                   {
                       Name = IdentityServerConstants.StandardScopes.OfflineAccess,
                       DisplayName = ConsentOptions.OfflineAccessDisplayName,
                       Description = ConsentOptions.OfflineAccessDescription,
                       Emphasize = true,
                       Checked = check
                   };
        }

    }

}
