using ASP_MVC_NoAuthentication.Data;
using ASP_MVC_NoAuthentication.Services;
using Microsoft.AspNetCore.Mvc;

namespace ASP_MVC_NoAuthentication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [RestrictDomain("localhost", "planertras.com")]
    public class DistanceController : Controller
    {

        private readonly IDistanceService distanceService;
        private readonly IWeatherAPIService weatherService;

        public DistanceController(IDistanceService service, IWeatherAPIService weatherService)
        {
            distanceService = service;
            this.weatherService = weatherService;
        }

        [HttpGet("GetRealDistance")]
        public async Task<string> GetRealDistance([FromQuery] int maximumDistance, int batteryLevel, float origLat, float origLng, float destLat, float destLng, int estimatedTravelTime, int drivingStyle)
        {
            estimatedTravelTime = estimatedTravelTime / 3600; // seconds -> hours
            estimatedTravelTime = estimatedTravelTime == 0 ? 1 : estimatedTravelTime;
            estimatedTravelTime = estimatedTravelTime > 24 ? 24 : estimatedTravelTime;
            float temperatureOfOrigin = await weatherService.GetTemperatureForLocationAsync(origLat, origLng, estimatedTravelTime);
            float temperatureOfDestination = await weatherService.GetTemperatureForLocationAsync(destLat, destLng, estimatedTravelTime);
            if (temperatureOfOrigin != -300f && temperatureOfDestination != -300f) // -300 is error code for weather service
            {
                return distanceService.GetRealMaximumDistanceAsync(batteryLevel, maximumDistance, (DataHelper.DrivingStyle)drivingStyle, (temperatureOfDestination + temperatureOfOrigin) / 2);
            }
            else
            {
                InternalApiResponse internalApiResponse = new(InternalApiResponse.StatusCode.DistanceControllerWeatherAPIError, null);
                return internalApiResponse.GetInternalResponseJson();
            }
        }
    }
}
