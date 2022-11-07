using ASP_MVC_NoAuthentication.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace ASP_MVC_NoAuthentication.Repositories
{
    public class ChargingStationsRepository : IChargingStationsRepository
    {
        private readonly MyDbContext _context;
        public ChargingStationsRepository(MyDbContext context)
        {
            _context = context;
        }



        public async Task<ChargingStation> GetById(long id)
        {
            return await _context.ChargingStations.Include(station => station.Provider).Include(station => station.OperatingHours).Include(station => station.ChargingPoints).ThenInclude(point => point.Connectors).ThenInclude(connector => connector.Interfaces).Where(station => station.Id.Equals(id)).SingleOrDefaultAsync();
        }

        public async Task Add(ChargingStation chargingStation)
        {
            if(chargingStation != null)
                await _context.ChargingStations.AddAsync(chargingStation);
            await _context.SaveChangesAsync();
        }

        public async Task AddRange(List<ChargingStation> chargingStations)
        {
            await _context.ChargingStations.AddRangeAsync(chargingStations);
            await _context.SaveChangesAsync();
        }

        public async Task Update(ChargingStation chargingStation)
        {
            if (chargingStation != null)
            {
                ChargingStation dbChargingStation = await GetById(chargingStation.Id);
                dbChargingStation.Provider = chargingStation.Provider;
                dbChargingStation.Latitude = chargingStation.Latitude;
                dbChargingStation.Longitude = chargingStation.Longitude;
                dbChargingStation.Name = chargingStation.Name;
                dbChargingStation.City = chargingStation.City;
                dbChargingStation.PostalCode = chargingStation.PostalCode;
                dbChargingStation.Street = chargingStation.Street;
                dbChargingStation.HouseNumber = chargingStation.HouseNumber;
                dbChargingStation.Community = chargingStation.Community;
                dbChargingStation.District = chargingStation.District;
                dbChargingStation.Province = chargingStation.Province;
                dbChargingStation.OperatingHours = chargingStation.OperatingHours;
                dbChargingStation.Accessibility = chargingStation.Accessibility;
                dbChargingStation.PaymentMethods = chargingStation.PaymentMethods;
                dbChargingStation.AuthenticationMethods = chargingStation.AuthenticationMethods;
                dbChargingStation.ChargingPoints = chargingStation.ChargingPoints;
            }
            await _context.SaveChangesAsync();
        }

        public async Task Remove(ChargingStation chargingStation)
        {
            if (chargingStation != null)
                _context.ChargingStations.Remove(chargingStation);
            await _context.SaveChangesAsync();
        }

        // Removes all records from ChargingStations repository
        public async Task RemoveAll()
        {
            await _context.ChargingStations
                .Include(station => station.OperatingHours)
                .Include(station => station.ChargingPoints)
                .ThenInclude(point => point.Connectors)
                .ThenInclude(connector => connector.Interfaces)
                .ForEachAsync(station => {
                if (station != null)
                    _context.ChargingStations.Remove(station);
            });
            await _context.SaveChangesAsync();
        }

        public async Task<List<ChargingStation>> GetAll()
        {
            return await _context.ChargingStations.Include(station => station.Provider).Include(station => station.OperatingHours).Include(station => station.ChargingPoints).ThenInclude(point => point.Connectors).ThenInclude(connector => connector.Interfaces).ToListAsync();
        }
    }
}
