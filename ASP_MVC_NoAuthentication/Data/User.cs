using Microsoft.AspNetCore.Identity;

namespace ASP_MVC_NoAuthentication.Data
{
    public class User : IdentityUser
    {
        public User()
        {
            this.PersonalCars = new HashSet<PersonalCar>();
        }
        public string DrivingStyle { set; get; }
        public double WinterFactor { set; get; }
        public double SummerFactor { set; get; }
        public string HighwaySpeed { set; get; }
        public ICollection<PersonalCar> PersonalCars { get; set;}
    }
}
