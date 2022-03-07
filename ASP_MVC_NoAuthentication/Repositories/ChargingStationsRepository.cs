using ASP_MVC_NoAuthentication.Data;
using Microsoft.EntityFrameworkCore;

namespace ASP_MVC_NoAuthentication.Repositories
{
    public class ChargingStationsRepository : IChargingStationsRepository
    {
        private readonly MyDbContext _context;
        public ChargingStationsRepository(MyDbContext context)
        {
            _context = context;
        }



        public async Task<ChargingStation> GetById(int id)
        {
            return await _context.ChargingStations.Include(station => station.ChargingPoints).ThenInclude(point => point.Connector).Where(station => station.Id.Equals(id)).SingleOrDefaultAsync();
        }

        public async Task Add(ChargingStation chargingStation)
        {
            if(chargingStation != null)
                await _context.ChargingStations.AddAsync(chargingStation);
            await _context.SaveChangesAsync();
        }

        public async Task Update(ChargingStation chargingStation)
        {
            if (chargingStation != null)
            {
                ChargingStation dbChargingStation = await GetById(chargingStation.Id);
                dbChargingStation.Street = chargingStation.Street;
                dbChargingStation.ChargingPoints = chargingStation.ChargingPoints;
                dbChargingStation.PaymentMethods = chargingStation.PaymentMethods;
                dbChargingStation.StationState = chargingStation.StationState;
                dbChargingStation.Latitude = chargingStation.Latitude;
                dbChargingStation.Longitude = chargingStation.Longitude;
                dbChargingStation.OpenHours = chargingStation.OpenHours;
                dbChargingStation.City = chargingStation.City;
                dbChargingStation.Owner = chargingStation.Owner;
                dbChargingStation.Name = chargingStation.Name;
                dbChargingStation.PostalAdress = chargingStation.PostalAdress;
            }
            await _context.SaveChangesAsync();
        }

        public async Task Remove(ChargingStation chargingStation)
        {
            if (chargingStation != null)
                _context.ChargingStations.Remove(chargingStation);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ChargingStation>> GetAll()
        {
            return await _context.ChargingStations.Include(station => station.ChargingPoints).ThenInclude(point => point.Connector).ToListAsync();
        }
    }
}
