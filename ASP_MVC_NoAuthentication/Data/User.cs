using Microsoft.AspNetCore.Identity;

namespace ASP_MVC_NoAuthentication.Data
{
    public class User : IdentityUser
    {
        public User()
        {
        }
        public string DrivingStyle { set; get; }
        public double WinterFactor { set; get; }
        public double SummerFactor { set; get; }
        public string HighwaySpeed { set; get; }
        public ICollection<PersonalCar> PersonalCars { get; set;}

        public List<Car> getCars()
		{
            List<Car> cars = new List<Car>();
            foreach(PersonalCar personalCar in this.PersonalCars)
                cars.Add(new Car(personalCar.Id, personalCar.Brand, personalCar.Brand, personalCar.MaximumDistance, personalCar.Connectors));
            return cars;
		}
    }
}
