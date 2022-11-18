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

        public async Task SaveSettingsAsync(User user)
        {
            await repository.UpdateAsync(user);
        }
    }
}
