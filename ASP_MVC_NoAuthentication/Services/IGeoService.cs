using Newtonsoft.Json.Linq;

namespace ASP_MVC_NoAuthentication.Services
{
    public interface IGeoService
    {
        public Task<string> GetAddress(string key, string longitude, string latitude);
        public Task<string> GetCoordinates(string key, string address);
    }
}
