using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Models;

namespace JobScheduler
{
    public class GraduationWorker : BackgroundService
    {
        private readonly ILogger<GraduationWorker> logger;
        private readonly IApiRequestHandler requestHandler;

        public GraduationWorker(ILogger<GraduationWorker> logger, IApiRequestHandler apiRequestHandler)
        {
            this.logger = logger;
            requestHandler = apiRequestHandler;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                List<UserTask> userTasks = requestHandler.GetExpectedGraduationUsers();
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                if(userTasks != null)
                {
                    foreach(UserTask user in userTasks)
                    {
                        logger.LogInformation("Found expected graduating user: " + user.UserId);
                    }
                }

                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
