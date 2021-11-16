using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.Extensions.Hosting;
using API;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using API.Tests.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace API.Tests.Base
{
    public class TestWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            IHostBuilder builder = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>().UseTestServer();
                    //webBuilder.UseUrls("http://localhost:5001/api/");
                    webBuilder.UseKestrel();
                })  
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettingsapi.Development.json", true, true);
                    config.AddEnvironmentVariables();
                });
            return builder;
        }


        //protected virtual void ConfigureServices(IServiceCollection services)
        //{
        //    //services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
        //    //{
        //    //    var config = new OpenIdConnectConfiguration()
        //    //    {
        //    //        Issuer = MockJwtTokens.Issuer
        //    //    };

        //    //    config.SigningKeys.Add(MockJwtTokens.SecurityKey);
        //    //    options.Configuration = config;
        //    //});
        //}


        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            //builder.ConfigureTestServices(ConfigureServices);
            builder.ConfigureLogging((WebHostBuilderContext context, ILoggingBuilder loggingBuilder) =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddConsole(options => options.IncludeScopes = true);
            });


            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<ApplicationDbContext>));

                services.Remove(descriptor);

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                //services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
                //{
                //    var config = new OpenIdConnectConfiguration()
                //    {
                //        Issuer = MockJwtTokens.Issuer
                //    };

                //    config.SigningKeys.Add(MockJwtTokens.SecurityKey);
                //    options.Configuration = config;
                //});


                var sp = services.BuildServiceProvider();

                using(var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ApplicationDbContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<TestWebApplicationFactory<TStartup>>>();

                    db.Database.EnsureCreated();

                    try
                    {
                        SeedUtility.InitializeDbForTests(db);
                    } catch(Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the " +
                            "database with test messages. Error: {Message}", ex.Message);
                    }
                }
            });
        }
        //protected override void ConfigureWebHost(IWebHostBuilder builder)
        //{
        //    builder.ConfigureServices(services =>
        //    {
        //        ServiceDescriptor descriptor = services.SingleOrDefault(d => d.ServiceType ==
        //                                                                     typeof(DbContextOptions<ApplicationDbContext>));

        //        services.Remove(descriptor);
        //        string databaseName = Guid.NewGuid().ToString();
        //        //DbContextOptions<ApplicationDbContext> contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>().Options;

        //        services.AddDbContext<ApplicationDbContext>(contextOptions =>
        //        {
        //            contextOptions.UseInMemoryDatabase(databaseName);
        //        });

        //        ServiceProvider sp = services.BuildServiceProvider();
        //        using(IServiceScope scope = sp.CreateScope())
        //        {
        //            IServiceProvider scopedServices = scope.ServiceProvider;
        //            ApplicationDbContext db = scopedServices.GetRequiredService<ApplicationDbContext>();
        //            ILogger<TestWebApplicationFactory<TStartup>> logger = scopedServices
        //                .GetRequiredService<ILogger<TestWebApplicationFactory<TStartup>>>();

        //            db.Database.EnsureCreated();

        //            try
        //            {
        //                SeedUtility.InitializeDbForTests(db);
        //            }
        //            catch(Exception ex)
        //            {
        //                logger.LogError(ex, "An error occurred seeding the " +
        //                    "database with test messages. Error: {Message}", ex.Message);
        //            }
        //        }
        //    });
        //}
    }
}
