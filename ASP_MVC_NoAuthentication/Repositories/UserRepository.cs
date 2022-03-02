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



        public User GetById(string id)
        {
            return _context.Users.Find(id);
        }

        public void Add(User user)
        {
            if (user != null)
                _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Update(User user)
        {
            User dbUser = _context.Users.Find(user.Id);
            if (dbUser != null)
            {
                dbUser.SummerFactor = user.SummerFactor;
                dbUser.WinterFactor = user.WinterFactor;
                dbUser.DrivingStyle = user.DrivingStyle;
            }
            _context.SaveChanges();
        }

        public void Remove(User user)
        {
            if (user != null)
                _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User GetByName(string name)
        {
            return _context.Users.Where(u => u.UserName == name).SingleOrDefault();
        }
    }
}
