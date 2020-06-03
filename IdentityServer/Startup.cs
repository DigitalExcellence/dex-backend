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
using IdentityServer.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;





namespace IdentityServer
{
    /// <summary>
    ///     Startup file for Identity Server
    /// </summary>
    public class Startup
    {
        /// <summary>
        ///     Startup constructor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Config = configuration.GetSection("App")
                                  .Get<Config>();
            Configuration = configuration;
            Environment = environment;
        }

        /// <summary>
        ///     Configuration for Identity server
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        ///     Config for Identity server
        /// </summary>
        public Config Config { get; }

        /// <summary>
        /// Gets the environment.
        /// </summary>
        /// <value>
        /// The environment.
        /// </value>
        public IWebHostEnvironment Environment { get; }

        /// <summary>
        ///     Configure services for the identity server
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // configures the OpenIdConnect handlers to persist the state parameter into the server-side IDistributedCache.
            services.AddOidcStateDataFormatterCache();

            services.AddControllersWithViews();

            IIdentityServerBuilder builder = services.AddIdentityServer(options =>
                                                     {
                                                         options.Events.RaiseErrorEvents = true;
                                                         options.Events.RaiseInformationEvents =
                                                             true;
                                                         options.Events.RaiseFailureEvents = true;
                                                         options.Events.RaiseSuccessEvents = true;
                                                         if(Environment.IsDevelopment())
                                                         {
                                                             options.IssuerUri = Config.Self.IssuerUri;
                                                         }
                                                     })
                                                     .AddTestUsers(TestUsers.Users);

            // in-memory, code config
            builder.AddInMemoryIdentityResources(IdentityConfig.GetIdentityResources());
            builder.AddInMemoryApiResources(IdentityConfig.Apis);
            builder.AddInMemoryClients(IdentityConfig.Clients(Config));
            builder.AddTestUsers(TestUsers.Users);

            services.AddSingleton(Config);

            // sets the authentication schema.
            services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    })
                    // Adds Fontys Single Sign On authentication.
                    .AddOpenIdConnect("oidc", "Fontys", options =>
                        {
                            options.ClientId = Config.FfhictOIDC.ClientId;
                            options.ClientSecret = Config.FfhictOIDC.ClientSecret;
                            options.Authority = Config.FfhictOIDC.Authority;
                            options.ResponseType = "code";
                            options.Scope.Clear();

                            string[] scopes = Config.FfhictOIDC.Scopes.Split(" ");
                            foreach(string scope in scopes)
                            {
                                options.Scope.Add(scope);
                            }

                            // Set this flow to get the refresh token.
                            // options.Scope.Add("offline_access");

                            options.SaveTokens = true;
                            options.GetClaimsFromUserInfoEndpoint = true;

                            // This sets the redirect uri, this is needed because the blackbox implementation does not implement fontys SSO.
                            options.Events.OnRedirectToIdentityProvider = async n =>
                            {
                                n.ProtocolMessage.RedirectUri = Config.FfhictOIDC.RedirectUri;
                                await Task.FromResult(0);
                            };
                        }
                        // Add jwt validation this is so that the DGS can authenticate.
                    ).AddJwtBearer(o =>
                    {
                        o.SaveToken = true;
                        o.Authority = Config.Self.JwtAuthority;
                        o.RequireHttpsMetadata = false;
                        o.TokenValidationParameters = new TokenValidationParameters()
                                                      {
                                                          ValidateActor = false,
                                                          ValidateAudience = false,
                                                          NameClaimType = "name",
                                                          RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
                                                      };
                    })
                    .AddCookie();

            if(Environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                certStore.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection certCollection = certStore.Certificates.Find(
                    X509FindType.FindByIssuerName,
                    "Let's Encrypt Authority X3",
                    false
                );
                if(certCollection.Count > 0)
                {
                    X509Certificate2 certificate = certCollection[0];
                    builder.AddSigningCredential(certificate);
                }
            }

            // TODO tighten cors
            services.AddCors(options =>
            {
                options.AddPolicy("dex-api",
                                  policy =>
                                  {
                                      policy.AllowAnyOrigin()
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                                  });
            });
        }

        /// <summary>
        ///     Configure the application
        /// </summary>
        /// <param name="app"></param>
        public void Configure(IApplicationBuilder app)
        {
            if(Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseCors("dex-api");
            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
        }
    }
}
