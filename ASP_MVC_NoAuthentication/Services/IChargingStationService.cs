using ASP_MVC_NoAuthentication.Data;

namespace ASP_MVC_NoAuthentication.Services
{
    public interface IChargingStationService
    {
        public List<ChargingStation> getChargingStationsByConnectors(List<String> connectorNames);
    }
}
