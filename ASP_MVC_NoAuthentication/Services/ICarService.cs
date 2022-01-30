using ASP_MVC_NoAuthentication.Data;

namespace ASP_MVC_NoAuthentication.Services
{
    public interface ICarService
    {
        public void RemoveUserCar(User user, int carId);
        public int AddUserCar(User user, Car car);
        public PersonalCar getPersonalCarById(string userId, int carId);
        public void UpdateCar(User user, Car car);
    }
}
