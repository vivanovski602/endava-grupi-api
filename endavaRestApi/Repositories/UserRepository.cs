using endavaRestApi.Data;
using log4net;
using Microsoft.EntityFrameworkCore;

namespace endavaRestApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ShopContext _context;
        private static readonly ILog log = LogManager.GetLogger(typeof(ShopRepository));    //log instance
        public UserRepository(ShopContext context)
        {
            _context = context;
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

        public async Task<User> GetUserByEmail(string email)
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
            log.Info($"Updating user with id {user.UserId}"); //log message of info level
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            log.Info($"User updated: {user.Name}"); //log message of info level
            return user;
        }
    }
}
