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
