using RoutePlanner.Data;

namespace RoutePlanner.Services
{
    public interface ICarService
    {
        public Task<List<Car>> GetDefaultCarsAsync();
        public Task<List<Car>> GetCarsByUserAsync(String userName);
        public Task<Car> GetCarByIdAsync(int id);
        public Task RemoveCarByUserAsync(string userName, int carId);
        public Task AddNewCarAsync(Car car);
        public Task UpdateCarAsync(Car car);
        public Task<Boolean> CheckIfCarBelongsToUserAsync(User user, Car car);
    }
}
