using ASP_MVC_NoAuthentication.Data;
using ASP_MVC_NoAuthentication.Repositories;

namespace ASP_MVC_NoAuthentication.Services
{
    public class CarService : ICarService
    {
        private readonly ILogger<CarService> _logger;
        private readonly UserRepository _userRepository;
        private readonly CarRepository _carRepository;

        public CarService(ILogger<CarService> logger, UserRepository userRepository, CarRepository carRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _carRepository = carRepository;
        }



        public async Task<List<Car>> GetDefaultCars() { return await _carRepository.GetDefaultCars(); }

        public async Task<List<Car>> GetCarsByUser(String userName) { return await _carRepository.GetCarsByUser(await _userRepository.GetByName(userName)); }

        public async Task<Car> GetCarById(int id) { return await _carRepository.GetById(id); }

        public async Task RemoveCarByUser(string userName, int carId) { await _carRepository.RemoveById(carId); }

        public async Task AddNewCar(Car car) { await _carRepository.Add(car); }

        public async Task UpdateCar(Car car) { await _carRepository.Update(car); }

        public async Task<Boolean> CheckIfCarBelongsToUser(User user, Car car)
        {
            if (car is null)
                return false;
            else if (car.User is null)
                return false;
            else if ((await _carRepository.GetById(car.Id)).User.Equals(await _userRepository.GetByName(user.UserName)))
                return true;
            else
                return false;
        }
    }
}
