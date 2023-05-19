﻿using endavaRestApi.Data;
using log4net;

namespace endavaRestApi.Repositories
{
    public class OrderRepository:IOrderRepository
    {
        private readonly ShopContext _context;
        private static readonly ILog log = LogManager.GetLogger(typeof(ShopRepository));    //log instance
        public OrderRepository(ShopContext context)
        {
            _context = context;
        }
        public async Task<bool> IsUserActive(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            return user != null && user.IsActive;
        }

        public async Task<bool> AreProductsAvailable(Dictionary<int, int> productQuantities)
        {
            foreach (var kvp in productQuantities)
            {
                var product = await _context.Products.FindAsync(kvp.Key);
                if (product == null || product.ProductQuantity < kvp.Value)
                    return false;
            }
            return true;
        }

        public async Task<Order> CreateOrder(int userId, Dictionary<int, int> productQuantities)
        {
            var order = new Order
            {
                // Set order properties
                UserId = userId,
                OrderPlaced = DateTime.Now,
            };

            _context.Orders.Add(order);

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
        }

        public async Task<Payment> CreatePayment(int orderId, decimal amount)
        {
            var payment = new Payment
            {
                OrderId = orderId,
                Amount = amount,
                Status = "Pending",
                // Additional payment properties as needed
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return payment;
        }



    }
}