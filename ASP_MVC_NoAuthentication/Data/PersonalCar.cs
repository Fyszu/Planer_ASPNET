using System.ComponentModel.DataAnnotations;

namespace ASP_MVC_NoAuthentication.Data
{
    public class PersonalCar : Car
    {
        public PersonalCar()
        {
            this.Connectors = new HashSet<Connector>();
        }
        [Required]
        public User User { set; get; }

        public PersonalCar(int id, string brand, string model, int maximumDistance, ICollection<Connector> connectors, User user)
        {
            this.Id = id;
            this.Brand = brand;
            this.Model = model;
            this.MaximumDistance = maximumDistance;
            this.Connectors = new HashSet<Connector>(connectors);
            this.User = user;
        }
        public PersonalCar(string brand, string model, int maximumDistance, ICollection<Connector> connectors, User user)
        {
            this.Brand = brand;
            this.Model = model;
            this.MaximumDistance = maximumDistance;
            this.Connectors = new HashSet<Connector>(connectors);
            this.User = user;
        }

        public override string ToString()
        {
            return $"{Brand} {Model} ({MaximumDistance}, {Connectors})";
        }

        public Car ToCar()
		{
            return new Car(this.Id, this.Brand, this.Model, this.MaximumDistance, this.Connectors);
		}
    }
}
