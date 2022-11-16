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



        public async Task<User> GetById(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task Add(User user)
        {
            if (user != null)
                await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task Update(User user)
        {
            User dbUser = _context.Users.Find(user.Id);
            if (dbUser != null)
            {
                dbUser.ShowOnlyMyCars = user.ShowOnlyMyCars;
            }
            await _context.SaveChangesAsync();
        }

        public async Task Remove(User user)
        {
            if (user != null)
                _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetByName(string name)
        {
            return await _context.Users.Include(user => user.Cars).ThenInclude(car => car.ConnectorInterfaces).Where(u => u.UserName == name).SingleOrDefaultAsync();
        }
    }
}
