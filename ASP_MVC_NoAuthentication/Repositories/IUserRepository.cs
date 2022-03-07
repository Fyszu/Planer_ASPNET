using ASP_MVC_NoAuthentication.Data;

namespace ASP_MVC_NoAuthentication.Repositories
{
    public interface IUserRepository : IRepository<User, string>
    {
        public Task<User> GetByName(string name);
    }
}
