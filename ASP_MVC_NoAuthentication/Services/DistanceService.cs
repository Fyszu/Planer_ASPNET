using ASP_MVC_NoAuthentication.Data;
using ASP_MVC_NoAuthentication.Repositories;

namespace ASP_MVC_NoAuthentication.Services
{
    public class DistanceService : IDistanceService
    {
        private readonly ILogger<DistanceService> _logger;

        public DistanceService(ILogger<DistanceService> logger)
        {
            _logger = logger;
        }

        // Get real maximum distance in meters
        public string GetRealMaximumDistance(int batteryLevel, int maximumDistance, DataHelper.DrivingStyle drivingStyle, float temperature) 
        {
            float realDistance = ((float)batteryLevel / 100f) * (maximumDistance * 1000); // *1000: conversion to meters, /100: conversion to precentage
            realDistance = CalculateDistanceByTemperature(realDistance, temperature);
            realDistance = CalculateDistanceByDrivingStyle(realDistance, drivingStyle);

            try
            {
                var result = ((int)Math.Round(realDistance)).ToString();
                InternalApiResponse internalApiResponse = new(InternalApiResponse.StatusCode.OK, result);
                return internalApiResponse.GetInternalResponseJson();
            }
            catch (Exception ex)
            {
                _logger.LogError("Problem podczas przetwarzania wyniku obliczeń dystansu samochodu. " +
                    $"Poziom baterii: {batteryLevel}, maksymalny dystans: {maximumDistance}, styl jazdy: {drivingStyle}, temperatura: {temperature}");
                return new InternalApiResponse(InternalApiResponse.StatusCode.DistanceControllerOtherError, null).GetInternalResponseJson();
            }
        }

        private float CalculateDistanceByTemperature(float distance, float temperature)
        {
            switch (temperature)
            {
                case <= -24:
                    return distance * 0.45f;

                case <= -22:
                    return distance * 0.48f;

                case <= -20:
                    return distance * 0.5f;

                case <= -18:
                    return distance * 0.51f;

                case <= -16:
                    return distance * 0.53f;

                case <= -14:
                    return distance * 0.56f;

                case <= -12:
                    return distance * 0.58f;

                case <= -10:
                    return distance * 0.6f;

                case <= -8:
                    return distance * 0.62f;

                case <= -6:
                    return distance * 0.65f;

                case <= -4:
                    return distance * 0.71f;

                case <= -2:
                    return distance * 0.75f;

                case <= 0:
                    return distance * 0.78f;

                case <= 2:
                    return distance * 0.8f;

                case <= 4:
                    return distance * 0.84f;

                case <= 6:
                    return distance * 0.92f;

                case <= 8:
                    return distance * 0.97f;

                case <= 10:
                    return distance * 0.98f;

                case <= 32:
                    return distance;

                case <= 34:
                    return distance * 0.98f;

                case <= 35:
                    return distance * 0.92f;

                case <= 36:
                    return distance * 0.9f;

                case <= 37:
                    return distance * 0.88f;

                case <= 38:
                    return distance * 0.85f;

                case <= 39:
                    return distance * 0.83f;

                case <= 40:
                    return distance * 0.8f;

                case <= 41:
                    return distance * 0.77f;

                case <= 42:
                    return distance * 0.73f;

                case <= 43:
                    return distance * 0.7f;

                case <= 44:
                    return distance * 0.67f;

                case <= 45:
                    return distance * 0.65f;

                default:
                    return distance * 0.5f;
            }
        }

        private float CalculateDistanceByDrivingStyle(float distance, DataHelper.DrivingStyle drivingStyle)
        {
            switch(drivingStyle)
            {
                case DataHelper.DrivingStyle.City:
                    return distance;

                case DataHelper.DrivingStyle.Combined:
                    return distance * 0.88f;

                case DataHelper.DrivingStyle.HighwaySlow:
                    return distance * 0.8f;

                case DataHelper.DrivingStyle.HighwayNormal:
                    return distance * 0.7f;

                case DataHelper.DrivingStyle.HighwayFast:
                    return distance * 0.5f;

                default:
                    return distance * 0.5f;
            }
        }
    }
}
