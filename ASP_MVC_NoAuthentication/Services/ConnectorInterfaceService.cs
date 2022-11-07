using ASP_MVC_NoAuthentication.Data;
using ASP_MVC_NoAuthentication.Repositories;

namespace ASP_MVC_NoAuthentication.Services
{
    public class ConnectorInterfaceService : IConnectorInterfaceService
    {
        private readonly IConnectorInterfaceRepository _repository;
        public ConnectorInterfaceService(IConnectorInterfaceRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ConnectorInterface>> GetAllConnectorInterfaces()
        {
            return await _repository.GetAll();
        }
    }
}
