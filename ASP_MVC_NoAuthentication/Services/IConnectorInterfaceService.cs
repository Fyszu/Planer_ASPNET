using ASP_MVC_NoAuthentication.Data;

namespace ASP_MVC_NoAuthentication.Services
{
    public interface IConnectorInterfaceService
    {
        public Task<List<ConnectorInterface>> GetAllConnectorInterfaces();
    }
}
