using RoutePlanner.Data;
using RoutePlanner.Repositories;

namespace RoutePlanner.Services
{
    public class ConnectorInterfaceService : IConnectorInterfaceService
    {
        private readonly IConnectorInterfaceRepository repository;
        public ConnectorInterfaceService(IConnectorInterfaceRepository repository)
        {
            this.repository = repository;
        }
        public async Task<List<ConnectorInterface>> GetAllConnectorInterfacesAsync()
        {
            return await repository.GetAllAsync();
        }
    }
}
