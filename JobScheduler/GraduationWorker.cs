using IdentityModel.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Net.Http;

namespace JobScheduler
{
    public class GraduationWorker : BackgroundService
    {
        private readonly ILogger<GraduationWorker> _logger;
        public GraduationWorker(ILogger<GraduationWorker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                var client = new HttpClient();
                var identityServerResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                                                 {
                                                     Address = "https://localhost:5005/connect/token",

                                                     ClientId = "dex-jobscheduler",
                                                     ClientSecret = "dex-jobscheduler",
                                                     Scope = "dex-api",
                                                 });
                if (identityServerResponse.IsError)
                {
                    _logger.LogError("Something went wrong: " + identityServerResponse.ErrorDescription);
                } else
                {
                    _logger.LogInformation(identityServerResponse.AccessToken);
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
