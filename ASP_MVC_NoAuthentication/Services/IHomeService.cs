using ASP_MVC_NoAuthentication.Data;

namespace ASP_MVC_NoAuthentication.Services
{
	public interface IHomeService
	{
		List<Car> getUserCars(string userName);
		List<Car> getDefaultCars();
		public List<Car> getPersonalisableCars(string userName);
		public User getUser(string userName);
		public void saveSettings(double summerFactor, double winterFactor, string drivingStyle, string id);
	}
}
