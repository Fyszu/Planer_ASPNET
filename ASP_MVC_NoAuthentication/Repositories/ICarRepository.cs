using ASP_MVC_NoAuthentication.Data;

namespace ASP_MVC_NoAuthentication.Repositories
{
    public interface ICarRepository : IRepository<Car, int>
    {
        public Task<List<Car>> GetDefaultCarsAsync();
        public Task<List<Car>> GetCarsByUserAsync(User user);
        Task RemoveByIdAsync(int id);
    }
}
