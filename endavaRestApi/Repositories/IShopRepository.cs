using endavaRestApi.Data;


namespace endavaRestApi.Repositories
{
    public interface IShopRepository
    {
        Task<IEnumerable<Product>> Get();
       Task<User> AddUser(User user);
       Task<User> Get(int id);
        //Task<IEnumerable<Product>> Filter(ProductFilter filter);
    }
}

