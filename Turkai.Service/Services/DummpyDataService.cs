using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Turkai.Model.ExtensionModel.DummpyDataModel;
using Turkai.Service.BackgroundServices;
using Turkai.Service.Extensions;
using Turkai.Service.Interfaces;

namespace Turkai.Service.Services
{
    public class DummpyDataService : IDummpyDataService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ReadRabbitMqWorker> _logger;
        public DummpyDataService(HttpClient httpClient, ILogger<ReadRabbitMqWorker> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<string> GetDummpyData()
        {
            try
            {
                var Urls = new DummpyUrl();
                var response = await _httpClient.GetAsync(Urls.Url);
                response.EnsureSuccessStatusCode();
                var stringData = await response.Content.ReadAsStringAsync();
                return stringData;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Dummpy Data get {ex.Message}");
                return string.Empty;
            }
        }
    }
}
