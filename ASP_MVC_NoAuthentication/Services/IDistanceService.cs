using ASP_MVC_NoAuthentication.Data;

namespace ASP_MVC_NoAuthentication.Services
{
    public interface IDistanceService
    {
        public Task<int> getRealMaximumDistance(int currentBatteryLevel, int maximumDistance, String? userName);
        public int getNumberOfRecharges(double routeDistance, int maximumDistance, int batteryLevel);
        public double[] FindPointAtDistanceFrom(double[] startPoint, double initialBearingRadians, double distanceKilometres);
        public double DegreesToRadians(double degrees);
        public double RadiansToDegrees(double radians);
    }
}
