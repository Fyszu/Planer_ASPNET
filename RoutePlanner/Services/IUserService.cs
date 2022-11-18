using RoutePlanner.Data;

namespace RoutePlanner.Services
{
    public interface IUserService
    {
        public Task<User> GetUserByNameAsync(string name);
        public Task SaveSettingsAsync(User user);
    }
}
