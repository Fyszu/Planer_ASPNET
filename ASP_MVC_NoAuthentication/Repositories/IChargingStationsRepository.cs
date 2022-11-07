using ASP_MVC_NoAuthentication.Data;

namespace ASP_MVC_NoAuthentication.Repositories
{
    public interface IChargingStationsRepository : IRepository<ChargingStation, long>
    {
        public Task RemoveAll();
        public Task AddRange(HashSet<ChargingStation> chargingStations);
    }
}
