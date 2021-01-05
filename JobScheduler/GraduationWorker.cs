using MessageBrokerPublisher;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Models;
using NotificationSystem.Notifications;

namespace JobScheduler
{
    public class GraduationWorker : BackgroundService
    {
        private readonly ILogger<GraduationWorker> logger;
        private readonly IApiRequestHandler requestHandler;
        private readonly INotificationSender notificationSender;

        public GraduationWorker(ILogger<GraduationWorker> logger,
                                IApiRequestHandler apiRequestHandler,
                                INotificationSender notificationSender)
        {
            this.logger = logger;
            requestHandler = apiRequestHandler;
            this.notificationSender = notificationSender;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Wait till API and Identity are started
            await Task.Delay(10000);

            while(!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("Graduation job started: {time}", DateTimeOffset.Now);
                GraduationJob();
                logger.LogInformation("Graduation job finished: {time}", DateTimeOffset.Now);

                // Time between job. 
                await Task.Delay(10000, stoppingToken);
            }
        }

        private void GraduationJob()
        {
            try
            {
                List<UserTask> userTasks = requestHandler.GetExpectedGraduationUsers();

                if(userTasks != null)
                {
                    foreach(UserTask userTask in userTasks)
                    {
                        EmailNotification notification = new EmailNotification(userTask.User.Email, "User graduation mail here,no template yet");
                        notificationSender.RegisterNotification(Newtonsoft.Json.JsonConvert.SerializeObject(notification), Subject.EMAIL);

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
