using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp.Common;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Models;
using Services.Services;

namespace JobScheduler
{
    public class GraduationWorker : BackgroundService
    {
#pragma warning disable IDE1006 // Naming Styles
        private readonly ILogger<GraduationWorker> _logger;
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        private readonly ICallToActionService _service;
#pragma warning restore IDE1006 // Naming Styles
        public GraduationWorker(ILogger<GraduationWorker> logger, ICallToActionService service)
        {
            _logger = logger;
            _service = service;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            List<CallToAction> callToActions = _service.GetAllGraduateCallToActions();
            while(!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
