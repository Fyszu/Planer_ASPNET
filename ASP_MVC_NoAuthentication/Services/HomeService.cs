using ASP_MVC_NoAuthentication.Data;

namespace ASP_MVC_NoAuthentication.Services
{
	public class HomeService : IHomeService
    {
        private readonly ILogger<HomeService> _logger;
        private readonly MyDbContext _context;

        public HomeService(ILogger<HomeService> logger, MyDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public List<Car> getUserCars(string userName)
        {
            var cars = _context.Cars.ToList();
            var userQueryable = from u in _context.Users where u.UserName == userName select u; //pobranie użytkownika
            User user = userQueryable.FirstOrDefault<User>();
            var personalCars = from u in _context.Users where u.Id == user.Id from c in u.PersonalCars select c; //pobranie aut użytkownika jako queryable
            List<Car>? userCars = cars;
            foreach (PersonalCar car in personalCars)
                userCars.Add(car.toCar());
            return userCars;
        }

        public List<Car> getDefaultCars()
		{
            return _context.Cars.ToList();
        }
	}
}
