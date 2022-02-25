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



        public void AddNewCar(Car car)
        {
            _context.Cars.Add(car);
            _context.SaveChanges();
        }

        public void UpdateCar(Car car)
        {
            var dbCar = _context.Cars.Where(dbc => dbc.Id.Equals(car.Id)).FirstOrDefault();
            if (dbCar != null)
            {
                dbCar.Brand = car.Brand;
                dbCar.Model = car.Model;
                dbCar.MaximumDistance = car.MaximumDistance;
                dbCar.Connectors = car.Connectors;
                _context.SaveChanges();
            }
        }
        public List<Car> GetDefaultCars()
        {
            return _context.Cars.Include(car => car.User).Include(car => car.Connectors).Where(car => car.User.Equals(null)).ToList();
        }

        public List<Car> GetCarsByUser(User user)
        {
            return _context.Cars.Include(car => car.User).Include(car => car.Connectors).Where(car => car.User.Equals(user)).ToList();
        }

        public Car GetCarById(int id)
        {
            return _context.Cars.Include(car => car.Connectors).Where(car => car.Id == id).FirstOrDefault();
        }

        public void RemoveCarById(int id)
        {
            Car car = _context.Cars.Where(car => car.Id == id).FirstOrDefault();
            if (car != null)
            {
                _context.Cars.Remove(car);
                _context.SaveChanges();
            }
        }
    }
}
