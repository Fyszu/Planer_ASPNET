using ASP_MVC_NoAuthentication.Data;
using Microsoft.EntityFrameworkCore;


namespace ASP_MVC_NoAuthentication.Services
{
    public class ChargingStationService : IChargingStationService
    {
        private readonly ILogger<ChargingStationService> _logger;
        private readonly MyDbContext _context;

        public ChargingStationService(ILogger<ChargingStationService> logger, MyDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public List<ChargingStation> getChargingStationsByConnectors(List<String> connectorNames)
        {
            List<ChargingStation>? chargingStations = new List<ChargingStation>();
            List<ChargingPoint>? chargingPoints = new List<ChargingPoint>();
            foreach (var connector in connectorNames)
            {
                chargingPoints.AddRange((from c in _context.ChargingPoints.Include("Connector").Include("Station") where c.Connector.Name == connector select c).ToList());
            }
            foreach (var chargingPoint in chargingPoints) {
                chargingStations.AddRange(from c in _context.ChargingStations where c.Id == chargingPoint.Station.Id select c);
            }
            return chargingStations.Distinct().ToList();
        }
    }
}
