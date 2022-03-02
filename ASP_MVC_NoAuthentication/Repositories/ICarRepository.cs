using ASP_MVC_NoAuthentication.Data;

namespace ASP_MVC_NoAuthentication.Repositories
{
    public interface ICarRepository : IRepository<Car, int>
    {
        public List<Car> GetDefaultCars();
        public List<Car> GetCarsByUser(User user);
        void RemoveById(int id);
    }
}
