using RoutePlanner.Data;

namespace RoutePlanner.Repositories
{
    public interface IChargingPointsRepository : IRepository<ChargingPoint, long>
    {
        public Task RemoveAllAsync();
    }
}
