using ASP_MVC_NoAuthentication.Data;

namespace ASP_MVC_NoAuthentication.Services
{
    public interface IUserService
    {
        public Task<User> GetUserByNameAsync(string name);
        public Task SaveSettingsAsync(User user);
    }
}
