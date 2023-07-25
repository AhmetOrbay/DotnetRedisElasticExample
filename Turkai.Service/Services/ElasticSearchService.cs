using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Turkai.Model.Entity;
using Turkai.Model.ExtensionModel.ElasticSearch;
using Turkai.Service.BackgroundServices;
using Turkai.Service.Interfaces;

namespace Turkai.Service.Services
{
    public class ElasticSearchService : IElasicSearchService
    {
        private readonly IElasticClient _elasticClient;
        private readonly ILogger<ReadRabbitMqWorker> _logger;

        public ElasticSearchService(IElasticClient elasticClient,
                        ILogger<ReadRabbitMqWorker> logger)
        {
            _elasticClient = elasticClient;
            _logger = logger;
        }

        /// <summary>
        /// It is Fetching products from Elasticsearch
        /// </summary>
        /// <param name="Index"></param>
        /// <returns></returns>
        public async Task<List<ElasticImportModel>> GetElasticSearchData(ElasticSearchIndex Index)
        {
            var response = await _elasticClient.SearchAsync<ElasticImportModel>(s => s
            .Index(Index.ToString()));

            if (response.IsValid) return response.Documents.ToList();
            else
            {
                _logger.LogError("not Found Products");
                return new List<ElasticImportModel>();
            }
        }

        /// <summary>
        /// Adding to Elasticsearch
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task ImportElasticProduct(List<ElasticImportModel> data)
        {
            List<ElasticImportModel> list = new();
            var descriptor = new BulkDescriptor();
            foreach (var d in data)
            {
                descriptor.Index<ElasticImportModel>(i => i
                    .Index(ElasticSearchIndex.productlist.ToString())
                    .Document(d)
                );
            }
            var bulkResponse = await _elasticClient.BulkAsync(descriptor);

            if (!bulkResponse.IsValid)  _logger.LogError($"Hata: {bulkResponse.ServerError?.Error?.Reason}");
            else _logger.LogError("Data successfully indexed.");
            
        }

    }
}
