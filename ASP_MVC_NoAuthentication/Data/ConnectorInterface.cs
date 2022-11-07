namespace ASP_MVC_NoAuthentication.Data
{
    public class ConnectorInterface
    {
        public ConnectorInterface()
        {
            Cars = new HashSet<Car>();
            Connectors = new HashSet<ChargingPoint.Connector>();
        }
        
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Car> Cars { get; set; }
        public ICollection<ChargingPoint.Connector> Connectors { get; set; }
        public override string ToString()
        {
            return $"{Name} (ID: {Id}, opis: {Description})";
        }
    }
}
