﻿using RoutePlanner.Data;
using RoutePlanner.Services;
using Microsoft.AspNetCore.Mvc;

namespace RoutePlanner.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Filters.RestrictDomain("localhost", "planertras.com")]
    public class DistanceController : Controller
    {

        private readonly IDistanceService distanceService;
        private readonly IWeatherApiService weatherService;

        public DistanceController(IDistanceService service, IWeatherApiService weatherService)
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
                return distanceService.GetRealMaximumDistance(batteryLevel, maximumDistance, (DataHelper.DrivingStyle)drivingStyle, (temperatureOfDestination + temperatureOfOrigin) / 2);
            }
            else
            {
                InternalApiResponse internalApiResponse = new(InternalApiResponse.StatusCode.DistanceControllerWeatherAPIError, null);
                return internalApiResponse.GetInternalResponseJson();
            }
        }
    }
}
