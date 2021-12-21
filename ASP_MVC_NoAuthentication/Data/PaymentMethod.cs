namespace ASP_MVC_NoAuthentication.Data
{
    public class PaymentMethod
    {
        public PaymentMethod()
        {
            this.ChargingStations = new HashSet<ChargingStation>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ChargingStation> ChargingStations { get; set; }
    }
}
