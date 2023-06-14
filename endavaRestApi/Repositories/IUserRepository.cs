using endavaRestApi.Data;

namespace endavaRestApi.Repositories
{
    public interface IUserRepository
    {
        Task<User> AddUser(User user);
        Task<User> Get(int id);
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserByName(string name);
        Task<User> UpdateUser(User user);
        Task<User> ValidateLogin(string email, string password);
    }
}
