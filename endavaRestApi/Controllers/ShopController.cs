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

        /*[HttpPost("filter")]
   
        public async Task<ActionResult<Product>> Filter([FromBody] ProductFilter filter)
        {
            var products = await _shopRepository.Get();
            var results = products.Where(p =>       //da se iskomentira
                    (filter.ProductCategory == null || p.ProductCategory == filter.ProductCategory) &&
                    (filter.ProductBrand == null || p.ProductBrand == filter.ProductBrand) &&
                    (filter.PriceMin == null || p.Price >= filter.PriceMin) &&
                    (filter.PriceMax == null || p.Price <= filter.PriceMax) &&
                    (filter.ProductSize == null || p.ProductSize == filter.ProductSize) &&
                    (filter.WeightMin == null || p.Weight >= filter.WeightMin) &&
                    (filter.WeightMax == null || p.Weight <= filter.WeightMax)
                );
         
            return Ok(results);
        }
        */



        [HttpPost("user/Add")]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            var createdUser = await _shopRepository.AddUser(user);
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);


        }

        [HttpGet("user/{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {

            return await _shopRepository.Get(id);
        }

        [HttpPost("user/Reset-Password")]
        public async Task<ActionResult> ResetPassword(ResetPasswordRequest resetRequest)
        {
            //Check if username exists
            var user = await _shopRepository.GetUserByName(resetRequest.Name);
            if (user == null)
            {
                return NotFound("Invalid username or e-mail!");
            }
            //Check if Email exists
            user = await _shopRepository.GetUserByEmail(resetRequest.Email);
            if (user == null)
            {
                return NotFound("Invalid username or e-mail!");
            }
            //Check if the old password is correct
            if (user.Password != resetRequest.Password)
            {
                return BadRequest("Old password is incorrect!");
            }
            //Check if the new password is the same as the old password
            if (user.Password == resetRequest.NewPassword)
            {
                return BadRequest("This password was previously used, add a new password!");
            }
            //Reset password
            user.Password = resetRequest.NewPassword;
            await _shopRepository.UpdateUser(user);
            return Ok("Password reset successfully!");
         }
            [HttpGet("product/{category}")]
            public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(string category)
            {
                var products = await _shopRepository.GetByCategory(category);
                return Ok(products);

            }
            [HttpPost("import-csv-file")]
            public async Task<IActionResult> ImportProducts(IFormFile file)
            {
                await _shopRepository.ImportCsv(file);
                return Ok();
            }
    }
    } 





