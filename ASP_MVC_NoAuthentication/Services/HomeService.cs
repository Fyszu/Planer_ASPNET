using ASP_MVC_NoAuthentication.Data;
using ASP_MVC_NoAuthentication.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ASP_MVC_NoAuthentication.Services
{
	public class HomeService : IHomeService
    {
        private readonly ILogger<HomeService> _logger;
        private readonly MyDbContext _context;
        private readonly UserRepository _userRepository;
        private readonly CarRepository _carRepository;

		public HomeService(ILogger<HomeService> logger, MyDbContext context, UserRepository userRepository, CarRepository carRepository)
		{
			_logger = logger;
			_context = context;
			_userRepository = userRepository;
			_carRepository = carRepository;
		}
		public List<Car> getUserCars(string userName)
        {
            var cars = _carRepository.getCars();
            var userQueryable = from u in _context.Users where u.UserName == userName select u; //pobranie użytkownika
            User user = userQueryable.FirstOrDefault<User>();
            var personalCars = _userRepository.getPersonalCars(user.Id); //pobranie aut użytkownika jako queryable
            List<Car>? userCars = cars;
            foreach (PersonalCar car in personalCars)
                userCars.Add(car.toCar());
            return userCars;
        }

        public List<Car> getDefaultCars()
		{
            return _carRepository.getCars();
        }
	}
}
