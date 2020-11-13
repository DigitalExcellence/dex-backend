using IdentityModel.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Net.Http;
using System.Collections.Generic;
using Models;

namespace JobScheduler
{
    public class GraduationWorker : BackgroundService
    {
        private readonly ILogger<GraduationWorker> logger;
        private List<UserTask> users;

        private readonly ApiRequestHandler requestHandler;
        public GraduationWorker(ILogger<GraduationWorker> logger)
        {
            this.logger = logger;
            requestHandler = new ApiRequestHandler(new Uri("https://localhost:5001/"));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            users = requestHandler.GetExpectedGraduationUsers();

            while(!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                logger.LogInformation(users[0].Type.ToString());

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
