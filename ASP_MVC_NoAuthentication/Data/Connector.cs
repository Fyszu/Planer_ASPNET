namespace ASP_MVC_NoAuthentication.Data
{
    public class Connector
    {
        public Connector()
        {
            this.Cars = new HashSet<Car>();
            this.ChargingPoints = new HashSet<ChargingPoint>();
        }
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Car> Cars { get; set; }
        public ICollection<PersonalCar> PersonalCars { get; set;}
        public ICollection<ChargingPoint> ChargingPoints { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
