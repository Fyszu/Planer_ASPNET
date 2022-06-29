using ASP_MVC_NoAuthentication.Data;
using ASP_MVC_NoAuthentication.Repositories;

namespace ASP_MVC_NoAuthentication.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly UserRepository _repository;

        public UserService(ILogger<UserService> logger, UserRepository userRepository)
        {
            _logger = logger;
            _repository = userRepository;
        }

        public async Task<User> GetUserByName(string name)
        {
            return await _repository.GetByName(name);
        }

        public async Task SaveSettings(double summerFactor, double winterFactor, string drivingStyle, string userId, Boolean showOnlyMyCars)
        {
            await _repository.Update(new User(userId, drivingStyle, winterFactor, summerFactor, "default", showOnlyMyCars));
        }
    }
}
