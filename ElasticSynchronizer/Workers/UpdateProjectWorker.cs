using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotificationSystem.Contracts;
using NotificationSystem.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ElasticSynchronizer
{
    public class UpdateProjectWorker : BackgroundService
    {
        private readonly ILogger<UpdateProjectWorker> _logger;
        private readonly string subject = "ELASTIC_CREATE_OR_UPDATE";
        private readonly IConfig config;

        public UpdateProjectWorker(ILogger<UpdateProjectWorker> logger, IConfig config)
        {
            _logger = logger;
            this.config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            RabbitMQSubscriber subscriber = new RabbitMQSubscriber(config.RabbitMQ.Hostname, config.RabbitMQ.Username, config.RabbitMQ.Password);
            IModel channel = subscriber.SubscribeToSubject(subject);
            RabbitMQListener listener = new RabbitMQListener(channel);


            INotificationService notificationService = new DocumentUpdater();
            EventingBasicConsumer consumer = listener.CreateConsumer(notificationService);

            listener.StartConsumer(consumer, subject);
        }
    }
}
