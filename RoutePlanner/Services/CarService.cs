using RoutePlanner.Data;
using RoutePlanner.Repositories;

namespace RoutePlanner.Services
{
    public class CarService : ICarService
    {
        private readonly IUserRepository userRepository;
        private readonly ICarRepository carRepository;

        public CarService(IUserRepository userRepository, ICarRepository carRepository)
        {
            this.userRepository = userRepository;
            this.carRepository = carRepository;
        }

        public async Task<List<Car>> GetDefaultCarsAsync() { return await carRepository.GetDefaultCarsAsync(); }

        public async Task<List<Car>> GetCarsByUserAsync(String userName) { return await carRepository.GetCarsByUserAsync(await userRepository.GetByNameAsync(userName)); }

        public async Task<Car> GetCarByIdAsync(int id) { return await carRepository.GetByIdAsync(id); }

        public async Task RemoveCarByUserAsync(string userName, int carId) { await carRepository.RemoveByIdAsync(carId); }

        public async Task AddNewCarAsync(Car car) { await carRepository.AddAsync(car); }

        public async Task UpdateCarAsync(Car car) { await carRepository.UpdateAsync(car); }

        public async Task<Boolean> CheckIfCarBelongsToUserAsync(User user, Car car)
        {
            if (car is null)
                return false;

            else if (car.User is null)
                return false;

            else if ((await carRepository.GetByIdAsync(car.Id)).User.Equals(await userRepository.GetByNameAsync(user.UserName)))
                return true;

            else
                return false;
        }
    }
}
