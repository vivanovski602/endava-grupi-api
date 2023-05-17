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
        public ShopController(IShopRepository shopRepository)
        {
            _shopRepository = shopRepository;
            XmlConfigurator.Configure(new FileInfo("log4net.config"));
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> GetProducts()
        {
           
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
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);


        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
           
            return await _shopRepository.Get(id);
        }

        [HttpGet("category/{category}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(string category)
        {
            var products = await _shopRepository.GetByCategory(category);
            return Ok(products);
        }
    }


}
