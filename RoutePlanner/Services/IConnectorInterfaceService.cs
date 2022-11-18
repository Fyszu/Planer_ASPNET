using RoutePlanner.Data;

namespace RoutePlanner.Services
{
    public interface IConnectorInterfaceService
    {
        public Task<List<ConnectorInterface>> GetAllConnectorInterfacesAsync();
    }
}
