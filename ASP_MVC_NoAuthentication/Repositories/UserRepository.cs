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

        public List<PersonalCar> getPersonalCars(string id)
		{
            return _context.PersonalCars.Where(p => p.User.Id.Equals(id)).Include(c => c.Connectors).ToList();
		}
    }
}
