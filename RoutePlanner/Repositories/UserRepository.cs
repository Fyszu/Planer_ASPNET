using RoutePlanner.Data;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace RoutePlanner.Repositories
{
	public class UserRepository : IUserRepository
	{
        private readonly MyDbContext context;
        public UserRepository(MyDbContext context)
        {
            this.context = context;
        }



        public async Task<User> GetByIdAsync(string id)
        {
            return await context.Users.Include(user => user.Cars).ThenInclude(car => car.ConnectorInterfaces).Where(u => u.Id == id).SingleOrDefaultAsync();
        }

        public async Task AddAsync(User user)
        {
            if (user != null)
                await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            User dbUser = context.Users.Find(user.Id);
            if (dbUser != null)
            {
                dbUser.ShowOnlyMyCars = user.ShowOnlyMyCars;
            }
            await context.SaveChangesAsync();
        }

        public async Task RemoveAsync(User user)
        {
            if (user != null && user.Cars != null)
            {
                foreach (Car car in user.Cars)
                {
                    if (car.ConnectorInterfaces == null)
                    {
                        throw new Exception($"Błąd podczas usuwania samochodu {car.Brand} {car.Model}.");
                    }
                }
                try
                {
                    using var transaction = await context.Database.BeginTransactionAsync();
                    context.Cars.RemoveRange(user.Cars);
                    context.Users.Remove(user);
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception("Problem podczas usuwania użytkownika i jego samochodów.", new Exception(ex.InnerException?.ToString()));
                }
            }
            else
            {
                throw new Exception("Błąd podczas usuwania użytkownika.", new Exception("Użytkownik lubj jego lista samochodów była nullem."));
            }
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await context.Users.ToListAsync();
        }

        public async Task<User> GetByNameAsync(string name)
        {
            return await context.Users.Include(user => user.Cars).ThenInclude(car => car.ConnectorInterfaces).Where(u => u.UserName == name).SingleOrDefaultAsync();
        }
    }
}
