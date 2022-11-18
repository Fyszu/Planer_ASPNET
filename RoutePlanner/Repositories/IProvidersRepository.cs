using RoutePlanner.Data;

namespace RoutePlanner.Repositories
{
    public interface IProvidersRepository : IRepository<Provider, long>
    {
        public Task RemoveAllAsync();
    }
}
