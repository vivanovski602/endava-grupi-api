using endavaRestApi.Data;
using log4net;
using Microsoft.AspNetCore.Mvc;

namespace endavaRestApi.Repositories
{
    public class OrderRepository: IOrderRepository
    {
        private readonly ShopContext _context;
        private static readonly ILog log = LogManager.GetLogger(typeof(ShopRepository));    //log instance
        public OrderRepository(ShopContext context)
        {
            _context = context;
        }
        public async Task<bool> IsUserActive(int userId)
        {
            log.Debug("Checking if user is active...");       //log message of debug level

            var user = await _context.Users.FindAsync(userId);
            return user != null && user.IsActive;
        }

        public async Task<bool> AreProductsAvailable(Dictionary<int, int> productQuantities)
        {
            log.Debug("Checking if products are available...");       //log message of debug level

            foreach (var kvp in productQuantities)
            {
                var product = await _context.Products.FindAsync(kvp.Key);
                if (product == null || product.ProductQuantity < kvp.Value)
                    return false;
            }
            return true;
        }

        /* public async Task<Order> CreateOrder(int userId, Dictionary<int, int> productQuantities)
        {

            if (!await  _orderRepository.IsUserActive(userId))           //vo Repository
            {
                return BadRequest("User is inactive or does not exist.");
            }

            if (!await _orderRepository.AreProductsAvailable(productQuantities))
            {
                return BadRequest("One or more products are not available or have insufficient quantity.");
            }
            //ovde validaciite so if od ShopController
            var order = new Order
            {
                // Set order properties
                UserId = userId,
                OrderPlaced = DateTime.Now,
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var kvp in productQuantities)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.OrderId,
                    ProductId = kvp.Key,
                    Quantity = kvp.Value,
                };

                _context.OrderDetails.Add(orderDetail);
            }

            await _context.SaveChangesAsync();

            return order;
        } */

        public async Task<(Order, IActionResult)> CreateOrder(int userId, Dictionary<int, int> productQuantities)
        {
            if (!await IsUserActive(userId))
            {
                return (null, new BadRequestObjectResult("User is inactive or does not exist."));
            }

            if (!await AreProductsAvailable(productQuantities))
            {
                return (null, new BadRequestObjectResult("One or more products are not available or have insufficient quantity."));
            }
            log.Debug("Creating order...");       //log message of debug level

            var order = new Order
            {   
                // Set order properties
                UserId = userId,
                OrderPlaced = DateTime.Now,
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            
            log.Debug("Adding Order Details...");       //log message of debug level

            foreach (var kvp in productQuantities)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.OrderId,
                    ProductId = kvp.Key,
                    Quantity = kvp.Value,
                };

                _context.OrderDetails.Add(orderDetail);
            }

            await _context.SaveChangesAsync();

            return (order, null);
        }
    


    public async Task<Payment> CreatePayment(int orderId)
        {
            log.Debug("Creating Payment...");       //log message of debug level

            var payment = new Payment
            {
                OrderId = orderId,
                //Amount = amount,
                Status = "Pending",
                // Additional payment properties as needed
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return payment;
        }



    }
}
