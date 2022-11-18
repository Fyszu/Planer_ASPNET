using RoutePlanner.Data;
using RoutePlanner.Repositories;

namespace RoutePlanner.Services
{
    public class ChargingStationService : IChargingStationService
    {
        private readonly IChargingStationsRepository chargingStationsRepository;

        public ChargingStationService(IChargingStationsRepository chargingStationsRepository)
        {
            this.chargingStationsRepository = chargingStationsRepository;
        }

        public async Task<List<ChargingStation>> GetAllChargingStationsAsync()
        {
            return await chargingStationsRepository.GetAllAsync();
        }
    }
}
