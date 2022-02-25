using ASP_MVC_NoAuthentication.Data;
using ASP_MVC_NoAuthentication.Repositories;

namespace ASP_MVC_NoAuthentication.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly UserRepository _repository;

        public UserService(ILogger<UserService> logger, UserRepository userRepository, CarRepository carRepository)
        {
            _logger = logger;
            _repository = userRepository;
        }

        public User GetUserByName(string name)
        {
            return _repository.GetUserByName(name);
        }

        public void SaveSettings(double summerFactor, double winterFactor, string drivingStyle, string userId)
        {
            _repository.UpdateUser(new User(userId, drivingStyle, winterFactor, summerFactor, "default"));
        }
    }
}
