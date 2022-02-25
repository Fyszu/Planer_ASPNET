using ASP_MVC_NoAuthentication.Data;
using Microsoft.EntityFrameworkCore;

namespace ASP_MVC_NoAuthentication.Repositories
{
	public class UserRepository
	{
        private readonly MyDbContext _context;
        public UserRepository(MyDbContext context)
        {
            _context = context;
        }


        public User GetUserByName(string name)
        {
            return _context.Users.Where(u => u.UserName == name).FirstOrDefault();
        }

        public User GetUserById(string id)
        {
            return _context.Users.Find(id);
        }

        public void UpdateUser(User user)
        {
            User dbUser = _context.Users.Find(user.Id);
            if (dbUser != null)
            {
                dbUser.SummerFactor = user.SummerFactor;
                dbUser.WinterFactor = user.WinterFactor;
                dbUser.DrivingStyle = user.DrivingStyle;
                _context.SaveChanges();
            }

        }
    }
}
