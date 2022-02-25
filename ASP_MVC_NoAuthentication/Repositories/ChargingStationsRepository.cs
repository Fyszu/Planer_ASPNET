using ASP_MVC_NoAuthentication.Data;
using Microsoft.EntityFrameworkCore;

namespace ASP_MVC_NoAuthentication.Repositories
{
    public class ChargingStationsRepository
    {
        private readonly MyDbContext _context;

        public ChargingStationsRepository(MyDbContext context)
        {
            _context = context;
        }


        public List<ChargingStation> GetAllChargingStations()
        {
            return _context.ChargingStations.Include(station => station.ChargingPoints).ThenInclude(point => point.Connector).ToList();
        }
    }
}
