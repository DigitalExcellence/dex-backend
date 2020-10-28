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

        private readonly ApiRequestHandler requestHandler;
        public GraduationWorker(ILogger<GraduationWorker> logger)
        {
            _logger = logger;
            requestHandler = new ApiRequestHandler(new Uri("https://test.com/"), "test");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                _logger.LogInformation(requestHandler.GetToken());


                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
