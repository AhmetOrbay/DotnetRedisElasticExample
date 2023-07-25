using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Turkai.Data;
using Turkai.Model.Dtos;
using Turkai.Model.Entity;
using Turkai.Service.BackgroundServices;
using Turkai.Service.Interfaces;

namespace Turkai.Service.Services
{
    public class ProductService : IProductService
    {
        private readonly TurkaiDbContext _turkaiDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<ReadRabbitMqWorker> _logger;


        public ProductService(TurkaiDbContext turkaiDbContext, IMapper mapper, 
                    ILogger<ReadRabbitMqWorker> logger)
        {
            _turkaiDbContext = turkaiDbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Added(Product entity)
        {
            try
            {
                if (entity is not null)  await _turkaiDbContext.Products.AddAsync(entity);
                else  _logger.LogError($"Added product Error");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Added product Error : {ex.Message}");
            }
        }

        public async Task<List<ProductDto>> GetAll()
        {
            try
            {
                var data = await _turkaiDbContext.Products.ToListAsync();
                return _mapper.Map<List<ProductDto>>(data);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Get products error : {ex.Message}");
                throw new Exception($"Get products error : {ex.Message}");
            }
        }

        public async Task<ProductDto> GetById(long Id)
        {
            try
            {
                var data =  await _turkaiDbContext.Products
                                    .FirstOrDefaultAsync(x => x.Id == Id);
                return _mapper.Map<ProductDto>(data) ?? new ProductDto();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Get by Id Error : {ex.Message}");
                throw new Exception($"Get by Id Error : {ex.Message}");
            }
        }
    }
}
