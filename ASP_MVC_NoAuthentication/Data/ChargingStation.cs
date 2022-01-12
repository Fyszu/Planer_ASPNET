using System.ComponentModel.DataAnnotations.Schema;

namespace ASP_MVC_NoAuthentication.Data
{
    public class ChargingStation
    {
        public ChargingStation()
        {
            this.PaymentMethods = new HashSet<PaymentMethod>();
            this.ChargingPoints = new HashSet<ChargingPoint>();
        }
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public String Name { get; set; }
        public String PostalAdress { get; set; }
        public String City { get; set; }
        public String Street { get; set; }
        public Boolean StationState { get; set; }
        public String OpenHours { get; set; }
        public String Owner { get; set; }
        public virtual ICollection<PaymentMethod> PaymentMethods { get; set; }
        public virtual ICollection<ChargingPoint> ChargingPoints { get; set; }
    }
}
