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
    public class AlgorithmWorker : BackgroundService
    {

        private readonly Config config;
        private readonly ILogger<AlgorithmWorker> logger;
        private readonly IApiRequestHandler requestHandler;

        /// <summary>
        ///     This is the constructor of the worker service
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="apiRequestHandler"></param>
        /// <param name="config"></param>
        public AlgorithmWorker(ILogger<AlgorithmWorker> logger,
                                IApiRequestHandler apiRequestHandler,
                                Config config)
        {
            this.logger = logger;
            requestHandler = apiRequestHandler;
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
                AlgorithmActivityJob();

                // Time between job.
                await Task.Delay(config.JobSchedulerConfig.TimeBetweenJobsInMs, stoppingToken);
            }
        }

        /// <summary>
        ///    Executes the request for the activity algorithm
        /// </summary>
        /// <returns></returns>
        private void AlgorithmActivityJob()
        {
            try
            {
                requestHandler.SetActivityAlgorithmScore();

            } catch(Exception e)
            {
                logger.LogCritical(e.InnerException + " " + e.Message);
            }
        }

    }

}
