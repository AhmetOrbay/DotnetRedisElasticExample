using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Turkai.Data;
using Turkai.Data.RabbitMq;
using Turkai.Model.Entity;
using Turkai.Model.ExtensionModel.DummpyDataModel;
using Turkai.Model.ExtensionModel.ElasticSearch;
using Turkai.Model.ExtensionModel.Enums;
using Turkai.Service.BackgroundServices;
using Turkai.Service.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Turkai.Service.Services
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly IConnection _connection;
        private readonly IRabbitMqRepo _rabbitMqDb;
        private readonly IMapper _mapper;
        private readonly IElasicSearchService _elasicSearchService;
        private readonly ILogger<ReadRabbitMqWorker> _logger;
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(5);
        public RabbitMQService(IConnection connection,
                        IRabbitMqRepo rabbitMqDb,
                        IMapper mapper,
                        IElasicSearchService elasicSearchService,
                        ILogger<ReadRabbitMqWorker> logger)
        {
            _connection = connection;
            _rabbitMqDb = rabbitMqDb;
            _mapper = mapper;
            _elasicSearchService = elasicSearchService;
            _logger = logger;
        }

        /// <summary>
        /// Reading data from Rabbitmq service and importing elasticsearch
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task ReadRabbiMQ(RabbitMQEnum key)
        {

            using (var channel = _connection.CreateModel())
            {
                string queueName = RabbitMQEnum.DummpyKey.ToString();
                channel.QueueDeclare(queueName, true, false, false, null);
                var consumer = new EventingBasicConsumer(channel);
                var aasda = string.Empty;
                consumer.Received +=async (model, ea) =>
                {
                    var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var modelProduct = JsonConvert.DeserializeObject<DummpyResponse>(message);
                    if (modelProduct is not null && modelProduct.Products.Any())
                    {
                        var modelP = _mapper.Map<List<Product>>(modelProduct.Products);
                        await semaphore.WaitAsync();
                        try
                        {
                            //RabbitMQ to DB
                            await _rabbitMqDb.WriteDbContext(modelP);
                            var modelElastic = modelP.Select(x => new ElasticImportModel()
                            {
                                Id = x.Id,
                                Title = x.Title
                            }).ToList();

                            //import elasticSearch
                            await _elasicSearchService.ImportElasticProduct(modelElastic);
                        }
                        catch(Exception ex)
                        {
                            _logger.LogError($"RABBITMQ read Error {ex.Message}");
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    }
                };
                channel.BasicConsume(queueName, true, consumer);
            }
            await Task.CompletedTask;
        }

        public async Task WriteRabbitMq(string product, Routing Routing)
        {
            using (var channel = _connection.CreateModel())
            {
                try
                {
                    string exchangeName = RabbitMQEnum.DummpyKey.ToString();
                    string routingKey = Routing.ToString();
                    var body = Encoding.UTF8.GetBytes(product);
                    channel.BasicPublish(exchangeName, routingKey, null, body);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"RABBITMQ write Error {ex.Message}");
                }
            }
            await Task.CompletedTask;
        }
    }
}
