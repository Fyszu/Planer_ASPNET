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
            return _context.Cars.Include(c => c.Connectors).ToList();
        }
    }
}
