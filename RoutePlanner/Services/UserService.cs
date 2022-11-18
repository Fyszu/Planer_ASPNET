using RoutePlanner.Data;
using RoutePlanner.Repositories;

namespace RoutePlanner.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository repository;

        public UserService(IUserRepository userRepository)
        {
            repository = userRepository;
        }

        public async Task<User> GetUserByNameAsync(string name)
        {
            return await repository.GetByNameAsync(name);
        }

        public async Task<User> GetByIdAsync(string userId)
        {
            return await repository.GetByIdAsync(userId);
        }

        public async Task DeleteAsync(User user)
        {

            await repository.RemoveAsync(user);
        }

        public async Task SaveSettingsAsync(User user)
        {
            await repository.UpdateAsync(user);
        }
    }
}
