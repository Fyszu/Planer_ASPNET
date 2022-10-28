namespace ASP_MVC_NoAuthentication.Services
{
    public interface IWeatherAPIService
    {
        public Task<float> GetTemperatureForLocation(float lat, float lng, int estimatedTravelTime);
    }
}
