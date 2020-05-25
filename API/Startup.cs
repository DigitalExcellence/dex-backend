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
using Data;
using Data.Helpers;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Models;
using Models.Defaults;
using Services.Services;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace API
{

    /// <summary>
    ///     Startup file
    /// </summary>
    public class Startup
    {

        /// <summary>
        ///     Constructor for Startup file
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Config = configuration.GetSection("App")
                                  .Get<Config>();
            Config.OriginalConfiguration = configuration;
            Environment = environment;
        }

        /// <summary>
        ///     Config file of API
        /// </summary>
        public Config Config { get; }

        /// <summary>
        ///     Environment of the API
        /// </summary>
        public IWebHostEnvironment Environment { get; }

        /// <summary>
        ///     Configure Services
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;
            services.AddDbContext<ApplicationDbContext>(o =>
            {
                o.UseSqlServer(Config.OriginalConfiguration.GetConnectionString("DefaultConnection"),
                               sqlOptions => { sqlOptions.EnableRetryOnFailure(50, TimeSpan.FromSeconds(30), null); });
            });
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
                o.AddPolicy(nameof(Defaults.Scopes.HighlightRead),
                    policy => policy.Requirements.Add(new ScopeRequirement(nameof(Defaults.Scopes.HighlightRead))));
                o.AddPolicy(nameof(Defaults.Scopes.HighlightWrite),
                    policy => policy.Requirements.Add(new ScopeRequirement(nameof(Defaults.Scopes.HighlightWrite))));

                o.AddPolicy(nameof(Defaults.Scopes.ProjectRead),
                    policy => policy.Requirements.Add(new ScopeRequirement(nameof(Defaults.Scopes.ProjectRead))));
                o.AddPolicy(nameof(Defaults.Scopes.ProjectWrite),
                    policy => policy.Requirements.Add(new ScopeRequirement(nameof(Defaults.Scopes.ProjectWrite))));

                o.AddPolicy(nameof(Defaults.Scopes.UserRead),
                    policy => policy.Requirements.Add(new ScopeRequirement(nameof(Defaults.Scopes.UserRead))));
                o.AddPolicy(nameof(Defaults.Scopes.UserWrite),
                    policy => policy.Requirements.Add(new ScopeRequirement(nameof(Defaults.Scopes.UserWrite))));

                o.AddPolicy(nameof(Defaults.Scopes.RoleRead),
                    policy => policy.Requirements.Add(new ScopeRequirement(nameof(Defaults.Scopes.RoleRead))));
                o.AddPolicy(nameof(Defaults.Scopes.RoleWrite),
                    policy => policy.Requirements.Add(new ScopeRequirement(nameof(Defaults.Scopes.RoleWrite))));
                    
                o.AddPolicy(nameof(Defaults.Scopes.EmbedRead),
                    policy => policy.Requirements.Add(new ScopeRequirement(nameof(Defaults.Scopes.EmbedRead))));
                o.AddPolicy(nameof(Defaults.Scopes.EmbedWrite),
                    policy => policy.Requirements.Add(new ScopeRequirement(nameof(Defaults.Scopes.EmbedWrite))));

            });

            services.AddCors();
            services.AddControllersWithViews()
                    .AddFluentValidation(c => c.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddSwaggerGen(o =>
            {
                o.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Dex API",
                        Version = "v1",
                        License = new OpenApiLicense
                        {
                            Name = "GNU Lesser General Public License v3.0",
                            Url = new Uri("https://www.gnu.org/licenses/lgpl-3.0.txt")
                        }
                    });
                o.IncludeXmlComments($@"{AppDomain.CurrentDomain.BaseDirectory}{typeof(Startup).Namespace}.xml");
                o.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(Config.IdentityServer.IdentityUrl + "/connect/authorize"),
                            Scopes = new Dictionary<string, string>
                            {
                                { "ProjectWrite", "There is a scope needed" },

                            }
                        }
                    }
                });
                o.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                        },
                        new[] { "" }
                    }
                });

            });

            // Add application services.
            services.AddSingleton(Config);
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
            

            UpdateDatabase(app,env);
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
                app.UseExceptionHandler();
            }

            app.UseProblemDetails();

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


            //StudentInfo
            app.UseWhen(context =>
                context.User.Identities.Any(i => i.IsAuthenticated), appBuilder =>
                {
                    appBuilder.Use(async (context, next) =>
                    {
                        DbContext dbContext = context.RequestServices.GetService<DbContext>();
                        IUserService userService =
                            context.RequestServices.GetService<IUserService>();
                        string studentId = context.User.GetStudentId(context);
                        if(await userService.GetUserByIdentityIdAsync(studentId) == null)
                        {
                            User newUser = context.GetUserInformationAsync(Config);
                            if(newUser == null)
                            {
                                // Then it probably belongs swagger so we set the username as developer.
                                newUser = new User()
                                {
                                    Name = "Developer",
                                    Email = "Developer@DEX.com",
                                    IdentityId = studentId
                                };
                                userService.Add(newUser);

                            } else
                            {
                                userService.Add(newUser);
                            }
                            await dbContext.SaveChangesAsync();
                        }

                        await next();
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

            app.UseStaticFiles();
        }

        /// <summary>
        /// Initializes the database
        /// </summary>
        /// <param name="app"></param>
        private static void UpdateDatabase(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using(IServiceScope serviceScope = app.ApplicationServices
                                                  .GetRequiredService<IServiceScopeFactory>()
                                                  .CreateScope())
            {
                using(ApplicationDbContext context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
                {
                    context.Database.Migrate();
                    if(!context.Role.Any())
                    {
                        // seed roles
                        context.AddRange(Seed.SeedRoles());
                        context.SaveChanges();
                    }
                    if(!context.User.Any())
                    {
                        // seed admin
                        List<Role> roles = context.Role.ToList();
                        context.User.Add(Seed.SeedAdminUser(roles));
                        context.SaveChanges();

                        if(!env.IsProduction())
                        {
                            //Seed random users
                            context.User.Add(Seed.SeedPrUser(roles));
                            context.User.AddRange(Seed.SeedUsers());
                            context.SaveChanges();
                        }
                    }

                    if(!env.IsProduction())
                    {
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
                }
            }
        }
    }

}
