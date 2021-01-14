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
using IdentityServer.Quickstart;
using IdentityServer.Quickstart.Account;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Models;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace IdentityServer
{

    /// <summary>
    ///     This sample controller implements a typical login/logout/provision workflow for local and external accounts.
    ///     The login service encapsulates the interactions with the user data store. This data store is in-memory only and
    ///     cannot be used for production!
    ///     The interaction service provides a way for the UI to communicate with identityserver for validation and context
    ///     retrieval
    /// </summary>
    [SecurityHeaders]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IClientStore clientStore;
        private readonly IEventService events;
        private readonly IIdentityServerInteractionService interaction;
        private readonly IAuthenticationSchemeProvider schemeProvider;
        private readonly IIdentityUserService identityUserService;

        public AccountController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events,
            IIdentityUserService identityUserService)
        {
            this.identityUserService = identityUserService;
            this.interaction = interaction;
            this.clientStore = clientStore;
            this.schemeProvider = schemeProvider;
            this.events = events;
        }

        /// <summary>
        ///     Entry point into the login workflow
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            // build a model so we know what to show on the login page
            LoginViewModel vm = await BuildLoginViewModelAsync(returnUrl);

            if(vm.IsExternalLoginOnly)
            {
                // we only have one option for logging in and it's an external provider
                return RedirectToAction("Challenge",
                                        "External",
                                        new
                                        {
                                            provider = vm.ExternalLoginScheme,
                                            returnUrl
                                        });
            }

            // get the provider parameter from the return url
            if(vm.ReturnUrl == null)
            {
                vm.ReturnUrl = "";
            }
            int idx = vm.ReturnUrl.IndexOf('?');
            string query = idx >= 0 ? vm.ReturnUrl.Substring(idx) : "";
            string providerSchema = HttpUtility.ParseQueryString(query).Get("provider");

            if(vm.VisibleExternalProviders.FirstOrDefault(i => i.AuthenticationScheme == providerSchema) != null)
            {
                return RedirectToAction("Challenge",
                                        "External",
                                        new
                                        {
                                            provider = providerSchema,
                                            returnUrl
                                        });
            }

            return View(vm);
        }

        /// <summary>
        ///     Handle postback from username/password login
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputModel model, string button)
        {
            // check if we are in the context of an authorization request
            AuthorizationRequest context = await interaction.GetAuthorizationContextAsync(model.ReturnUrl);

            // the user clicked the "cancel" button
            if(button != "login")
            {
                if(context != null)
                {
                    // if the user cancels, send a result back into IdentityServer as if they 
                    // denied the consent (even if this client does not require consent).
                    // this will send back an access denied OIDC error response to the client.
                    await interaction.GrantConsentAsync(context, ConsentResponse.Denied);

                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    if(await clientStore.IsPkceClientAsync(context.ClientId))
                    {
                        // if the client is PKCE then we assume it's native, so this change in how to
                        // return the response is for better UX for the end user.
                        return this.LoadingPage("Redirect", model.ReturnUrl);
                    }

                    return Redirect(model.ReturnUrl);
                }

                // since we don't have a valid context, then we just go back to the home page
                return Redirect("~/");
            }

            if(ModelState.IsValid)
            {
                // validate username/password against in-memory store
                if(await identityUserService.ValidateCredentialsAsync(model.Username, model.Password))
                {

                    IdentityUser user = await identityUserService.FindByUsername(model.Username);
                    await events.RaiseAsync(new UserLoginSuccessEvent(user.Username,
                                                                       user.SubjectId,
                                                                       user.Username,
                                                                       clientId: context?.ClientId));

                    // only set explicit expiration here if user chooses "remember me". 
                    // otherwise we rely upon expiration configured in cookie middleware.
                    AuthenticationProperties props = null;
                    if(AccountOptions.AllowRememberLogin &&
                       model.RememberLogin)
                    {
                        props = new AuthenticationProperties
                                {
                                    IsPersistent = true,
                                    ExpiresUtc = DateTimeOffset.UtcNow.Add(AccountOptions.RememberMeLoginDuration)
                                };
                    }
                    
                    // issue authentication cookie with subject ID and username
                    IdentityServerUser isuser = new IdentityServerUser(user.SubjectId)
                                                {
                                                    DisplayName = user.Username
                                                };

                    await HttpContext.SignInAsync(isuser, props);

                    if(context != null)
                    {
                        if(await clientStore.IsPkceClientAsync(context.ClientId))
                        {
                            // if the client is PKCE then we assume it's native, so this change in how to
                            // return the response is for better UX for the end user.
                            return this.LoadingPage("Redirect", model.ReturnUrl);
                        }

                        // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                        return Redirect(model.ReturnUrl);
                    }

                    // request for a local page
                    if(Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    if(string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return Redirect("~/");
                    }

                    // user might have clicked on a malicious link - should be logged
                    throw new Exception("invalid return URL");
                }

                await events.RaiseAsync(
                    new UserLoginFailureEvent(model.Username, "invalid credentials", clientId: context?.ClientId));
                ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
            }

            // something went wrong, show form with error
            LoginViewModel vm = await BuildLoginViewModelAsync(model);
            return View(vm);
        }


        /// <summary>
        ///     Show logout page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            // build a model so the logout page knows what to display
            LogoutViewModel vm = await BuildLogoutViewModelAsync(logoutId);

            if(!vm.ShowLogoutPrompt)
            {
                // if the request for logout was properly authenticated from IdentityServer, then
                // we don't need to show the prompt and can just log the user out directly.
                return await Logout(vm);
            }

            return View(vm);
        }

        /// <summary>
        ///     Handle logout page postback
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            // build a model so the logged out page knows what to display
            LoggedOutViewModel vm = await BuildLoggedOutViewModelAsync(model.LogoutId);

            if(User?.Identity.IsAuthenticated == true)
            {
                // delete local authentication cookie
                await HttpContext.SignOutAsync();

                // raise the logout event
                await events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
            }

            // check if we need to trigger sign-out at an upstream identity provider
            if(vm.TriggerExternalSignout)
            {
                // build a return URL so the upstream provider will redirect back
                // to us after the user has logged out. this allows us to then
                // complete our single sign-out processing.
                string url = Url.Action("Logout",
                                        new
                                        {
                                            logoutId = vm.LogoutId
                                        });

                // this triggers a redirect to the external provider for sign-out
                return SignOut(new AuthenticationProperties
                               {
                                   RedirectUri = url
                               },
                               vm.ExternalAuthenticationScheme);
            }

            return View("LoggedOut", vm);
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }


        /*****************************************/
        /* helper APIs for the AccountController */
        /*****************************************/
        private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
        {
            AuthorizationRequest context = await interaction.GetAuthorizationContextAsync(returnUrl);
            if(context?.IdP != null &&
               await schemeProvider.GetSchemeAsync(context.IdP) != null)
            {
                bool local = context.IdP == IdentityServerConstants.LocalIdentityProvider;

                // this is meant to short circuit the UI and only trigger the one external IdP
                LoginViewModel vm = new LoginViewModel
                                    {
                                        EnableLocalLogin = local,
                                        ReturnUrl = returnUrl,
                                        Username = context?.LoginHint
                                    };

                if(!local)
                {
                    vm.ExternalProviders = new[]
                                           {
                                               new ExternalProvider
                                               {
                                                   AuthenticationScheme = context.IdP
                                               }
                                           };
                }

                return vm;
            }

            IEnumerable<AuthenticationScheme> schemes = await schemeProvider.GetAllSchemesAsync();

            List<ExternalProvider> providers = schemes
                                               .Where(x => x.DisplayName != null ||
                                                           x.Name.Equals(AccountOptions.WindowsAuthenticationSchemeName,
                                                                         StringComparison.OrdinalIgnoreCase))
                                               .Select(x => new ExternalProvider
                                                            {
                                                                DisplayName = x.DisplayName ?? x.Name,
                                                                AuthenticationScheme = x.Name
                                                            })
                                               .ToList();

            bool allowLocal = true;
            if(context?.ClientId != null)
            {
                Client client = await clientStore.FindEnabledClientByIdAsync(context.ClientId);
                if(client != null)
                {
                    allowLocal = client.EnableLocalLogin;

                    if(client.IdentityProviderRestrictions != null &&
                       client.IdentityProviderRestrictions.Any())
                    {
                        providers = providers
                                    .Where(provider =>
                                               client.IdentityProviderRestrictions.Contains(
                                                   provider.AuthenticationScheme))
                                    .ToList();
                    }
                }
            }

            return new LoginViewModel
                   {
                       AllowRememberLogin = AccountOptions.AllowRememberLogin,
                       EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                       ReturnUrl = returnUrl,
                       Username = context?.LoginHint,
                       ExternalProviders = providers.ToArray()
                   };
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
        {
            LoginViewModel vm = await BuildLoginViewModelAsync(model.ReturnUrl);
            vm.Username = model.Username;
            vm.RememberLogin = model.RememberLogin;
            return vm;
        }

        private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
        {
            LogoutViewModel vm = new LogoutViewModel
                                 {
                                     LogoutId = logoutId,
                                     ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt
                                 };

            if(User?.Identity.IsAuthenticated != true)
            {
                // if the user is not authenticated, then just show logged out page
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            LogoutRequest context = await interaction.GetLogoutContextAsync(logoutId);
            if(context?.ShowSignoutPrompt == false)
            {
                // it's safe to automatically sign-out
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.
            return vm;
        }

        private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
        {
            // get context information (client name, post logout redirect URI and iframe for federated signout)
            LogoutRequest logout = await interaction.GetLogoutContextAsync(logoutId);

            LoggedOutViewModel vm = new LoggedOutViewModel
                                    {
                                        AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
                                        PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                                        ClientName =
                                            string.IsNullOrEmpty(logout?.ClientName)
                                                ? logout?.ClientId
                                                : logout?.ClientName,
                                        SignOutIframeUrl = logout?.SignOutIFrameUrl,
                                        LogoutId = logoutId
                                    };

            if(User?.Identity.IsAuthenticated == true)
            {
                string idp = User.FindFirst(JwtClaimTypes.IdentityProvider)
                                 ?.Value;
                if(idp != null &&
                   idp != IdentityServerConstants.LocalIdentityProvider)
                {
                    bool providerSupportsSignout = await HttpContext.GetSchemeSupportsSignOutAsync(idp);
                    if(providerSupportsSignout)
                    {
                        if(vm.LogoutId == null)
                        {
                            // if there's no current logout context, we need to create one
                            // this captures necessary info from the current logged in user
                            // before we signout and redirect away to the external IdP for signout
                            vm.LogoutId = await interaction.CreateLogoutContextAsync();
                        }

                        vm.ExternalAuthenticationScheme = idp;
                    }
                }
            }

            return vm;
        }

        
        [HttpPut]
        /*[ValidateAntiForgeryToken]*/
        public async Task<IActionResult> ChangeCredentials()
        {
            string pass = Request.Headers.FirstOrDefault(x => x.Key == "password").Value.FirstOrDefault();
            string email = Request.Headers.FirstOrDefault(x => x.Key == "email").Value.FirstOrDefault();
            string subjectId = Request.Headers.FirstOrDefault(x => x.Key == "subjectId").Value.FirstOrDefault();

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
