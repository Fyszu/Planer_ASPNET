using RoutePlanner.Data;

namespace RoutePlanner.Services
{
    public interface IChargingStationService
    {
        public Task<List<ChargingStation>> GetAllChargingStationsAsync();
    }
}
