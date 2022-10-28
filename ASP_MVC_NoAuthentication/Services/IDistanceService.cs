using ASP_MVC_NoAuthentication.Data;

namespace ASP_MVC_NoAuthentication.Services
{
    public interface IDistanceService
    {
        // Get real maximum distance in meters
        public int GetRealMaximumDistance(int currentBatteryLevel, int maximumDistance, DataHelper.DrivingStyle drivingStyle, float temperature);
    }
}
