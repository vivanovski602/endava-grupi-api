using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using endavaRestApi.Data;
using endavaRestApi.Repositories;
using log4net;
using log4net.Config;

namespace endavaRestApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase

    {
        private readonly IShopRepository _shopRepository;
        private readonly IOrderRepository _orderRepository;
        public ShopController(IShopRepository shopRepository, IOrderRepository orderRepository)
        {
            _shopRepository = shopRepository;
            XmlConfigurator.Configure(new FileInfo("log4net.config"));
            _orderRepository = orderRepository;
        }

        [HttpGet("products/Get-All")]
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _shopRepository.Get();
        }

        [HttpPost("products/filter")]
        public async Task<ActionResult<Product>> Filter([FromBody] ProductFilter filter)
        {
            var results = await _shopRepository.Filter(filter);
            return Ok(results);
        }


        [HttpGet("product/{category}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(string category)
        {
            var products = await _shopRepository.GetByCategory(category);
            return Ok(products);

        }






        [HttpPost("orders")]
        public async Task<IActionResult> CreateOrder(int userId, Dictionary<int, int> productQuantities)
        {
            var (order, errorResult) = await _orderRepository.CreateOrder(userId, productQuantities);
            if (errorResult != null)
            {
                return errorResult;
            }

            var payment = await _orderRepository.CreatePayment(order.OrderId);

            return Ok(new { Order = order, Payment = payment });
        }

        
     
            
        [HttpPost("import-csv-file")]
        public async Task<IActionResult> ImportProducts(IFormFile file)
        {
            await _shopRepository.ImportCsv(file);
            return Ok();
        }
        [HttpGet("payment-details")]
        public async Task<IActionResult> GetMatchingPaymentDetailsAsync(int userId, string productName)
        {
            var result = await _orderRepository.GetMatchingPaymentDetailsAsync(userId, productName);
            return Ok(result);
        }
    }
    } 






