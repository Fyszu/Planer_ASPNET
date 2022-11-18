using RoutePlanner.Data;

namespace RoutePlanner.Services
{
    public interface IDistanceService
    {
        // Get real maximum distance in meters
        public string GetRealMaximumDistance(int currentBatteryLevel, int maximumDistance, DataHelper.DrivingStyle drivingStyle, float temperature);
    }
}
