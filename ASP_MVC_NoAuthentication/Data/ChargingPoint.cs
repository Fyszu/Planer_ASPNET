using System.ComponentModel.DataAnnotations;

namespace ASP_MVC_NoAuthentication.Data
{
    public class ChargingPoint
    {
        public ChargingPoint()
        {
            Connectors = new HashSet<Connector>();
        }
        public long Id { get; set; }
        public string? ChargingModes { get; set; }
        public double? Price { get; set; }
        public string? PriceUnit { get; set; }
        [Required]
        public bool Status { get; set; }
        [Required]
        public ChargingStation Station { set; get; }
        public ICollection<Connector> Connectors { get; set; }
        public partial class Connector
        {
            public Connector()
            {
                Interfaces = new HashSet<ConnectorInterface>();
            }
            public long Id { get; set; }
            public ICollection<ConnectorInterface> Interfaces { get; set; }
            [Required]
            public bool CableAttached { get; set; }
            public long? ChargingPower { get; set; }
        }

        public void AddChargingMode(string chargingMode)
        {
            if (string.IsNullOrEmpty(ChargingModes))
                ChargingModes = chargingMode;
            else
                ChargingModes += ";;" + chargingMode;
        }

        public void RemoveChargingMode(string chargingMode)
        {
            if (!string.IsNullOrEmpty(ChargingModes) && ChargingModes.Contains(chargingMode))
            {
                ChargingModes = ChargingModes.Replace(";;" + chargingMode, "");
            }
        }

        public List<string> GetChargingModesList()
        {
            if (!string.IsNullOrEmpty(ChargingModes))
                return ChargingModes.Split(";;").ToList();
            else
                return new List<string>();
        }
    }
}
