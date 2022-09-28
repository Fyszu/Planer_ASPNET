using ASP_MVC_NoAuthentication.Data;
using ASP_MVC_NoAuthentication.Repositories;

namespace ASP_MVC_NoAuthentication.Services
{
    public class DistanceService : IDistanceService
    {
        private readonly ILogger<DistanceService> _logger;
        private readonly UserRepository _userRepository;

        public DistanceService(ILogger<DistanceService> logger, UserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task<int> getRealMaximumDistance(int currentBatteryLevel, int maximumDistance, String? userName) // string 
        {
            //wyparsować usera z nazwy
            double currentDistance = (maximumDistance * 1000) * (currentBatteryLevel * 0.01);
            if (userName == null)
                userName = "default";
            if (userName == "default")
                userName = "default@default.pl";
            User user = await _userRepository.GetByName(userName);
            switch (user.DrivingStyle)
            {
                case "ekonomiczny":
                    break;

                case "mieszany":
                    currentDistance = currentDistance * 0.9;
                    break;

                case "dynamiczny":
                    currentDistance = currentDistance * 0.8;
                    break;

                default:
                    return 0;
            }

            DateTime aktualnaData = DateTime.Now;
            var summerDateStartV = "1/6/" + aktualnaData.Year + " 0:00:00 AM";
            var summerDateEndV = "1/9/" + aktualnaData.Year + " 0:00:00 AM";
            var winterDateStartV = "1/11/" + aktualnaData.Year + " 0:00:00 AM";
            var winterDateEndV = "1/3/" + (aktualnaData.Year + 1) + " 0:00:00 AM";
            DateTime summerDateStart = DateTime.Parse(summerDateStartV, System.Globalization.CultureInfo.InvariantCulture);
            DateTime summerDateEnd = DateTime.Parse(summerDateEndV, System.Globalization.CultureInfo.InvariantCulture);
            DateTime winterDateStart = DateTime.Parse(winterDateStartV, System.Globalization.CultureInfo.InvariantCulture);
            DateTime winterDateEnd = DateTime.Parse(winterDateEndV, System.Globalization.CultureInfo.InvariantCulture);

            if (aktualnaData > summerDateStart && aktualnaData < summerDateEnd)
            {
                currentDistance = currentDistance * (1 - user.SummerFactor);
            }

            else if (aktualnaData > winterDateStart && aktualnaData < winterDateEnd)
            {
                currentDistance = currentDistance * (1 - user.WinterFactor);
            }

            return (int)Math.Round(currentDistance);
        }


        
        public int getNumberOfRecharges(double routeDistance, int maximumDistance, int batteryLevel)
        {
           /* double distance = routeDistance / maximumDistance;
          //  if ((route.Distance / maximumDistance) < 0.8) //Da radę dojechać na obecnym naładowaniu
          //  {
          //      double estimatedBatteryLevel = (maximumDistance - route.Distance) / maximumDistance;
          //      return estimatedBatteryLevel;
          //  }
            else if (distance < 0.8)
                return 1;
            else if (distance >= 0.8 && distance < 1.8)
                return 2;
            else if (distance >= 1.8 && distance < 2.8)
                return 3;*/
            return 4;
        }


        public double[] FindPointAtDistanceFrom(double[] startPoint, double initialBearingRadians, double distanceKilometres)
        {
            const double radiusEarthKilometres = 6371.01;
            var distRatio = distanceKilometres / radiusEarthKilometres;
            var distRatioSine = Math.Sin(distRatio);
            var distRatioCosine = Math.Cos(distRatio);

            var startLatRad = DegreesToRadians(startPoint[0]);
            var startLonRad = DegreesToRadians(startPoint[1]);

            var startLatCos = Math.Cos(startLatRad);
            var startLatSin = Math.Sin(startLatRad);

            var endLatRads = Math.Asin((startLatSin * distRatioCosine) + (startLatCos * distRatioSine * Math.Cos(initialBearingRadians)));
            var endLonRads = startLonRad + Math.Atan2(Math.Sin(initialBearingRadians) * distRatioSine * startLatCos, distRatioCosine - startLatSin * Math.Sin(endLatRads));

            double[] resultArray = new double[2] { endLatRads, endLonRads };

            return resultArray;
        }

        public double DegreesToRadians(double degrees)
        {
            const double degToRadFactor = Math.PI / 180;
            return degrees * degToRadFactor;
        }

        public double RadiansToDegrees(double radians)
        {
            const double radToDegFactor = 180 / Math.PI;
            return radians * radToDegFactor;
        }
    }
}
