using CsvHelper;
using endavaRestApi.Data;
using log4net;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace endavaRestApi.Repositories
{
    public class ShopRepository : IShopRepository
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

        /* public async Task<User> AddUser(User user)
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

        } */
        /*
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


        } */
        
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

       /* public async Task<User> GetUserByEmail(string email)
        {
            log.Info($"Getting user with email {email}"); //log message of info level

            return await _context.Users.FirstOrDefaultAsync(user => user.Email == email);
        }

        public async Task<User> GetUserByName(string name)
        {
            log.Info($"Getting user with name {name}"); //log message of info level

            return await _context.Users.FirstOrDefaultAsync(user => user.Name == name);
        }

        public async Task<User> UpdateUser(User user)
        {
            log.Info($"Updating user with id {user.Id}"); //log message of info level
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            log.Info($"User updated: {user.Name}"); //log message of info level
            return user;
        } */
        public async Task<IEnumerable<Product>> GetByCategory(string category)
        {
            log.Info($"Filtering products by category"); //log message of info level
            var allProducts = await _context.Products.ToListAsync();
            var filteredProducts = allProducts.Where(p => p.ProductCategory == category);

            return filteredProducts;
        }
        public async Task ImportCsv(IFormFile file)
        {

            if (file == null || file.Length == 0)
            {
                log.Error("File is empty or null.");
                throw new ArgumentException("File is empty or null.");
            }

            using var streamReader = new StreamReader(file.OpenReadStream());
            using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);
            var records = csvReader.GetRecords<ProductCSV>().ToList();

            var avgPrice = await _context.Products.AverageAsync(p => p.Price);
            var newProducts = new List<Product>();
            foreach (var record in records)
            {
                var product = await _context.Products
                    .SingleOrDefaultAsync(p => p.ProductGuidId == record.ProductGuidId);

                if (product == null)
                {
                    product = new Product();
                    product.ProductGuidId = Guid.NewGuid();
                    product.ProductName = record.ProductName;
                    product.ProductBrand = record.ProductBrand;
                    product.ProductCategory = record.ProductCategory;
                    product.ProductDescription = record.ProductDescription;
                    product.ProductQuantity = record.ProductQuantity;
                    product.ProductSize = record.ProductSize;
                    product.Color = record.Color;
                    product.Weight = record.Weight;
                    product.Price = record.Price;
                    product.TotalPrice = record.Price > _context.Products.Average(p => p.Price) ? record.Price - record.ProductQuantity : record.Price + record.ProductQuantity;
                    newProducts.Add(product);
                    _context.Products.Add(product);
                }
                else
                {
                    product.ProductGuidId = record.ProductGuidId;
                    product.ProductName = record.ProductName;
                    product.ProductBrand = record.ProductBrand;
                    product.ProductCategory = record.ProductCategory;
                    product.ProductDescription = record.ProductDescription;
                    product.ProductQuantity = record.ProductQuantity;
                    product.ProductSize = record.ProductSize;
                    product.Color = record.Color;
                    product.Weight = record.Weight;
                    product.Price = record.Price;
                    product.TotalPrice = record.Price > _context.Products.Average(p => p.Price) ? record.Price - record.ProductQuantity : record.Price + record.ProductQuantity;
                    _context.Products.Update(product);
                }
            }
            log.Info("Successful");
            await _context.SaveChangesAsync();
        }

}
}

