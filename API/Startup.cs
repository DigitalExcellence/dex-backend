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

using API.Configuration;
using API.Extensions;
using API.Filters;
using API.HelperClasses;
using API.InternalResources;
using Data;
using Data.Helpers;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using IdentityModel.Client;
using MessageBrokerPublisher.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using Models;
using Models.Defaults;
using Newtonsoft.Json;
using Polly;
using Serilog;
using Services.Resources;
using Services.Services;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Sqlite;

namespace API
{
    /// <summary>
    ///     Startup file
    /// </summary>
    public class Startup
    {

        /// <summary>
        ///     Initializes a new instance of the <see cref="Startup" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="environment">The environment.</param>
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Config = configuration.GetSection("App")
                                  .Get<Config>();
            ElasticConfig = configuration.GetSection("App")
                                         .GetSection("Elastic")
                                         .Get<ElasticConfig>();
            Config.OriginalConfiguration = configuration;
            Environment = environment;
        }

        /// <summary>
        ///     Config file of API
        /// </summary>
        public Config Config { get; }

        /// <summary>
        ///     Config file of Elastic
        /// </summary>
        public ElasticConfig ElasticConfig { get; }

        /// <summary>
        ///     Environment of the API
        /// </summary>
        public IWebHostEnvironment Environment { get; }

        /// <summary>
        ///     Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;
            services.AddDbContext<ApplicationDbContext>(o =>
            {
                if (Environment.IsEnvironment("test"))
                {
                    o.UseInMemoryDatabase("inMemoryTestDatabase");
                }
                else
                {
                    o.UseSqlServer(Config.OriginalConfiguration.GetConnectionString("DefaultConnection"),
                        sqlOptions => sqlOptions.EnableRetryOnFailure(50, TimeSpan.FromSeconds(30), null));
                }
            });

            services.AddSingleton<IRabbitMQConnectionFactory>(c => new RabbitMQConnectionFactory(Config.RabbitMQ.Hostname, Config.RabbitMQ.Username, Config.RabbitMQ.Password));

            services.AddSingleton<IElasticSearchContext>(new ElasticSearchContext(ElasticConfig.Hostname, ElasticConfig.Username, ElasticConfig.Password, ElasticConfig.IndexUrl));

            services.AddAutoMapper();

            services.UseConfigurationValidation();
            services.ConfigureValidatableSetting<Config>(Config.OriginalConfiguration.GetSection("App"));

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddIdentityServerAuthentication(options =>
                    {
                        options.Authority = Config.IdentityServer.IdentityUrl;
                        options.RequireHttpsMetadata = false;
                        options.ApiName = Config.Frontend.ClientId;
                        options.ApiSecret = Config.Frontend.ClientSecret;
                        options.EnableCaching = true;
                    });

            services.AddAuthorization(o =>
            {
                o.AddPolicy(nameof(Defaults.Roles.BackendApplication),
                            policy => policy.RequireClaim("client_role", nameof(Defaults.Roles.BackendApplication)));

                o.AddPolicy(nameof(Defaults.Scopes.HighlightRead),
                            policy => policy.Requirements.Add(
                                new ScopeRequirement(nameof(Defaults.Scopes.HighlightRead))));
                o.AddPolicy(nameof(Defaults.Scopes.HighlightWrite),
                            policy => policy.Requirements.Add(
                                new ScopeRequirement(nameof(Defaults.Scopes.HighlightWrite))));

                o.AddPolicy(nameof(Defaults.Scopes.ProjectRead),
                            policy => policy.Requirements.Add(
                                new ScopeRequirement(nameof(Defaults.Scopes.ProjectRead))));
                o.AddPolicy(nameof(Defaults.Scopes.AdminProjectWrite),
                            policy => policy.Requirements.Add(
                                new ScopeRequirement(nameof(Defaults.Scopes.AdminProjectWrite))));
                o.AddPolicy(nameof(Defaults.Scopes.ProjectWrite),
                            policy => policy.Requirements.Add(
                                new ScopeRequirement(nameof(Defaults.Scopes.ProjectWrite))));

                o.AddPolicy(nameof(Defaults.Scopes.UserRead),
                            policy => policy.Requirements.Add(new ScopeRequirement(nameof(Defaults.Scopes.UserRead))));
                o.AddPolicy(nameof(Defaults.Scopes.UserWrite),
                            policy => policy.Requirements.Add(new ScopeRequirement(nameof(Defaults.Scopes.UserWrite))));

                o.AddPolicy(nameof(Defaults.Scopes.RoleRead),
                            policy => policy.Requirements.Add(new ScopeRequirement(nameof(Defaults.Scopes.RoleRead))));
                o.AddPolicy(nameof(Defaults.Scopes.RoleWrite),
                            policy => policy.Requirements.Add(new ScopeRequirement(nameof(Defaults.Scopes.RoleWrite))));

                o.AddPolicy(nameof(Defaults.Scopes.CategoryRead),
                            policy => policy.Requirements.Add(new ScopeRequirement(nameof(Defaults.Scopes.CategoryRead))));
                o.AddPolicy(nameof(Defaults.Scopes.CategoryWrite),
                            policy => policy.Requirements.Add(new ScopeRequirement(nameof(Defaults.Scopes.CategoryWrite))));

                o.AddPolicy(nameof(Defaults.Scopes.EmbedRead),
                            policy => policy.Requirements.Add(new ScopeRequirement(nameof(Defaults.Scopes.EmbedRead))));
                o.AddPolicy(nameof(Defaults.Scopes.EmbedWrite),
                            policy => policy.Requirements.Add(
                                new ScopeRequirement(nameof(Defaults.Scopes.EmbedWrite))));

                o.AddPolicy(nameof(Defaults.Scopes.InstitutionEmbedWrite),
                            policy => policy.Requirements.Add(
                                new ScopeRequirement(nameof(Defaults.Scopes.InstitutionEmbedWrite))));
                o.AddPolicy(nameof(Defaults.Scopes.InstitutionProjectWrite),
                            policy => policy.Requirements.Add(
                                new ScopeRequirement(nameof(Defaults.Scopes.InstitutionProjectWrite))));
                o.AddPolicy(nameof(Defaults.Scopes.InstitutionUserRead),
                            policy => policy.Requirements.Add(
                                new ScopeRequirement(nameof(Defaults.Scopes.InstitutionUserRead))));
                o.AddPolicy(nameof(Defaults.Scopes.InstitutionUserWrite),
                            policy => policy.Requirements.Add(
                                new ScopeRequirement(nameof(Defaults.Scopes.InstitutionUserWrite))));

                o.AddPolicy(nameof(Defaults.Scopes.InstitutionWrite),
                            policy => policy.Requirements.Add(
                                new ScopeRequirement(nameof(Defaults.Scopes.InstitutionWrite))));
                o.AddPolicy(nameof(Defaults.Scopes.InstitutionRead),
                            policy => policy.Requirements.Add(
                                new ScopeRequirement(nameof(Defaults.Scopes.InstitutionRead))));

                o.AddPolicy(nameof(Defaults.Scopes.DataSourceWrite),
                            policy => policy.Requirements.Add(
                                new ScopeRequirement(nameof(Defaults.Scopes.DataSourceWrite))));

                o.AddPolicy(nameof(Defaults.Scopes.FileWrite),
                            policy => policy.Requirements.Add(new ScopeRequirement(nameof(Defaults.Scopes.FileWrite))));

                o.AddPolicy(nameof(Defaults.Scopes.CallToActionOptionWrite),
                            policy => policy.Requirements.Add(
                                new ScopeRequirement(nameof(Defaults.Scopes.CallToActionOptionWrite))));

                o.AddPolicy(nameof(Defaults.Scopes.UserTaskWrite),
                            policy => policy.Requirements.Add(
                                new ScopeRequirement(nameof(Defaults.Scopes.UserTaskWrite))));

                o.AddPolicy(nameof(Defaults.Scopes.WizardPageWrite),
                            policy => policy.Requirements.Add(
                                new ScopeRequirement(nameof(Defaults.Scopes.WizardPageWrite))));

                o.AddPolicy(nameof(Defaults.Scopes.AdminProjectExport),
                    policy => policy.Requirements.Add(new ScopeRequirement(nameof(Defaults.Scopes.AdminProjectExport))));

            });

            services.AddCors();
            services.AddControllersWithViews()
                    .AddFluentValidation(c => c.RegisterValidatorsFromAssemblyContaining<Startup>())
                    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling =
                                                      ReferenceLoopHandling.Ignore);

            services.AddSwaggerGen(o =>
            {
                o.OperationFilter<DefaultOperationFilter>();
                o.SwaggerDoc("v1",
                             new OpenApiInfo
                             {
                                 Title = "Dex API",
                                 Version = "v1",
                                 Description =
                                     "Dex API Swagger surface. DeX provides a platform for students, teachers and employees to share and work on projects and ideas. Find, create, share and work on projects and ideas on DeX",
                                 License = new OpenApiLicense
                                           {
                                               Name = "GNU Lesser General Public License v3.0",
                                               Url = new Uri("https://www.gnu.org/licenses/lgpl-3.0.txt")
                                           }
                             });
                //o.IncludeXmlComments($"{AppDomain.CurrentDomain.BaseDirectory}{typeof(Startup).Namespace}.xml", true);

                o.AddSecurityDefinition("oauth2",
                                        new OpenApiSecurityScheme
                                        {
                                            Type = SecuritySchemeType.OAuth2,
                                            Flows = new OpenApiOAuthFlows
                                                    {
                                                        Implicit = new OpenApiOAuthFlow
                                                                   {
                                                                       AuthorizationUrl = GetAuthorizationUrl(),
                                                                       Scopes = new Dictionary<string, string>
                                                                           {
                                                                               {"dex-api", "Resource scope"}
                                                                           }
                                                                   }
                                                    }
                                        });
                o.AddSecurityRequirement(new OpenApiSecurityRequirement
                                         {
                                             {
                                                 new OpenApiSecurityScheme
                                                 {
                                                     Reference = new OpenApiReference
                                                                 {
                                                                     Type = ReferenceType.SecurityScheme,
                                                                     Id = "oauth2"
                                                                 }
                                                 },
                                                 new[] {""}
                                             }
                                         });
            });

            services.AddAccessTokenManagement(options =>
                    {
                        options.Client.Clients.Add("identityserver",
                                                   new ClientCredentialsTokenRequest
                                                   {
                                                       Address = Config.IdentityServer.IdentityUrl + "/connect/token",
                                                       ClientId = Config.IdentityServer.ClientId,
                                                       ClientSecret = Config.IdentityServer.ClientSecret
                                                   });
                    })
                    .ConfigureBackchannelHttpClient()
                    .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(new[]
                                                     {
                                                         TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2),
                                                         TimeSpan.FromSeconds(3)
                                                     }));
            services.AddClientAccessTokenClient("identityclient",
                                                configureClient: client =>
                                                {
                                                    client.BaseAddress =
                                                        new Uri(string.Concat(Config.IdentityServer.IdentityUrl + "/"));
                                                });

            // Add application services.
            services.AddSingleton(Config);
            services.AddSingleton(ElasticConfig);
            services.AddServicesAndRepositories();
            services.AddProblemDetails();
        }

        /// <summary>
        ///     Configures the specified application.
        ///     This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            env.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            Defaults.Path.FilePath = Path.Combine(env.WebRootPath, "Images");


            UpdateDatabase(app, env);
            if(env.IsDevelopment())
            {
                //app.UseBrowserLink();
                app.UseDeveloperExceptionPage();

                //app.UseDatabaseErrorPage();
            } else if(env.IsProduction())
            {
                app.UseExceptionHandler(new ExceptionHandlerOptions
                                        {
                                            ExceptionHandler = context =>
                                            {
                                                context.Response.ContentType = "text/HTML";
                                                context.Response.Redirect("/Error.html");
                                                return Task.CompletedTask;
                                            }
                                        });
            } else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseProblemDetails();

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
                               {
                                   FileProvider = new PhysicalFileProvider(
                                       Path.Combine(env.ContentRootPath, "Uploads", "Images")),
                                   RequestPath = "/Uploads/Images"
                               });

            app.UseRouting();
            app.UseCors(c =>
            {
                c.WithOrigins(Config.Frontend.FrontendUrl);
                c.SetIsOriginAllowedToAllowWildcardSubdomains();
                c.AllowAnyHeader();
                c.AllowAnyMethod();
            });
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            //UserInfo
            app.UseWhen(context =>
                            context.User.Identities.Any(i => i.IsAuthenticated),
                        appBuilder =>
                        {
                            appBuilder.Use(async (context, next) =>
                            {
                                DbContext dbContext = context.RequestServices.GetService<DbContext>();
                                IUserService userService =
                                    context.RequestServices.GetService<IUserService>();
                                string identityId = "";
                                try
                                {
                                    identityId = context.User.GetIdentityId(context);
                                } catch(UnauthorizedAccessException e)
                                {
                                    Log.Logger.Error(e, "User is not authorized.");
                                    await next();
                                }
                                IInstitutionService institutionService =
                                    context.RequestServices.GetService<IInstitutionService>();
                                UserCreateInternalResource userInformation = context.GetUserInformation(Config);
                                User user = await userService.GetUserByIdentityIdAsync(identityId)
                                                             .ConfigureAwait(false);
                                if(user == null)
                                {
                                    IRoleService roleService = context.RequestServices.GetService<IRoleService>();
                                    Role registeredUserRole =
                                        (await roleService.GetAll()).FirstOrDefault(
                                            i => i.Name == nameof(Defaults.Roles.RegisteredUser));

                                    if(userInformation == null)
                                    {
                                        // Then it probably belongs swagger so we set the username as developer.
                                        User newUser = new User
                                                       {
                                                           Name = "Developer",
                                                           Email = "Developer@DEX.com",
                                                           IdentityId = identityId,
                                                           Role = registeredUserRole,
                                                           InstitutionId = 1
                                                       };
                                        userService.Add(newUser);
                                    } else
                                    {
                                        User newUser = new User
                                                       {
                                                           Name = userInformation.Name,
                                                           Email = userInformation.Email,
                                                           IdentityId = userInformation.IdentityId,
                                                           Role = registeredUserRole
                                                       };
                                        Institution institution =
                                            await institutionService.GetInstitutionByInstitutionIdentityId(
                                                userInformation.IdentityInstitutionId);
                                        if(institution != null)
                                        {
                                            newUser.InstitutionId = institution.Id;
                                        }
                                        userService.Add(newUser);
                                    }
                                    await dbContext.SaveChangesAsync()
                                                   .ConfigureAwait(false);
                                }

                                // If the user is already in the database and the Update institution on login is set to true
                                else if(Config.OriginalConfiguration.GetValue<bool>("UpdateInstitutionOnLogin"))
                                {
                                    if(userInformation != null)
                                    {
                                        Institution institution =
                                            await institutionService.GetInstitutionByInstitutionIdentityId(
                                                userInformation.IdentityInstitutionId);
                                        if(institution != null) user.InstitutionId = institution.Id;

                                        //if(user.Email == "Developer@DEX.com" || user.Name == "Developer" || user.Email == "<Redacted>")
                                        //{
                                        //    user.Email = userInformation.Email;
                                        //    user.Name = userInformation.Name;
                                        //}

                                        userService.Update(user);
                                        await dbContext.SaveChangesAsync()
                                                       .ConfigureAwait(false);
                                    }
                                }
                                await next()
                                    .ConfigureAwait(false);
                            });
                        });

            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());

            app.UseSwagger();
            app.UseSwaggerUI(o =>
            {
                o.SwaggerEndpoint("/swagger/v1/swagger.json", "Dex API V1");
                o.DocExpansion(DocExpansion.None);
                o.RoutePrefix = "";
                o.DisplayRequestDuration();
                o.OAuthClientId(Config.Swagger.ClientId);
            });
        }

        /// <summary>
        ///     Updates the database.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        private static void UpdateDatabase(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using IServiceScope serviceScope = app.ApplicationServices
                                                  .GetRequiredService<IServiceScopeFactory>()
                                                  .CreateScope();
            using ApplicationDbContext context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

            //Only apply migrations when db is running via MSSQL instead of IN Memory
            if(!context.Database.IsInMemory())
            {
                context.Database.Migrate();
            }

            // Check if Roles and RoleScopes in DB matches seed, if it doesn't match: database is updated.
            SeedHelper.InsertRoles(Seed.SeedRoles(), context);
            List<Role> roles = context.Role.ToList();

            if(!env.IsProduction())
            {
                if(!context.Institution.Any())
                {
                    // Seed institutions
                    context.Institution.AddRange(Seed.SeedInstitution());
                    context.SaveChanges();
                }

                if(!context.User.Any())
                {
                    // seed admin
                    context.User.Add(Seed.SeedAdminUser(roles));
                    context.SaveChanges();

                    //Seed random users
                    context.User.Add(Seed.SeedPrUser(roles));
                    context.User.AddRange(Seed.SeedUsers(roles));
                    context.User.Add(Seed.SeedDataOfficerUser(roles));
                    context.SaveChanges();
                }

                if(!context.Project.Any())
                {
                    //Seed projects
                    List<User> users = context.User.ToList();
                    context.Project.AddRange(Seed.SeedProjects(users));
                    context.SaveChanges();
                }
                if(!context.Collaborators.Any())
                {
                    //seed collaborators
                    List<Project> projects = context.Project.ToList();
                    context.Collaborators.AddRange(Seed.SeedCollaborators(projects));
                    context.SaveChanges();
                }
                if(!context.Highlight.Any())
                {
                    List<Project> projects = context.Project.ToList();
                    context.Highlight.AddRange(Seed.SeedHighlights(projects));
                    context.SaveChanges();
                }

                // TODO seed embedded projects
            }


            // Seed call to action options
            List<CallToActionOption> options = Seed.SeedCallToActionOptions();
            foreach(CallToActionOption callToActionOption in options)
            {
                if(!context.CallToActionOption.Any(s => s.Type == callToActionOption.Type &&
                                                        s.Value == callToActionOption.Value))
                {
                    context.CallToActionOption.Add(callToActionOption);
                    context.SaveChanges();
                }
            }

            if(!context.Category.Any())
            {
                context.Category.AddRange(Seed.SeedCategories());
                context.SaveChanges();
            }

            if(!context.WizardPage.Any())
            {
                context.WizardPage.AddRange(Seed.SeedWizardPages());
                context.SaveChanges();
            }
            if(!context.DataSource.Any())
            {
                context.DataSource.AddRange(Seed.SeedDataSources());
                context.SaveChanges();
            }

            SeedHelper.SeedDataSourceWizardPages(context);
        }

        /// <summary>
        ///     Depending on the environment, will return authorization url based on development identity url or identity url
        /// </summary>
        private Uri GetAuthorizationUrl()
        {
            if(Environment.IsDevelopment())
            {
                return new Uri(Config.IdentityServer.DevelopmentIdentityUrl + "/connect/authorize");
            }
            return new Uri(Config.IdentityServer.IdentityUrl + "/connect/authorize");
        }

    }

}
