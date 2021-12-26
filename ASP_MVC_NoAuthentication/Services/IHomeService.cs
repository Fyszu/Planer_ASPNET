using ASP_MVC_NoAuthentication.Data;

namespace ASP_MVC_NoAuthentication.Services
{
	public interface IHomeService
	{
		List<Car> getUserCars(string userName);
		List<Car> getDefaultCars();
	}
}
