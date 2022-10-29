using ASP_MVC_NoAuthentication.Data;
using ASP_MVC_NoAuthentication.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace ASP_MVC_NoAuthentication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DistanceController : Controller
    {

        private readonly IDistanceService _distanceService;
        private readonly IWeatherAPIService _weatherService;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public DistanceController(IDistanceService service, IWeatherAPIService weatherService, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _distanceService = service;
            _weatherService = weatherService;
            _signInManager = signInManager;
            _userManager = userManager;
        }


        [HttpGet("GetRealDistance")]
        public async Task<int> GetRealDistance([FromQuery] int maximumDistance, int batteryLevel, float origLat, float origLng, float destLat, float destLng, int estimatedTravelTime, int drivingStyle)
        {
            estimatedTravelTime = estimatedTravelTime / 3600; // seconds -> hours
            estimatedTravelTime = estimatedTravelTime == 0 ? 1 : estimatedTravelTime;
            estimatedTravelTime = estimatedTravelTime > 24 ? 24 : estimatedTravelTime;
            float temperatureOfOrigin = await _weatherService.GetTemperatureForLocation(origLat, origLng, estimatedTravelTime);
            float temperatureOfDestination = await _weatherService.GetTemperatureForLocation(destLat, destLng, estimatedTravelTime);
            if (temperatureOfOrigin != -300f && temperatureOfDestination != -300f)
            {
                return _distanceService.GetRealMaximumDistance(batteryLevel, maximumDistance, (DataHelper.DrivingStyle)drivingStyle, (temperatureOfDestination + temperatureOfOrigin) / 2);
            }
            else
            {
                return -1;
            }
        }
    }
}
