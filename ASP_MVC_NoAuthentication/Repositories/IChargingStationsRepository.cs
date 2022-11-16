using ASP_MVC_NoAuthentication.Data;

namespace ASP_MVC_NoAuthentication.Repositories
{
    public interface IChargingStationsRepository : IRepository<ChargingStation, long>
    {
        public Task RemoveAllAsync();
        public Task AddRangeAsync(HashSet<ChargingStation> chargingStations);
    }
}
