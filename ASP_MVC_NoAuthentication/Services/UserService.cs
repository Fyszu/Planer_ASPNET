using ASP_MVC_NoAuthentication.Data;
using ASP_MVC_NoAuthentication.Repositories;

namespace ASP_MVC_NoAuthentication.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IUserRepository _repository;

        public UserService(ILogger<UserService> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _repository = userRepository;
        }

        public async Task<User> GetUserByName(string name)
        {
            return await _repository.GetByName(name);
        }

        public async Task SaveSettings(User user)
        {
            await _repository.Update(user);
        }
    }
}
