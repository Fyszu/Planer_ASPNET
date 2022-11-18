using RoutePlanner.Data;
using Microsoft.EntityFrameworkCore;

namespace RoutePlanner.Repositories
{
    public class ChargingStationsRepository : IChargingStationsRepository
    {
        private readonly MyDbContext context;
        public ChargingStationsRepository(MyDbContext context)
        {
            this.context = context;
        }



        public async Task<ChargingStation> GetByIdAsync(long id)
        {
            return await context.ChargingStations.Include(station => station.Provider).Include(station => station.OperatingHours).Include(station => station.ChargingPoints).ThenInclude(point => point.Connectors).ThenInclude(connector => connector.Interfaces).Where(station => station.Id.Equals(id)).SingleOrDefaultAsync();
        }

        public async Task AddAsync(ChargingStation chargingStation)
        {
            if(chargingStation != null)
                await context.ChargingStations.AddAsync(chargingStation);
            await context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(HashSet<ChargingStation> chargingStations)
        {
            await context.ChargingStations.AddRangeAsync(chargingStations);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ChargingStation chargingStation)
        {
            if (chargingStation != null)
            {
                ChargingStation dbChargingStation = await GetByIdAsync(chargingStation.Id);
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
            await context.SaveChangesAsync();
        }

        public async Task RemoveAsync(ChargingStation chargingStation)
        {
            if (chargingStation != null)
                context.ChargingStations.Remove(chargingStation);
            await context.SaveChangesAsync();
        }

        // Removes all records from ChargingStations repository
        public async Task RemoveAllAsync()
        {
            await context.ChargingStations
                .Include(station => station.OperatingHours)
                .Include(station => station.ChargingPoints)
                .ThenInclude(point => point.Connectors)
                .ThenInclude(connector => connector.Interfaces)
                .ForEachAsync(station => {
                if (station != null)
                    context.ChargingStations.Remove(station);
            });
            await context.SaveChangesAsync();
        }

        public async Task<List<ChargingStation>> GetAllAsync()
        {
            return await context.ChargingStations.Include(station => station.Provider).Include(station => station.OperatingHours).Include(station => station.ChargingPoints).ThenInclude(point => point.Connectors).ThenInclude(connector => connector.Interfaces).ToListAsync();
        }
    }
}
