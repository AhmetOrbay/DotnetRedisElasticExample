using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turkai.Service.Interfaces;

namespace Turkai.Service.BackgroundServices
{
    public class ReadRabbitMqWorker : BackgroundService
    {
        private readonly ILogger<ReadRabbitMqWorker> _logger;
        private readonly IRabbitMQService _rabbitMQService;

        public ReadRabbitMqWorker(IRabbitMQService rabbitMQService, 
            ILogger<ReadRabbitMqWorker> logger)
        {
            _rabbitMQService = rabbitMQService;
            _logger = logger;
        }


        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogError("fasfasas");
                try
                {
                    await _rabbitMQService.ReadRabbiMQ(Model.ExtensionModel.Enums.RabbitMQEnum.DummpyKey);

                }
                catch (Exception ex)
                {
                    throw new Exception($"get rabbit error : {ex.Message}");

                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
