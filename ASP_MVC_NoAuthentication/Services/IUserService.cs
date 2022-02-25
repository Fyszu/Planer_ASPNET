using ASP_MVC_NoAuthentication.Data;

namespace ASP_MVC_NoAuthentication.Services
{
    public interface IUserService
    {
        public User GetUserByName(string name);
        public void SaveSettings(double summerFactor, double winterFactor, string drivingStyle, string userId);
    }
}
