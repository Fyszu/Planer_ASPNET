using Microsoft.AspNetCore.Identity;

namespace ASP_MVC_NoAuthentication.Data
{
    public class User : IdentityUser
    {
        public User()
        {
            Cars = new HashSet<Car>();
        }
        public string DrivingStyle { set; get; }
        public double WinterFactor { set; get; }
        public double SummerFactor { set; get; }
        public string HighwaySpeed { set; get; }
        public ICollection<Car> Cars { set; get; }
        public Boolean ShowOnlyMyCars { set; get; }
        public User(string id, string drivingStyle, double winterFactor, double summerFactor, string highwaySpeed, bool showOnlyMyCars)
        {
            this.Id = id;
            this.DrivingStyle = drivingStyle;
            this.WinterFactor = winterFactor;
            this.SummerFactor = summerFactor;
            this.HighwaySpeed = highwaySpeed;
            this.ShowOnlyMyCars = showOnlyMyCars;
        }
    }
}
