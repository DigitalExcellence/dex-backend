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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer
{

    /// <summary>
    ///     This controller processes the consent UI
    /// </summary>
    [SecurityHeaders]
    [Authorize]
    public class ConsentController : Controller
    {

        private readonly IClientStore _clientStore;
        private readonly IEventService _events;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly ILogger<ConsentController> _logger;
        private readonly IResourceStore _resourceStore;

        public ConsentController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IResourceStore resourceStore,
            IEventService events,
            ILogger<ConsentController> logger)
        {
            _interaction = interaction;
            _clientStore = clientStore;
            _resourceStore = resourceStore;
            _events = events;
            _logger = logger;
        }

        /// <summary>
        ///     Shows the consent screen
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index(string returnUrl)
        {
            ConsentViewModel vm = await BuildViewModelAsync(returnUrl);
            if(vm != null)
            {
                return View("Index", vm);
            }

            return View("Error");
        }

        /// <summary>
        ///     Handles the consent screen postback
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ConsentInputModel model)
        {
            ProcessConsentResult result = await ProcessConsent(model);

            if(result.IsRedirect)
            {
                if(await _clientStore.IsPkceClientAsync(result.ClientId))
                {
                    // if the client is PKCE then we assume it's native, so this change in how to
                    // return the response is for better UX for the end user.
                    return this.LoadingPage("Redirect", result.RedirectUri);
                }

                return Redirect(result.RedirectUri);
            }

            if(result.HasValidationError)
            {
                ModelState.AddModelError(string.Empty, result.ValidationError);
            }

            if(result.ShowView)
            {
                return View("Index", result.ViewModel);
            }

            return View("Error");
        }

        /*****************************************/
        /* helper APIs for the ConsentController */
        /*****************************************/
        private async Task<ProcessConsentResult> ProcessConsent(ConsentInputModel model)
        {
            ProcessConsentResult result = new ProcessConsentResult();

            // validate return url is still valid
            AuthorizationRequest request = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
            if(request == null) return result;

            ConsentResponse grantedConsent = null;

            // user clicked 'no' - send back the standard 'access_denied' response
            if(model?.Button == "no")
            {
                grantedConsent = ConsentResponse.Denied;

                // emit event
                await _events.RaiseAsync(
                    new ConsentDeniedEvent(User.GetSubjectId(), request.ClientId, request.ScopesRequested));
            }

            // user clicked 'yes' - validate the data
            else if(model?.Button == "yes")
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
                await _interaction.GrantConsentAsync(request, grantedConsent);

                // indicate that's it ok to redirect back to authorization endpoint
                result.RedirectUri = model.ReturnUrl;
                result.ClientId = request.ClientId;
            } else
            {
                // we need to redisplay the consent UI
                result.ViewModel = await BuildViewModelAsync(model.ReturnUrl, model);
            }

            return result;
        }

        private async Task<ConsentViewModel> BuildViewModelAsync(string returnUrl, ConsentInputModel model = null)
        {
            AuthorizationRequest request = await _interaction.GetAuthorizationContextAsync(returnUrl);
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
                        return CreateConsentViewModel(model, returnUrl, request, client, resources);
                    }
                    _logger.LogError("No scopes matching: {0}",
                                     request.ScopesRequested.Aggregate((x, y) => x + ", " + y));
                } else
                {
                    _logger.LogError("Invalid client id: {0}", request.ClientId);
                }
            } else
            {
                _logger.LogError("No consent request matching request: {0}", returnUrl);
            }

            return null;
        }

        private ConsentViewModel CreateConsentViewModel(
            ConsentInputModel model,
            string returnUrl,
            AuthorizationRequest request,
            Client client,
            Resources resources)
        {
            ConsentViewModel vm = new ConsentViewModel
                                  {
                                      RememberConsent = model?.RememberConsent ?? true,
                                      ScopesConsented = model?.ScopesConsented ?? Enumerable.Empty<string>(),

                                      ReturnUrl = returnUrl,

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
