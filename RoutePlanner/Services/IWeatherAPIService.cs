namespace RoutePlanner.Services
{
    public interface IWeatherApiService
    {
        public Task<float> GetTemperatureForLocationAsync(float lat, float lng, int estimatedTravelTime);
    }
}
