using endavaRestApi.Data;
using log4net;
using Microsoft.EntityFrameworkCore;

namespace endavaRestApi.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly ShopContext _context;
        private static readonly ILog log = LogManager.GetLogger(typeof(ShopRepository));    //log instance
        public ReportRepository(ShopContext context)
        {
            _context = context;
        }
        public async Task<int> GetOrderCountAsync()
        {
            try
            {
                int orderCount = await _context.Orders.Select(o => o.OrderId).CountAsync();
                return orderCount;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw;
            }
        }
    }
}
