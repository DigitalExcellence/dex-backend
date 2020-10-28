using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models;

namespace JobScheduler
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
                    services.AddHostedService<GraduationWorker>();
                });
    }
}
