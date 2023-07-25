using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Turkai.Model.ExtensionModel.Enums;
using Turkai.Service.Extensions;
using Turkai.Service.Interfaces;

namespace Turkai.Service.BackgroundServices
{
    /// <summary>
    /// Background service get dummpy data
    /// </summary>
    public class GetDataWorkerService : BackgroundService
    {
        private readonly ILogger<ReadRabbitMqWorker> _logger;
        private readonly IDummpyDataService _dummpyData;
        private readonly IRabbitMQService _rabbitMQService;
        private static string DataHash = string.Empty;

        public GetDataWorkerService(IDummpyDataService dummpyData
            , IRabbitMQService rabbitMQService,
                    ILogger<ReadRabbitMqWorker> logger)
        {
            _dummpyData = dummpyData;
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
                    var data = await _dummpyData.GetDummpyData();
                    var dataNewHash = data.GetBase64();
                    if (!DataHash.Equals(dataNewHash))
                    {
                        if (!string.IsNullOrEmpty(data)) await Task.Run(() => _rabbitMQService.WriteRabbitMq(data, Routing.routingKey));
                        DataHash = dataNewHash;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"get data error : {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
