using ASP_MVC_NoAuthentication.Data;

namespace ASP_MVC_NoAuthentication.Repositories
{
    public interface IConnectorInterfaceRepository : IRepository<ConnectorInterface, long>
    {
        public Task RemoveAllAsync();
    }
}
