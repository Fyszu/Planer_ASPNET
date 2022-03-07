using ASP_MVC_NoAuthentication.Data;

namespace ASP_MVC_NoAuthentication.Repositories
{
    public interface ICarRepository : IRepository<Car, int>
    {
        public Task<List<Car>> GetDefaultCars();
        public Task<List<Car>> GetCarsByUser(User user);
        Task RemoveById(int id);
    }
}
