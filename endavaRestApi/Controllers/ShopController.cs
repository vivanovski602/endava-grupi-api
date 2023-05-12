using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using endavaRestApi.Data;
using endavaRestApi.Repositories;
using log4net;

namespace endavaRestApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase

    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(ShopController));
        private readonly IShopRepository _shopRepository;
        public ShopController(IShopRepository shopRepository)
        {
            _shopRepository = shopRepository;
            log4net.Config.XmlConfigurator.Configure();
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> GetProducts()
        {
           
                _logger.Info("Getting products...");
                return await _shopRepository.Get();
            
         }

        [HttpPost("filter")]
        public async Task<ActionResult<Product>> Filter([FromBody] ProductFilter filter)
        {
            var results = await _shopRepository.Filter(filter);
            return Ok(results);
        }



        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            var createdUser = await _shopRepository.AddUser(user);
            _logger.Info($"User created: {createdUser.Id}");

            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);


        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            _logger.Info($"Getting user with id {id}");
            _logger.Info($"User with id {id} retrieved successfully");
            return await _shopRepository.Get(id);
        }


    }


}
