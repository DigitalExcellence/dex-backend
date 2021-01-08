using IdentityModel.Client;
using MessageBrokerPublisher;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Polly;


namespace JobScheduler
{
    public class Program
    {

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            string environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                                                  .AddJsonFile("appsettingsjobscheduler.json", true, true)
                                                  .AddJsonFile($"appsettingsjobscheduler.{environmentName}.json", true, true)
                                                  .AddEnvironmentVariables();

            IConfiguration configuration = configurationBuilder.Build();
            Config config = new Config(configuration);

            return Host.CreateDefaultBuilder(args)
                       .ConfigureAppConfiguration(builder => configurationBuilder.Build())
                       .ConfigureServices((hostContext, services) =>
                       {
                           services.AddAccessTokenManagement(options =>
                           {
                               options.Client.Clients.Add("identityserver",
                                  new ClientCredentialsTokenRequest
                                  {
                                      Address = config.IdentityServerConfig.IdentityUrl + "connect/token",
                                      ClientId = config.IdentityServerConfig.ClientId,
                                      ClientSecret = config.IdentityServerConfig.ClientSecret
                                  });

                           }).ConfigureBackchannelHttpClient()
                                   .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(new[]
                                    {
                                        TimeSpan.FromSeconds(1),
                                        TimeSpan.FromSeconds(2),
                                        TimeSpan.FromSeconds(3)
                                    }));
                           services.AddClientAccessTokenClient("client", configureClient: client =>
                           {
                               client.BaseAddress = new Uri(config.ApiConfig.ApiUrl);
                           });
                           services.AddScoped<INotificationSender, NotificationSender>();
                           services.AddScoped<IApiRequestHandler, ApiRequestHandler>();
                           services.AddHostedService<GraduationWorker>();

                       });
        }
    }
}
