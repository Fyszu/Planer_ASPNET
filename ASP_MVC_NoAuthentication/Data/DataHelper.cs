namespace ASP_MVC_NoAuthentication.Data
{
    public static class DataHelper
    {
        public enum DrivingStyle
        {
            None = 0,
            City = 1, // 50km/h - 70km/h
            Combined = 2, // 70km/h - 120km/h
            HighwaySlow = 3, // up to 120km/h
            HighwayNormal = 4, // up to 140km/h
            HighwayFast = 5 // faster than 140km/h
        }

    }
}
