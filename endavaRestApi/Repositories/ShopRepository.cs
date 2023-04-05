using endavaRestApi.Data;
using Microsoft.EntityFrameworkCore;

namespace endavaRestApi.Repositories
{
    public class ShopRepository:IShopRepository
    {
        private readonly ShopContext _context;
       
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

            return user;
        }


        public async Task<User> Get(int id)
        {
            return await _context.Users.FindAsync(id);

        }

        
       

    }
    }

