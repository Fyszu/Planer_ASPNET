using ASP_MVC_NoAuthentication.Data;
using ASP_MVC_NoAuthentication.Repositories;
using Microsoft.EntityFrameworkCore;


namespace ASP_MVC_NoAuthentication.Services
{
    public class ChargingStationService : IChargingStationService
    {
        private readonly ILogger<ChargingStationService> _logger;
        private readonly ChargingStationsRepository _chargingStationsRepository;

        public ChargingStationService(ILogger<ChargingStationService> logger, ChargingStationsRepository chargingStationsRepository)
        {
            _logger = logger;
            _chargingStationsRepository = chargingStationsRepository;
        }

        public List<ChargingStation> GetAllChargingStations() //przejrzeć
        {
            return _chargingStationsRepository.GetAll();
        }
    }
}
