using System.ComponentModel.DataAnnotations;

namespace ASP_MVC_NoAuthentication.Data
{
    public class PersonalCar
    {
        public PersonalCar()
        {
            this.Connectors = new HashSet<Connector>();
        }
        public int Id { set; get; }
        public String Brand { set; get; }
        public String Model { set; get; }
        public int MaximumDistance { set; get; }
        public ICollection<Connector> Connectors { get; set; }

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

        public Car toCar()
		{
            return new Car(this.Id, this.Brand, this.Model, this.MaximumDistance, this.Connectors);
		}
    }
}
