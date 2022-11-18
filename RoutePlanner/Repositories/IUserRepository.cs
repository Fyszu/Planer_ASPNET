using RoutePlanner.Data;

namespace RoutePlanner.Repositories
{
    public interface IUserRepository : IRepository<User, string>
    {
        public Task<User> GetByNameAsync(string name);
    }
}
