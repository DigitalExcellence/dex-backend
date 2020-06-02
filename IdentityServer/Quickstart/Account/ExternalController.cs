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
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class ExternalController : Controller
    {
        private readonly IClientStore clientStore;
        private readonly IEventService events;
        private readonly IIdentityServerInteractionService interaction;
        private readonly ILogger<ExternalController> logger;
        private readonly TestUserStore users;

        public ExternalController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IEventService events,
            ILogger<ExternalController> logger,
            TestUserStore users = null)
        {
            // if the TestUserStore is not in DI, then we'll just use the global users collection
            // this is where you would plug in your own custom identity management library (e.g. ASP.NET Identity)
            this.users = users ?? new TestUserStore(TestUsers.Users);

            this.interaction = interaction;
            this.clientStore = clientStore;
            this.logger = logger;
            this.events = events;
        }

        /// <summary>
        ///     initiate roundtrip to external authentication provider
        /// </summary>
        [HttpGet]
        public IActionResult Challenge(string provider, string returnUrl)
        {
            if(string.IsNullOrEmpty(returnUrl)) returnUrl = "~/";

            // validate returnUrl - either it is a valid OIDC URL or back to a local page
            if(!Url.IsLocalUrl(returnUrl) &&
               !interaction.IsValidReturnUrl(returnUrl))
            {
                // user might have clicked on a malicious link - should be logged
                throw new Exception("invalid return URL");
            }

            // start challenge and roundtrip the return URL and scheme 
            AuthenticationProperties props = new AuthenticationProperties
                                             {
                                                 RedirectUri = Url.Action(nameof(Callback)),
                                                 Items =
                                                 {
                                                     {"returnUrl", returnUrl},
                                                     {"scheme", provider}
                                                 }
                                             };

            return Challenge(props, provider);
        }

        /// <summary>
        ///     Post processing of external authentication
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Callback()
        {
            // read external identity from the temporary cookie
            AuthenticateResult result =
                await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme).ConfigureAwait(false);
            if(result?.Succeeded != true)
            {
                throw new Exception("External authentication error");
            }

            if(logger.IsEnabled(LogLevel.Debug))
            {
                IEnumerable<string> externalClaims = result.Principal.Claims.Select(c => $"{c.Type}: {c.Value}");
                logger.LogDebug("External claims: {@claims}", externalClaims);
            }

            // lookup our user and external provider info
            (TestUser user, string provider, string providerUserId, IEnumerable<Claim> claims) =
                FindUserFromExternalProvider(result);

            //TODO: Call our API and add the user to our database
            //TODO: Calling our API, requires the identity server to be authenticated, is this done with ClientCredential flow?
            //TODO: Since we do not want users to register, we can remove the part from below, correct?
            if(user == null)
            {
                // this might be where you might initiate a custom workflow for user registration
                // in this sample we don't show how that would be done, as our sample implementation
                // simply auto-provisions new external user
                user = AutoProvisionUser(provider, providerUserId, claims);
            }

            // this allows us to collect any additional claims or properties
            // for the specific protocols used and store them in the local auth cookie.
            // this is typically used to store data needed for signout from those protocols.
            List<Claim> additionalLocalClaims = new List<Claim>();
            AuthenticationProperties localSignInProps = new AuthenticationProperties();
            ProcessLoginCallbackForOidc(result, additionalLocalClaims, localSignInProps);

            // issue authentication cookie for user
            IdentityServerUser isuser = new IdentityServerUser(user.SubjectId)
                                        {
                                            DisplayName = user.Username,
                                            IdentityProvider = provider,
                                            AdditionalClaims = additionalLocalClaims
                                        };

            await HttpContext.SignInAsync(isuser, localSignInProps).ConfigureAwait(false);

            // delete temporary cookie used during external authentication
            await HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme).ConfigureAwait(false);

            // retrieve return URL
            string returnUrl = result.Properties.Items["returnUrl"] ?? "~/";

            // check if external login is in the context of an OIDC request
            AuthorizationRequest context = await interaction.GetAuthorizationContextAsync(returnUrl).ConfigureAwait(false);
            await events.RaiseAsync(new UserLoginSuccessEvent(provider,
                                                               providerUserId,
                                                               user.SubjectId,
                                                               user.Username,
                                                               true,
                                                               context?.ClientId)).ConfigureAwait(false);

            if(context != null)
            {
                if(await clientStore.IsPkceClientAsync(context.ClientId).ConfigureAwait(false))
                {
                    // if the client is PKCE then we assume it's native, so this change in how to
                    // return the response is for better UX for the end user.
                    return this.LoadingPage("Redirect", returnUrl);
                }
            }

            return Redirect(returnUrl);
        }

        private (TestUser user, string provider, string providerUserId, IEnumerable<Claim> claims)
            FindUserFromExternalProvider(AuthenticateResult result)
        {
            ClaimsPrincipal externalUser = result.Principal;

            // try to determine the unique id of the external user (issued by the provider)
            // the most common claim type for that are the sub claim and the NameIdentifier
            // depending on the external provider, some other claim type might be used
            Claim userIdClaim = externalUser.FindFirst(JwtClaimTypes.Subject) ??
                                externalUser.FindFirst(ClaimTypes.NameIdentifier) ??
                                throw new Exception("Unknown userid");

            // remove the user id claim so we don't include it as an extra claim if/when we provision the user
            List<Claim> claims = externalUser.Claims.ToList();
            claims.Remove(userIdClaim);

            string provider = result.Properties.Items["scheme"];
            string providerUserId = userIdClaim.Value;

            // find external user
            TestUser user = users.FindByExternalProvider(provider, providerUserId);

            return (user, provider, providerUserId, claims);
        }

        private TestUser AutoProvisionUser(string provider, string providerUserId, IEnumerable<Claim> claims)
        {
            TestUser user = users.AutoProvisionUser(provider, providerUserId, claims.ToList());
            return user;
        }

        private void ProcessLoginCallbackForOidc(AuthenticateResult externalResult,
                                                 List<Claim> localClaims,
                                                 AuthenticationProperties localSignInProps)
        {
            // if the external system sent a session id claim, copy it over
            // so we can use it for single sign-out
            Claim sid = externalResult.Principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.SessionId);
            if(sid != null)
            {
                localClaims.Add(new Claim(JwtClaimTypes.SessionId, sid.Value));
            }

            // if the external provider issued an id_token, we'll keep it for signout
            string idToken = externalResult.Properties.GetTokenValue("id_token");
            if(idToken != null)
            {
                localSignInProps.StoreTokens(new[]
                                             {
                                                 new AuthenticationToken
                                                 {
                                                     Name = "id_token",
                                                     Value = idToken
                                                 }
                                             });
            }
        }
    }
}
