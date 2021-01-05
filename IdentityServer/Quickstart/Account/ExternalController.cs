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
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;
using RestSharp;
using Services.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
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
        private readonly Config config;
        private readonly IIdentityUserService identityUserService;

        public ExternalController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IEventService events,
            ILogger<ExternalController> logger,
            Config config,
            IIdentityUserService identityUserService)
        {
            this.identityUserService = identityUserService;
            this.interaction = interaction;
            this.clientStore = clientStore;
            this.logger = logger;
            this.events = events;
            this.config = config;
            //this.dataProviderService = dataProviderService;
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
        /// <param name="code">The authorization code.</param>
        /// <param name="state">The authorization state.</param>
        /// <param name="session_state">State of the session.</param>
        /// <returns>Redirection to the callback url.</returns>
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
            request.AddParameter("redirect_uri", config.FfhictOIDC.RedirectUri);
            request.AddParameter("client_id", config.FfhictOIDC.ClientId);
            request.AddParameter("client_secret", config.FfhictOIDC.ClientSecret);
            IRestResponse response = client.Execute(request);

            if(response.StatusCode != HttpStatusCode.OK)
            {
                return BadRequest("Could not validate the identity server authentication.");
            }

            // Parse the content to get the access token.
            ExternalConnectToken fhictToken = JsonConvert.DeserializeObject<ExternalConnectToken>(response.Content);

            if(string.IsNullOrWhiteSpace(fhictToken.AccessToken))
            {
                throw new Exception("The FHICT didn't return a correct response. Is the FHICT server accessible?", new Exception("Content:\n" + response.Content + "\n\nError:\n" + response.ErrorMessage, response.ErrorException));
            }

            JwtSecurityToken jwt = new JwtSecurityToken(fhictToken.AccessToken);
            string idp = (string) jwt.Payload.FirstOrDefault(c => c.Key.Equals("idp")).Value;
            string iss = (string) jwt.Payload.FirstOrDefault(c => c.Key.Equals("iss")).Value;

            ExternalResult result = new ExternalResult
            {
                Schema = iss,
                Claims = jwt.Claims,
                ReturnUrl = returnUrl,
                IdToken = fhictToken.IdToken
            };

            // lookup our user and external provider info
            (IdentityUser user, string provider, string providerUserId, IEnumerable<Claim> claims) = await FindUserFromExternalProvider(result);

            if(user == null)
            {
                //Retrieve more user information from the external source.
                // Get User information
                RestClient informationClient = new RestClient($"{iss}/connect/userinfo");
                RestRequest informationRequest = new RestRequest(Method.GET);
                informationRequest.AddHeader("Authorization", $"Bearer {fhictToken.AccessToken}");
                IRestResponse informationResponse = informationClient.Execute(informationRequest);
                ExternalUserInfo userinfo = JsonConvert.DeserializeObject<ExternalUserInfo>(informationResponse.Content);

                List<Claim> claimsList = claims.ToList();
                claimsList.Add(new Claim("email", userinfo.PreferredUsername));
                claimsList.Add(new Claim("idp", idp));
                claimsList.Add(new Claim("name", userinfo.Name));
                IdentityUser toInsertuser = new IdentityUser()
                {
                    ProviderId = provider,
                    ExternalSubjectId = providerUserId,
                    Email = userinfo.Email,
                    Firstname = userinfo.GivenName,
                    Lastname = userinfo.FamilyName,
                    Name = userinfo.Name,
                    Username = userinfo.PreferredUsername,
                    ExternalProfileUrl = userinfo.Profile
                };

                // simply auto-provisions new external user
                user = await identityUserService.AutoProvisionUser(toInsertuser);
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
                DisplayName = user.Name,
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
        /// Default callback function.
        /// </summary>
        /// <returns>Redirection to the callback url.</returns>
        /// <exception cref="Exception">External authentication error</exception>
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
            (IdentityUser user, string provider, string providerUserId, IEnumerable<Claim> claims) =
                await FindUserFromExternalProvider(null);

            if(user == null)
            {
                // this might be where you might initiate a custom workflow for user registration
                // in this sample we don't show how that would be done, as our sample implementation
                // simply auto-provisions new external user
                user = await identityUserService.AutoProvisionUser(provider, providerUserId, claims.ToList());
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

        ///// <summary>
        ///// This method returns the url of the oauth login page of the correct data provider.
        ///// </summary>
        ///// <param name="guid">The guid is used for identifying the data provider.</param>
        ///// <returns>This method returns the url of the oauth login page.</returns>
        //[HttpGet]
        //public async Task<IActionResult> ExternalAuthentication(string guid, string redirectUrl)
        //{
        //    HttpContext.Response.Cookies.Append("redirectUrl", redirectUrl);

        //    string oauthUrl = await dataProviderService.GetOauthUrl(guid);
        //    return Redirect(oauthUrl);
        //}

        ///// <summary>
        ///// This method converts the retrieved code to the tokens to use in the external API.
        ///// </summary>
        ///// <param name="code">This is the retrieved code from the login page from the external data source.</param>
        ///// <param name="state">This is a random string which can be a variable, for example the guid.</param>
        ///// <returns>This method returns the correct tokens.</returns>
        //public async Task<IActionResult> RetrieveTokens(string code, string state)
        //{
        //    OauthTokens tokens = await dataProviderService.GetTokens(code, state);

        //    HttpContext.Response.Headers.Add("OauthTokens", JsonConvert.SerializeObject(tokens));

        //    string returnUrl = Request.Cookies["redirectUrl"];
        //    return Redirect(returnUrl);
        //}

        /// <summary>
        /// Finds the user from external provider.
        /// </summary>
        /// <param name="result">The ExternalResult information.</param>
        /// <returns>The user from the identity server, the external provider uri, The external user id, the claims</returns>
        /// <exception cref="System.Exception">Unknown userid</exception>
        private async Task<(IdentityUser user, string provider, string providerUserId, IEnumerable<Claim> claims)> FindUserFromExternalProvider(ExternalResult result)
        {
            // try to determine the unique id of the external user (issued by the provider)
            // the most common claim type for that are the sub claim and the NameIdentifier
            // depending on the external provider, some other claim type might be used
            Claim userIdClaim = result.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Subject) ??
                                result.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier) ??
                                throw new Exception("Unknown userid");

            // remove the user id claim so we don't include it as an extra claim if/when we provision the user
            List<Claim> claims = result.Claims.ToList();
            claims.Remove(userIdClaim);

            string provider = result.Schema;
            string providerUserId = userIdClaim.Value;

            // find external user
            IdentityUser user = await identityUserService.FindByExternalProvider(provider, providerUserId);

            return (user, provider, providerUserId, claims);
        }

        /// <summary>
        /// Processes the login callback for oidc.
        /// </summary>
        /// <param name="externalResult">The result model got from the external identity server.</param>
        /// <param name="localClaims">The extra claims our identity server can add.</param>
        /// <param name="localSignInProps">Properties that the identity server can add for self use.</param>
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
