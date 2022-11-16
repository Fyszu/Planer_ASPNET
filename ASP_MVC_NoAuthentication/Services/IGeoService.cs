using Newtonsoft.Json.Linq;

namespace ASP_MVC_NoAuthentication.Services
{
    public interface IGeoService
    {
        public Task<string> GetAddressAsync(string longitude, string latitude);
        public Task<string> GetCoordinatesAsync(string address);
    }
}
