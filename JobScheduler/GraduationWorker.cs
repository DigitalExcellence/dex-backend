using MessageBrokerPublisher.HelperClasses;
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
        private readonly IEmailSender emailSender;
        private readonly Config config;

        public GraduationWorker(ILogger<GraduationWorker> logger,
                                IApiRequestHandler apiRequestHandler,
                                IEmailSender emailSender,
                                Config config)
        {
            
            this.logger = logger;
            requestHandler = apiRequestHandler;
            this.emailSender = emailSender;
            this.config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Wait till API and Identity are started
            await Task.Delay(20000);

            while(!stoppingToken.IsCancellationRequested)
            {
                await GraduationJob();

                // Time between job. 
                await Task.Delay(config.JobSchedulerConfig.TimeBetweenJobsInMs, stoppingToken);
            }
        }

        private async Task GraduationJob()
        {
            try
            {
                List<UserTask> userTasks = await requestHandler.GetExpectedGraduationUsersAsync();

                if(userTasks != null)
                {
                    foreach(UserTask userTask in userTasks)
                    {
                        emailSender.Send(userTask.User.Email, "test", "test");
                        
                        logger.LogInformation("Found expected graduating user: " + userTask.User.Id);
                        requestHandler.SetGraduationTaskStatusToMailed(userTask.Id);
                    }
                }
            } catch(Exception e)
            {
                logger.LogCritical(e.InnerException + " " + e.Message);
            }
        }
    }
}
