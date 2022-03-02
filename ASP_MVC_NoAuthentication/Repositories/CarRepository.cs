using ASP_MVC_NoAuthentication.Data;
using Microsoft.EntityFrameworkCore;

namespace ASP_MVC_NoAuthentication.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly MyDbContext _context;
        public CarRepository(MyDbContext context)
        {
            _context = context;
        }



        public Car GetById(int id)
        {
            return _context.Cars.Include(car => car.Connectors).Where(car => car.Id == id).SingleOrDefault();
        }

        public void Add(Car car)
        {
            if(car != null)
                _context.Cars.Add(car);
            _context.SaveChanges();
        }

        public void Update(Car car)
        {
            if (car != null)
            {
                Car dbCar = GetById(car.Id);
                dbCar.Brand = car.Brand;
                dbCar.Model = car.Model;
                dbCar.MaximumDistance = car.MaximumDistance;
                dbCar.Connectors = car.Connectors;
            }
            _context.SaveChanges();
        }

        public void Remove(Car car)
        {
            if(car != null)
            {
                _context.Cars.Remove(car);
                _context.SaveChanges();
            }
        }

        public List<Car> GetAll()
        {
            return _context.Cars.ToList();
        }

        public List<Car> GetDefaultCars()
        {
            return _context.Cars.Include(car => car.User).Include(car => car.Connectors).Where(car => car.User.Equals(null)).ToList();
        }

        public List<Car> GetCarsByUser(User user)
        {
            return _context.Cars.Include(car => car.User).Include(car => car.Connectors).Where(car => car.User.Equals(user)).ToList();
        }

        public void RemoveById(int id)
        {
            Car car = GetById(id);
            if(car != null)
                Remove(car);
            _context.SaveChanges();
        }
    }
}
