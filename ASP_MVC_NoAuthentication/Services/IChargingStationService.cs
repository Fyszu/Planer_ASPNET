using ASP_MVC_NoAuthentication.Data;

namespace ASP_MVC_NoAuthentication.Services
{
    public interface IChargingStationService
    {
        public Task<List<ChargingStation>> GetAllChargingStations();
    }
}
