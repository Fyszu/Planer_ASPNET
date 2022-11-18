using RoutePlanner.Data;
using Microsoft.EntityFrameworkCore;

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
            return await context.Users.FindAsync(id);
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
            if (user != null)
                context.Users.Remove(user);
            await context.SaveChangesAsync();
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
