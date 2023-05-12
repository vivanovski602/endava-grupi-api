using endavaRestApi.Data;
using log4net;
using Microsoft.EntityFrameworkCore;

namespace endavaRestApi.Repositories
{
    public class ShopRepository:IShopRepository
    {
        private readonly ShopContext _context;
        private static readonly ILog log = LogManager.GetLogger(typeof(ShopRepository));

        public ShopRepository(ShopContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> Get()
        {
            return await _context.Products.ToListAsync();

        }

        public async Task<User> AddUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            log.Info($"User added: {user.Id}");
            return user;
        }


        public async Task<User> Get(int id)
        {
            return await _context.Users.FindAsync(id);

        }

        public async Task<IEnumerable<Product>> Filter(ProductFilter filter)
        {

            var _products = await _context.Products.ToListAsync();

            var results = _products.Where(p =>
                        (filter.ProductCategory == null || p.ProductCategory == filter.ProductCategory) &&
                        (filter.ProductBrand == null || p.ProductBrand == filter.ProductBrand) &&
                        (filter.PriceMin == null || p.Price >= filter.PriceMin) &&
                        (filter.PriceMax == null || p.Price <= filter.PriceMax) &&
                        (filter.ProductSize == null || p.ProductSize == filter.ProductSize) &&
                        (filter.WeightMin == null || p.Weight >= filter.WeightMin) &&
                        (filter.WeightMax == null || p.Weight <= filter.WeightMax)
                    );

            return results;
        }


    }
    }

