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



        public async Task<Car> GetById(int id)
        {
            return await _context.Cars.Include(car => car.Connectors).Where(car => car.Id == id).SingleOrDefaultAsync();
        }

        public async Task Add(Car car)
        {
            if(car != null)
                await _context.Cars.AddAsync(car);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Car car)
        {
            if (car != null)
            {
                Car dbCar = await GetById(car.Id);
                dbCar.Brand = car.Brand;
                dbCar.Model = car.Model;
                dbCar.MaximumDistance = car.MaximumDistance;
                dbCar.Connectors = car.Connectors;
            }
            await _context.SaveChangesAsync();
        }

        public async Task Remove(Car car)
        {
            if(car != null)
            {
                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Car>> GetAll()
        {
            return await _context.Cars.ToListAsync();
        }

        public async Task<List<Car>> GetDefaultCars()
        {
            return await _context.Cars.Include(car => car.User).Include(car => car.Connectors).Where(car => car.User.Equals(null)).ToListAsync();
        }

        public async Task<List<Car>> GetCarsByUser(User user)
        {
            return await _context.Cars.Include(car => car.User).Include(car => car.Connectors).Where(car => car.User.Equals(user)).ToListAsync();
        }

        public async Task RemoveById(int id)
        {
            Car car = await GetById(id);
            if(car != null)
                Remove(car);
            await _context.SaveChangesAsync();
        }
    }
}
