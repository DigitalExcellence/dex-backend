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

using Configuration;
using IdentityModel;
using IdentityServer.Quickstart.Account;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

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
        private readonly Config config;
        public ExternalController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IEventService events,
            ILogger<ExternalController> logger,
            Config config,
            TestUserStore users = null)
        {
            // if the TestUserStore is not in DI, then we'll just use the global users collection
            // this is where you would plug in your own custom identity management library (e.g. ASP.NET Identity)
            this.users = users ?? new TestUserStore(TestUsers.Users);

            this.interaction = interaction;
            this.clientStore = clientStore;
            this.logger = logger;
            this.events = events;
            this.config = config;
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

            HttpContext.Response.Cookies.Append("returnUrl", returnUrl);

            return Challenge(props, provider);
        }

        /// <summary>
        /// The callback endpoint for the fontys single sign on.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="state">The state.</param>
        /// <param name="session_state">State of the session.</param>
        /// <returns></returns>
        /// <exception cref="Exception">
        /// The FHICT didn't return a correct response. Is the FHICT server accessible? - new Exception("Content:\n" + response.Content + "\n\nError:\n" + response.ErrorMessage, response.ErrorException)
        /// or
        /// Content:\n" + response.Content + "\n\nError:\n" + response.ErrorMessage
        /// </exception>
        [HttpPost("/external/callback/fhict")]
        public async Task<IActionResult> Callback(string code, string state, string session_state)
        {
            // Get the return url for the frontend from the cookies.
            string returnUrl = HttpContext.Request.Cookies["returnUrl"];
            HttpContext.Response.Cookies.Delete("returnUrl");

            //Request an accesstoken and idtoken from the authority.
            RestClient client = new RestClient(config.FfhictOIDC.Authority + "/connect/token");
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("code", code);
            request.AddParameter("redirect_uri",config.FfhictOIDC.RedirectUri);
            request.AddParameter("client_id", config.FfhictOIDC.ClientId);
            request.AddParameter("client_secret", config.FfhictOIDC.ClientSecret);
            IRestResponse response = client.Execute(request);

            if(response.StatusCode != HttpStatusCode.OK)
            {
                return BadRequest("Could not validate the identity server authentication.");
            }

            // Parse the content to get the access token.
            ExternalConnectToken fhictToken = JsonConvert.DeserializeObject<ExternalConnectToken>(response.Content);

            if(string.IsNullOrWhiteSpace(fhictToken.access_token))
            {
                throw new Exception("The FHICT didn't return a correct response. Is the FHICT server accessible?", new Exception("Content:\n" + response.Content + "\n\nError:\n" + response.ErrorMessage, response.ErrorException));
            }


            JwtSecurityToken jwt = new JwtSecurityToken(fhictToken.access_token);
            
            string idp = (string) jwt.Payload.FirstOrDefault(c => c.Key.Equals("idp")).Value;
            string sub = (string) jwt.Payload.FirstOrDefault(c => c.Key.Equals("sub")).Value;
            string name = (string) jwt.Payload.FirstOrDefault(c => c.Key.Equals("name")).Value;
            string iss = (string) jwt.Payload.FirstOrDefault(c => c.Key.Equals("iss")).Value;
            string schema = (string) jwt.Payload.FirstOrDefault(c => c.Key.Equals("schema")).Value;

            ExternalResult result = new ExternalResult();
            result.Schema = iss;
            result.Claims = jwt.Claims;
            result.ReturnUrl = returnUrl;
            result.IdToken = fhictToken.id_token;

            // lookup our user and external provider info
            (TestUser user, string provider, string providerUserId, IEnumerable<Claim> claims) = FindUserFromExternalProvider(result);

            if(user == null)
            {
                //Retrieve more user information from the external source.
                // Get User information
                RestClient informationClient = new RestClient($"{iss}/connect/userinfo");
                RestRequest informationRequest = new RestRequest(Method.GET);
                informationRequest.AddHeader("Authorization", $"Bearer {fhictToken.access_token}");
                IRestResponse informationResponse = informationClient.Execute(informationRequest);
                ExternalUserInfo userinfo =  JsonConvert.DeserializeObject<ExternalUserInfo>(informationResponse.Content);

                List<Claim> claimsList = claims.ToList();
                claimsList.Add(new Claim("email", userinfo.preferred_username));
                claimsList.Add(new Claim("idp", idp));
                claimsList.Add(new Claim("name", userinfo.preferred_username));

                // simply auto-provisions new external user
                user = AutoProvisionUser(provider, providerUserId, claimsList);
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

        /// <summary>
        ///     Post processing of external authentication
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> DefaultCallback()
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
                FindUserFromExternalProvider(null);

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
            ProcessLoginCallbackForOidc(null, additionalLocalClaims, localSignInProps);

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


        


        

        

        private (TestUser user, string provider, string providerUserId, IEnumerable<Claim> claims) FindUserFromExternalProvider(ExternalResult result)
        {
            // try to determine the unique id of the external user (issued by the provider)
            // the most common claim type for that are the sub claim and the NameIdentifier
            // depending on the external provider, some other claim type might be used
            Claim userIdClaim = result.Claims.Where(c => c.Type == JwtClaimTypes.Subject).FirstOrDefault() ??
                                result.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault() ??
                                throw new Exception("Unknown userid");

            // remove the user id claim so we don't include it as an extra claim if/when we provision the user
            List<Claim> claims = result.Claims.ToList();
            claims.Remove(userIdClaim);

            string provider = result.Schema;
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

        private void ProcessLoginCallbackForOidc(ExternalResult externalResult,
                                                 List<Claim> localClaims,
                                                 AuthenticationProperties localSignInProps)
        {
            // if the external system sent a session id claim, copy it over
            // so we can use it for single sign-out
            Claim sid = externalResult.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.SessionId);
            if(sid != null)
            {
                localClaims.Add(new Claim(JwtClaimTypes.SessionId, sid.Value));
            }

            // if the external provider issued an id_token, we'll keep it for signout
            string idToken = externalResult.IdToken;
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
