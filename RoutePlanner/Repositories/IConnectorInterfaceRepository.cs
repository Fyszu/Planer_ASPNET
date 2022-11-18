using RoutePlanner.Data;

namespace RoutePlanner.Repositories
{
    public interface IConnectorInterfaceRepository : IRepository<ConnectorInterface, long>
    {
        public Task RemoveAllAsync();
    }
}
