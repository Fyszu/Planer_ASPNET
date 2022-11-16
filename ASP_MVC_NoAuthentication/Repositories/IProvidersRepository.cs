using ASP_MVC_NoAuthentication.Data;

namespace ASP_MVC_NoAuthentication.Repositories
{
    public interface IProvidersRepository : IRepository<Provider, long>
    {
        public Task RemoveAllAsync();
    }
}
