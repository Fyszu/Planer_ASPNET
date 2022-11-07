using System.ComponentModel.DataAnnotations;

namespace ASP_MVC_NoAuthentication.Data
{
    public class Provider
    {
        public Provider()
        {
            ChargingStations = new HashSet<ChargingStation>();
        }
        public long Id { get; set; }
        public string? Type { get; set; } // mapa company_type 
        [Required]
        public string Name { get; set; }
        public string? ShortName { get; set; }
        public string? Phone { get; set; }
        public string? Code { get; set; }
        public string? Country { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public ICollection<ChargingStation> ChargingStations { get; set; }
    }
}
