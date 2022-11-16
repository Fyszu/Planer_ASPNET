using ASP_MVC_NoAuthentication.Data;
using Microsoft.EntityFrameworkCore;

namespace ASP_MVC_NoAuthentication.Repositories
{
	public class UserRepository : IUserRepository
	{
        private readonly MyDbContext _context;
        public UserRepository(MyDbContext context)
        {
            _context = context;
        }



        public async Task<User> GetByIdAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task AddAsync(User user)
        {
            if (user != null)
                await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            User dbUser = _context.Users.Find(user.Id);
            if (dbUser != null)
            {
                dbUser.ShowOnlyMyCars = user.ShowOnlyMyCars;
            }
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(User user)
        {
            if (user != null)
                _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetByNameAsync(string name)
        {
            return await _context.Users.Include(user => user.Cars).ThenInclude(car => car.ConnectorInterfaces).Where(u => u.UserName == name).SingleOrDefaultAsync();
        }
    }
}
