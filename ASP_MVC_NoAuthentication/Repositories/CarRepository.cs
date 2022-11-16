using ASP_MVC_NoAuthentication.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;

namespace ASP_MVC_NoAuthentication.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly MyDbContext _context;
        public CarRepository(MyDbContext context)
        {
            _context = context;
        }



        public async Task<Car> GetByIdAsync(int id)
        {
            return await _context.Cars.Include(car => car.ConnectorInterfaces).Where(car => car.Id == id).SingleOrDefaultAsync();
        }

        public async Task AddAsync(Car car)
        {
            if(car != null)
                await _context.Cars.AddAsync(car);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Car car)
        {
            if (car != null)
            {
                Car dbCar = await GetByIdAsync(car.Id);
                dbCar.Brand = car.Brand;
                dbCar.Model = car.Model;
                dbCar.MaximumDistance = car.MaximumDistance;
                dbCar.ConnectorInterfaces = car.ConnectorInterfaces;
            }
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(Car car)
        {
            if(car != null)
            {
                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Car>> GetAllAsync()
        {
            return await _context.Cars.Include(car => car.ConnectorInterfaces).Include(car => car.User).ToListAsync();
        }

        public async Task<List<Car>> GetDefaultCarsAsync()
        {
            return await _context.Cars.Include(car => car.User).Include(car => car.ConnectorInterfaces).Where(car => car.User.Equals(null)).ToListAsync();
        }

        public async Task<List<Car>> GetCarsByUserAsync(User user)
        {
            return await _context.Cars.Include(car => car.User).Include(car => car.ConnectorInterfaces).Where(car => car.User.Equals(user)).ToListAsync();
        }

        public async Task RemoveByIdAsync(int id)
        {
            Car car = await GetByIdAsync(id);
            if(car != null)
                _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
        }

        // Removes all records from Cars table
        public async Task RemoveAll()
        {
            await _context.Cars
                .ForEachAsync(car =>
                {
                    if (car != null)
                        _context.Cars.Remove(car);
                });
            await _context.SaveChangesAsync();
        }

        public async Task AddRange(List<Car> cars)
        {
            List<User> users = await _context.Users.ToListAsync();
            foreach (Car car in cars)
            {
                if (car.User != null)
                {
                    car.User = users.Where(user => car.User.Id.Equals(user.Id)).FirstOrDefault() ?? throw new Exception("Nie znaleziono użytkownika po ID na nowej liście.");
                }
            }
            await _context.Cars.AddRangeAsync(cars);
            HashSet<User> usersTracked = new();
            cars.ForEach(c => {
                if (c.User != null)
                {
                    _context.Entry(c.User).State = EntityState.Unchanged;
                }
            });
            await _context.SaveChangesAsync();
        }

        public async Task UpdateInterfaces(int carId, List<ConnectorInterface> connectorInterfaces)
        {
            Car dbCar = await GetByIdAsync(carId);
            if (dbCar != null)
            {
                dbCar.ConnectorInterfaces = connectorInterfaces;
                await _context.SaveChangesAsync();
            }
            else
                throw new Exception($"Zły numer id samochodu ({carId}).");
        }
    }
}
