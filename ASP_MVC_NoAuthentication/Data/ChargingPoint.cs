namespace ASP_MVC_NoAuthentication.Data
{
    public class ChargingPoint
    {
        public int Id { get; set; }
        public String TypeOfCurrent { get; set; }
        public int Power { get; set; }
        public Connector Connector { get; set; }
        public ChargingStation Station { set; get; }
    }
}
