using RoutePlanner.Data;

namespace RoutePlanner.Repositories
{
    public interface IChargingStationsRepository : IRepository<ChargingStation, long>
    {
        public Task RemoveAllAsync();
        public Task AddRangeAsync(HashSet<ChargingStation> chargingStations);
    }
}
