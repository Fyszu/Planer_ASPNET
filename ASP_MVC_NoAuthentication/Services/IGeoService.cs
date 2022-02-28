using Newtonsoft.Json.Linq;

namespace ASP_MVC_NoAuthentication.Services
{
    public interface IGeoService
    {
        string GetAddress(string key, string longitude, string latitude);
        string GetCoordinates(string key, string address);
    }
}
