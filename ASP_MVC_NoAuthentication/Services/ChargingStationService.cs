using ASP_MVC_NoAuthentication.Data;
using ASP_MVC_NoAuthentication.Repositories;
using Microsoft.EntityFrameworkCore;


namespace ASP_MVC_NoAuthentication.Services
{
    public class ChargingStationService : IChargingStationService
    {
        private readonly ILogger<ChargingStationService> _logger;
        private readonly IChargingStationsRepository _chargingStationsRepository;

        public ChargingStationService(ILogger<ChargingStationService> logger, IChargingStationsRepository chargingStationsRepository)
        {
            _logger = logger;
            _chargingStationsRepository = chargingStationsRepository;
        }

        public async Task<List<ChargingStation>> GetAllChargingStationsAsync()
        {
            return await _chargingStationsRepository.GetAllAsync();
        }
    }
}
