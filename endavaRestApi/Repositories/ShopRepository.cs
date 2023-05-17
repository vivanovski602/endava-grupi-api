using endavaRestApi.Data;
using log4net;
using Microsoft.EntityFrameworkCore;

namespace endavaRestApi.Repositories
{
    public class ShopRepository:IShopRepository
    {
        private readonly ShopContext _context;
        private static readonly ILog log = LogManager.GetLogger(typeof(ShopRepository));    //log instance
        public ShopRepository(ShopContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> Get()
        {
            log.Debug("Getting products...");       //log message of debug level
            return await _context.Products.ToListAsync();

        }

        public async Task<User> AddUser(User user)
        {
            log.Info($"Adding user {user.Name}.");      //log message of info level
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            log.Info($"User added: {user.Name}");       //log message of info level
            return user;
        }


        public async Task<User> Get(int id)
        {
            log.Info($"Getting user with id {id}");         //log message of info level
            return await _context.Users.FindAsync(id);

        }

        public async Task<IEnumerable<Product>> GetByCategory(string category)
        {
            var allProducts = await _context.Products.ToListAsync();

            var filteredProducts = allProducts.Where(p => p.ProductCategory == category);

            return filteredProducts;
        }

        public async Task<IEnumerable<Product>> Filter(ProductFilter filter)
        {
            log.Info("Filtering products.");                    //log message of info level
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

