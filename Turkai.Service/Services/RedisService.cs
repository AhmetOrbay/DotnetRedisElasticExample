using AutoMapper;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turkai.Model.Dtos;
using Turkai.Model.Entity;
using Turkai.Service.BackgroundServices;
using Turkai.Service.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Turkai.Service.Services
{
    public class RedisService : IRedisService
    {
        private readonly IConnectionMultiplexer _redisDb;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly ILogger<ReadRabbitMqWorker> _logger;

        public RedisService(IConnectionMultiplexer redis,
                IProductService productService,
                IMapper mapper,
                ILogger<ReadRabbitMqWorker> logger)
        {
            _redisDb = redis;
            _productService = productService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Read Redis
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<ProductDto> GetDataRedis(long Id)
        {
            try
            {
                var database = _redisDb.GetDatabase();
                var GetRedisData = await database.StringGetAsync($"product-{Id}");
                if (GetRedisData.HasValue)
                {
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ProductDto>(GetRedisData.ToString());
                    if (result is not null)
                        return result;
                }
                var data = await _productService.GetById(Id);
                await ImportProduct(data.Id);
                return data ?? new ProductDto();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cash read Error {ex.Message}");
                return new ProductDto();
            }
           
        }
        /// <summary>
        /// Redis importing
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<bool> ImportProduct(long productId)
        {
            try
            {
                var Model = await _productService.GetById(productId);
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(_mapper.Map<ProductDto>(Model));
                var database = _redisDb.GetDatabase();
                var result = await database.StringSetAsync($"product-{Model.Id}", json);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cash write Error {ex.Message}");
                return false;
            }
        }
    }
}
