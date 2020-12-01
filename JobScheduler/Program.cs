using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models;
using System;
using System.Threading.Tasks;

namespace JobScheduler
{
    public class Program
    {

        public static void Main(string[] args)
        {
            Task.Delay(7500);

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddScoped<IConfig, Config>();
                    services.AddScoped<IApiRequestHandler, ApiRequestHandler>();
                    services.AddHostedService<GraduationWorker>();
                });
    }
}
