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
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Security.Cryptography.X509Certificates;

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

            // services.AddAuthentication()
            //     .AddOpenIdConnect("FHICT", "Fontys FHICT", options =>
            //     {
            //         options.ClientId = "";
            //         options.ClientSecret = "";
            //         options.Authority = "";
            //         // ...
            //     });

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
                if (certCollection.Count > 0)
                {
                    X509Certificate2 certificate = certCollection[0];
                    builder.AddSigningCredential(certificate);
                }
            }

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
