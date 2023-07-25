using Microsoft.AspNetCore.Mvc;
using Nest;
using System.Diagnostics;
using System.Text.Json.Nodes;
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
                var data = await _elasicSearchService.GetElasticSearchData(ElasticSearchIndex.productlist);
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
                return BadRequest($"List  {ex.Message}");
            }
        }


        public async Task<IActionResult> BasketCheck(List<BasketDetail> basketDetail)
        {
            var resultFalse = new CheckStockModel() { CheckStock = true, Title = string.Empty };

            try
            {
                foreach (var item in basketDetail)
                {
                    var data = await _redisService.GetDataRedis(item.Id);
                    if (data.Stock < item.Count) resultFalse = new CheckStockModel() { CheckStock = false, Title = item.Title };
                }
                return Json(resultFalse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"List  {ex.Message}");
                return Json(resultFalse);
            }
        }


        public async Task<IActionResult> BasketDetail()
        {
            return View();
        }
    }
}