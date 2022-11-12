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



        public async Task<Car> GetById(int id)
        {
            return await _context.Cars.Include(car => car.ConnectorInterfaces).Where(car => car.Id == id).SingleOrDefaultAsync();
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
                dbCar.ConnectorInterfaces = car.ConnectorInterfaces;
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
            return await _context.Cars.Include(car => car.ConnectorInterfaces).Include(car => car.User).ToListAsync();
        }

        public async Task<List<Car>> GetDefaultCars()
        {
            return await _context.Cars.Include(car => car.User).Include(car => car.ConnectorInterfaces).Where(car => car.User.Equals(null)).ToListAsync();
        }

        public async Task<List<Car>> GetCarsByUser(User user)
        {
            return await _context.Cars.Include(car => car.User).Include(car => car.ConnectorInterfaces).Where(car => car.User.Equals(user)).ToListAsync();
        }

        public async Task RemoveById(int id)
        {
            Car car = await GetById(id);
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
            Car dbCar = await GetById(carId);
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
