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

using ElasticSynchronizer.Configuration;
using ElasticSynchronizer.Executors;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotificationSystem.Contracts;
using NotificationSystem.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading;
using System.Threading.Tasks;

namespace ElasticSynchronizer.Workers
{
    public class UpdateProjectWorker : BackgroundService
    {
        private readonly ILogger<UpdateProjectWorker> _logger;
        private readonly string subject = "ELASTIC_CREATE_OR_UPDATE";
        private readonly Config config;

        public UpdateProjectWorker(ILogger<UpdateProjectWorker> logger, Config config)
        {
            _logger = logger;
            this.config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            RabbitMQSubscriber subscriber = new RabbitMQSubscriber(config.RabbitMQ.Hostname, config.RabbitMQ.Username, config.RabbitMQ.Password);
            IModel channel = subscriber.SubscribeToSubject(subject);
            RabbitMQListener listener = new RabbitMQListener(channel);


            INotificationService notificationService = new DocumentUpdater(config);
            EventingBasicConsumer consumer = listener.CreateConsumer(notificationService);

            listener.StartConsumer(consumer, subject);
        }
    }
}
