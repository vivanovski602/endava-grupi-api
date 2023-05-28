using endavaRestApi.Data;
using log4net;

namespace endavaRestApi.Repositories
{
    public class UserRepository
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
    }
}
