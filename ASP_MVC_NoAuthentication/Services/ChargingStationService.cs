using ASP_MVC_NoAuthentication.Data;
using ASP_MVC_NoAuthentication.Repositories;

namespace ASP_MVC_NoAuthentication.Services
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
