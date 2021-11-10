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
using _15_API.Tests.Helpers;

namespace _15_API.Tests.Base
{
    public class TestWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            IHostBuilder builder = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>().UseTestServer();
                })
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettingsapi.Development.json", true, true);
                    config.AddEnvironmentVariables();
                });
            return builder;
        }


        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                //ApplicationDbContext inMemoryContext;
                //DbContextOptions<ApplicationDbContext> options;
                //options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

                //inMemoryContext = new ApplicationDbContext(options);
                //services.AddScoped<ApplicationDbContext>(_ => inMemoryContext);

                //using(var _newContext = new ApplicationDbContext(options))
                //{
                //}

                ServiceDescriptor descriptor = services.SingleOrDefault(d => d.ServiceType ==
                                                                             typeof(DbContextOptions<ApplicationDbContext>));
                
                services.Remove(descriptor);

                var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                                     .UseInMemoryDatabase(Guid.NewGuid().ToString())
                                     .Options;

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                ServiceProvider sp = services.BuildServiceProvider();

                using(IServiceScope scope = sp.CreateScope())
                {
                    IServiceProvider scopedServices = scope.ServiceProvider;
                    ApplicationDbContext db = scopedServices.GetRequiredService<ApplicationDbContext>();
                    ILogger<TestWebApplicationFactory<TStartup>> logger = scopedServices
                        .GetRequiredService<ILogger<TestWebApplicationFactory<TStartup>>>();

                    db.Database.EnsureCreated();

                    try
                    {
                        SeedUtility.InitializeDbForTests(new ApplicationDbContext(contextOptions));
                    }
                    catch(Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the " +
                            "database with test messages. Error: {Message}", ex.Message);
                    }
                }
            });
        }
    }
}
