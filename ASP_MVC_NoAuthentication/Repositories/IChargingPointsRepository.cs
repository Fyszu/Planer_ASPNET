using ASP_MVC_NoAuthentication.Data;

namespace ASP_MVC_NoAuthentication.Repositories
{
    public interface IChargingPointsRepository : IRepository<ChargingPoint, long>
    {
        public Task RemoveAllAsync();
    }
}
