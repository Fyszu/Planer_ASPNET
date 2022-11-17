using ASP_MVC_NoAuthentication.Data;
using ASP_MVC_NoAuthentication.Repositories;

namespace ASP_MVC_NoAuthentication.Services
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
