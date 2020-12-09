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
    public class DeleteProjectWorker : BackgroundService
    {
        private readonly ILogger<DeleteProjectWorker> _logger;
        private readonly string subject = "ELASTIC_DELETE";
        private readonly IConfig config;

        public DeleteProjectWorker(ILogger<DeleteProjectWorker> logger)
        {
            _logger = logger;
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
