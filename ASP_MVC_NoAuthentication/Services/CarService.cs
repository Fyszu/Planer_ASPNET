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



        public List<Car> GetDefaultCars() { return _carRepository.GetDefaultCars(); }

        public List<Car> GetCarsByUser(String userName) { return _carRepository.GetCarsByUser(_userRepository.GetByName(userName)); }

        public Car GetCarById(int id) { return _carRepository.GetById(id); }

        public void RemoveCarByUser(string userName, int carId) { _carRepository.RemoveById(carId); }

        public void AddNewCar(Car car) { _carRepository.Add(car); }

        public void UpdateCar(Car car) { _carRepository.Update(car); }

        public Boolean CheckIfCarBelongsToUser(User user, Car car)
        {
            if (car.User is null)
                return false;
            else if (_carRepository.GetById(car.Id).User.Equals(_userRepository.GetByName(user.UserName)))
                return true;
            else
                return false;
        }
    }
}
