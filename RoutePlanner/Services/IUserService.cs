using RoutePlanner.Data;

namespace RoutePlanner.Services
{
    public interface IUserService
    {
        public Task<User> GetUserByNameAsync(string name);
        public Task<User> GetByIdAsync(string userId);
        public Task DeleteAsync(User user);
        public Task SaveSettingsAsync(User user);
    }
}
