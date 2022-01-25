using ASP_MVC_NoAuthentication.Data;

namespace ASP_MVC_NoAuthentication.Services
{
    public interface IDistanceService
    {
        public int getRealMaximumDistance(int currentBatteryLevel, int maximumDistance, String? userName);
        public double getNumberOfRecharges(double routeDistance, int maximumDistance);
        public double[] FindPointAtDistanceFrom(double[] startPoint, double initialBearingRadians, double distanceKilometres);
        public double DegreesToRadians(double degrees);
        public double RadiansToDegrees(double radians);
    }
}
