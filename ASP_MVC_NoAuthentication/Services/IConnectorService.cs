using ASP_MVC_NoAuthentication.Data;

namespace ASP_MVC_NoAuthentication.Services
{
    public interface IConnectorService
    {
        public Task<List<Connector>> GetAllConnectors();
    }
}
