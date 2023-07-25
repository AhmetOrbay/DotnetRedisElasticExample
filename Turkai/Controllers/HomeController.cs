using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Turkai.Model.Dtos;
using Turkai.Model.ExtensionModel.ElasticSearch;
using Turkai.Models;
using Turkai.Service.Interfaces;

namespace Turkai.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IElasicSearchService _elasicSearchService;
        private readonly IRedisService _redisService;
        public HomeController(ILogger<HomeController> logger,
            IElasicSearchService elasicSearchService,
            IRedisService redisService)
        {
            _logger = logger;
            _elasicSearchService = elasicSearchService;
            _redisService = redisService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var data = await _elasicSearchService.GetElasticSearchData<ElasticImportModel>(ElasticSearchIndex.productlist);
                return View(data);
            }
            catch (Exception ex)
            {
                _logger.LogError("ssss");
                return BadRequest($"List  {ex.Message}");
            }
            
        }

        [HttpGet("Home/Detail/{Id}")]
        public async Task<IActionResult> Detail(long Id)
        {
            try
            {
                var data = await _redisService.GetDataRedis(Id);
                return View(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"List  {ex.Message}");
                return BadRequest($"List  {ex.Message}");
            }
        }
    }
}