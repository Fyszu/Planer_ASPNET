using ASP_MVC_NoAuthentication.Data;
using ASP_MVC_NoAuthentication.Repositories;

namespace ASP_MVC_NoAuthentication.Services
{
    public class CarService : ICarService
    {
        private readonly ILogger<CarService> _logger;
        private readonly IUserRepository _userRepository;
        private readonly ICarRepository _carRepository;

        public CarService(ILogger<CarService> logger, IUserRepository userRepository, ICarRepository carRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _carRepository = carRepository;
        }



        public async Task<List<Car>> GetDefaultCarsAsync() { return await _carRepository.GetDefaultCarsAsync(); }

        public async Task<List<Car>> GetCarsByUserAsync(String userName) { return await _carRepository.GetCarsByUserAsync(await _userRepository.GetByNameAsync(userName)); }

        public async Task<Car> GetCarByIdAsync(int id) { return await _carRepository.GetByIdAsync(id); }

        public async Task RemoveCarByUserAsync(string userName, int carId) { await _carRepository.RemoveByIdAsync(carId); }

        public async Task AddNewCarAsync(Car car) { await _carRepository.AddAsync(car); }

        public async Task UpdateCarAsync(Car car) { await _carRepository.UpdateAsync(car); }

        public async Task<Boolean> CheckIfCarBelongsToUserAsync(User user, Car car)
        {
            if (car is null)
                return false;
            else if (car.User is null)
                return false;
            else if ((await _carRepository.GetByIdAsync(car.Id)).User.Equals(await _userRepository.GetByNameAsync(user.UserName)))
                return true;
            else
                return false;
        }
    }
}
