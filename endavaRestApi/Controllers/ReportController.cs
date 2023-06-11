using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using endavaRestApi.Data;
using endavaRestApi.Repositories;
using log4net;
using log4net.Config;
using System.Runtime.CompilerServices;

namespace endavaRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ShopContext _context;
        public ReportController(ShopContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetOrderCount()
        {
            try
            {
                int orderCount = _context.Orders.Select(o => o.OrderId).Count();
                return Ok("This is a report that shows the number of orders\nNumber or orders : " + orderCount);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
