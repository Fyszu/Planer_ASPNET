using ASP_MVC_NoAuthentication.Data;

namespace ASP_MVC_NoAuthentication.Services
{
    public interface ICarService
    {
        public Task<List<Car>> GetDefaultCars();
        public Task<List<Car>> GetCarsByUser(String userName);
        public Task<Car> GetCarById(int id);
        public Task RemoveCarByUser(string userName, int carId);
        public Task AddNewCar(Car car);
        public Task UpdateCar(Car car);
        public Task<Boolean> CheckIfCarBelongsToUser(User user, Car car);
    }
}
