using ASP_MVC_NoAuthentication.Data;

namespace ASP_MVC_NoAuthentication.Services
{
    public interface ICarService
    {
        public List<Car> GetDefaultCars();
        public List<Car> GetCarsByUser(String userName);
        public Car GetCarById(int id);
        public void RemoveCarByUser(string userName, int carId);
        public void AddNewCar(Car car);
        public void UpdateCar(Car car);
        public Boolean CheckIfCarBelongsToUser(User user, Car car);
    }
}
