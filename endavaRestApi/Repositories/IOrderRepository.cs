using endavaRestApi.Data;

namespace endavaRestApi.Repositories
{
    public interface IOrderRepository
    {
        Task<bool> IsUserActive(int userId);
        Task<bool> AreProductsAvailable(Dictionary<int, int> productQuantities);
        Task<Order> CreateOrder(int userId, Dictionary<int, int> productQuantities);
        Task<Payment> CreatePayment(int orderId, int amount);
    }
}
