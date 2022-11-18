using RoutePlanner.Data;

namespace RoutePlanner.Repositories
{
    public interface ICarRepository : IRepository<Car, int>
    {
        public Task<List<Car>> GetDefaultCarsAsync();
        public Task<List<Car>> GetCarsByUserAsync(User user);
        public Task RemoveAllAsync();
        public Task AddRangeAsync(List<Car> cars);
        Task RemoveByIdAsync(int id);
    }
}
