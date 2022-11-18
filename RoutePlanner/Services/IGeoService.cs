using Newtonsoft.Json.Linq;

namespace RoutePlanner.Services
{
    public interface IGeoService
    {
        public Task<string> GetAddressAsync(string longitude, string latitude);
        public Task<string> GetCoordinatesAsync(string address);
    }
}
