using ElasticSynchronizer.Configuration;
using ElasticSynchronizer.Workers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace ElasticSynchronizer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>


            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    string environmentName = Environment.GetEnvironmentVariable("ELASTIC_DOTNET_ENVIRONMENT");
                    IConfiguration configuration = new ConfigurationBuilder()
                                                   .AddJsonFile("appsettings.json", true, true)
                                                   .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                                                   .AddEnvironmentVariables()
                                                   .Build();

                    services.AddScoped<Config>( c => configuration.GetSection("App")
                                                                  .Get<Config>()  );

                    services.AddHostedService<DeleteProjectWorker>();
                    services.AddHostedService<UpdateProjectWorker>();
                });
    }
}
