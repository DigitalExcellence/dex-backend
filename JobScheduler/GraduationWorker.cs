/*
* Digital Excellence Copyright (C) 2020 Brend Smits
* 
* This program is free software: you can redistribute it and/or modify 
* it under the terms of the GNU Lesser General Public License as published 
* by the Free Software Foundation version 3 of the License.
* 
* This program is distributed in the hope that it will be useful, 
* but WITHOUT ANY WARRANTY; without even the implied warranty 
* of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
* See the GNU Lesser General Public License for more details.
* 
* You can find a copy of the GNU Lesser General Public License 
* along with this program, in the LICENSE.md file in the root project directory.
* If not, see https://www.gnu.org/licenses/lgpl-3.0.txt
*/

using MessageBrokerPublisher.HelperClasses;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace JobScheduler
{

    /// <summary>
    ///     This is the worker service which is responsible for running graduation related jobs.
    /// </summary>
    public class GraduationWorker : BackgroundService
    {

        private readonly Config config;
        private readonly IEmailSender emailSender;
        private readonly ILogger<GraduationWorker> logger;
        private readonly IApiRequestHandler requestHandler;

        /// <summary>
        ///     This is the constructor of the worker service
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="apiRequestHandler"></param>
        /// <param name="emailSender"></param>
        /// <param name="config"></param>
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

        /// <summary>
        ///     This is the asynchronous method which executes the job.
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
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

        /// <summary>
        ///     This is the graduation job. It requests expecting graduating users and sends them an email to inform them.
        /// </summary>
        /// <returns></returns>
        private async Task GraduationJob()
        {
            try
            {
                List<UserTask> userTasks = await requestHandler.GetExpectedGraduationUsersAsync();

                if(userTasks != null)
                {
                    foreach(UserTask userTask in userTasks)
                    {
                        emailSender.Send(userTask.User.Email,
                                         "DeX: you are graduating, please convert your account.",
                                         null);
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
