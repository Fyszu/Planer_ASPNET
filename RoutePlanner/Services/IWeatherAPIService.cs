namespace RoutePlanner.Services
{
    public interface IWeatherAPIService
    {
        public Task<float> GetTemperatureForLocationAsync(float lat, float lng, int estimatedTravelTime);
    }
}
