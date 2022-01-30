using ASP_MVC_NoAuthentication.Data;
using ASP_MVC_NoAuthentication.Repositories;

namespace ASP_MVC_NoAuthentication.Services
{
    public class CarService : ICarService
    {
        private readonly ILogger<HomeService> _logger;
        private readonly MyDbContext _context;
        private readonly UserRepository _userRepository;
        private readonly CarRepository _carRepository;

        public CarService(ILogger<HomeService> logger, MyDbContext context, UserRepository userRepository, CarRepository carRepository)
        {
            _logger = logger;
            _context = context;
            _userRepository = userRepository;
            _carRepository = carRepository;
        }


        public void RemoveUserCar(User user, int carId)
        {
            PersonalCar car = _carRepository.getPersonalCarById(user.Id, carId);
            if (car != null)
            {
                _context.PersonalCars.Remove(car);
                _context.SaveChanges();
            }
        }
        public int AddUserCar(User user, Car car)
        {
            PersonalCar userCar = new PersonalCar(car.Brand, car.Model, car.MaximumDistance, car.Connectors, user);
            _context.PersonalCars.Add(userCar);
            _context.SaveChanges();
            return 1;
        }

        public PersonalCar getPersonalCarById(string userId, int carId)
        {
            return _carRepository.getPersonalCarById(userId, carId);
        }

        public void UpdateCar(User user, Car car)
        {
            PersonalCar userCar = new PersonalCar(car.Id, car.Brand, car.Model, car.MaximumDistance, car.Connectors, user);
            _carRepository.UpdatePersonalCar(user, userCar);
        }
    }
}
