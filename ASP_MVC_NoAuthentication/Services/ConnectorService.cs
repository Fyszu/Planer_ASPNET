using ASP_MVC_NoAuthentication.Data;
using ASP_MVC_NoAuthentication.Repositories;

namespace ASP_MVC_NoAuthentication.Services
{
    public class ConnectorService : IConnectorService
    {
        private readonly ILogger<ConnectorService> _logger;
        private readonly ConnectorRepository _connectorRepository;

        public ConnectorService(ILogger<ConnectorService> logger, ConnectorRepository connectorRepository)
        {
            _logger = logger;
            _connectorRepository = connectorRepository;
        }


        public async Task<List<Connector>> GetAllConnectors()
        {
            return await _connectorRepository.GetAll();
        }
    }
}
