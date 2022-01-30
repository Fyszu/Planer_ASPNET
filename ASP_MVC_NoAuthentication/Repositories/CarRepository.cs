using ASP_MVC_NoAuthentication.Data;
using Microsoft.EntityFrameworkCore;

namespace ASP_MVC_NoAuthentication.Repositories
{
    public class CarRepository
    {
        private readonly MyDbContext _context;

        public CarRepository(MyDbContext context)
        {
            _context = context;
        }

        public int AddNewCar(Car car)
        {
            Car newCar = new Car()
            {
                Brand = car.Brand,
                Model = car.Model,
                MaximumDistance = car.MaximumDistance,
                Connectors = car.Connectors
            };
            _context.Cars.Add(newCar);
            _context.SaveChanges();
            return newCar.Id;
        }

        public List<Car> getCars()
		{
            List<PersonalCar> personalCars = _context.Cars.OfType<PersonalCar>().ToList();
            List<Car> cars = _context.Cars.Include(c => c.Connectors).ToList();
            return cars.Except(personalCars).ToList();
        }

        public PersonalCar getPersonalCarById(string userId, int carId)
        {
            return _context.PersonalCars.Where(p => p.User.Id.Equals(userId)).Where(i => i.Id.Equals(carId)).Include(c => c.Connectors).FirstOrDefault();
        }

        public void UpdatePersonalCar(User user, PersonalCar car)
        {
            var dbCar = _context.PersonalCars.Where(c => c.User.Id.Equals(user.Id)).Where(pc => pc.Id.Equals(car.Id)).FirstOrDefault();
            if (dbCar != null)
            {
                dbCar.Brand = car.Brand;
                dbCar.Model = car.Model;
                dbCar.MaximumDistance = car.MaximumDistance;
                dbCar.Connectors = car.Connectors;
                _context.SaveChanges();
            }
        }
    }
}
