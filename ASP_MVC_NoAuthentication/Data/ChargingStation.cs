using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP_MVC_NoAuthentication.Data
{
    public class ChargingStation
    {
        public ChargingStation()
        {
            OperatingHours = new HashSet<OperatingHour>();
            ChargingPoints = new HashSet<ChargingPoint>();
        }
        public long Id { get; set; }
        public Provider? Provider { get; set; }
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }
        [Required]
        public string Name { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? Street { get; set; }
        public string? HouseNumber { get; set; }
        public string? Community { get; set; }
        public string? District { get; set; }
        public string? Province { get; set; }
        public ICollection<OperatingHour> OperatingHours { get; set; }
        public string? Accessibility { get; set; }
        public string? PaymentMethods { get; set; }
        public string? AuthenticationMethods { get; set; }
        public ICollection<ChargingPoint> ChargingPoints { get; set; }
        public partial class OperatingHour
        {
            public long Id { get; set; }
            public ChargingStation Station { get; set; }
            public bool WholeDay
            {
                get
                {
                    if (FromTime.Equals("00:00") && ToTime.Equals("23:59"))
                        return true;
                    else
                        return false;
                }
            }
            public string FromTime { get; set; }
            public string ToTime { get; set; }
            public string Weekday { get; set; } // mapa weekday
        }

        public void AddPaymentMethod(string paymentMethod)
        {
            if (string.IsNullOrEmpty(PaymentMethods))
                PaymentMethods = paymentMethod;
            else
                PaymentMethods += ";;" + paymentMethod;
        }

        public void AddAuthenticationMethod(string authMethod)
        {
            if (string.IsNullOrEmpty(AuthenticationMethods))
                AuthenticationMethods = authMethod;
            else
                AuthenticationMethods += ";;" + authMethod;
        }

        public void RemovePaymentMethod(string paymentMethod)
        {
            if (PaymentMethods.Contains(paymentMethod))
            {
                PaymentMethods = PaymentMethods.Replace(";;" + paymentMethod, "");
            }
        }

        public void RemoveAuthenticationMethod(string authMethod)
        {
            if (AuthenticationMethods.Contains(authMethod))
            {
                AuthenticationMethods = AuthenticationMethods.Replace(";;" + authMethod, "");
            }
        }

        public List<string> GetPaymentMethodsList()
        {
            if (!string.IsNullOrEmpty(PaymentMethods))
            {
                return PaymentMethods.Split(";;").ToList();
            }
            else
            {
                return new List<string>();
            }
        }

        public List<string> GetAuthenticationMethodsList()
        {
            if(!string.IsNullOrEmpty(AuthenticationMethods))
            {
                return AuthenticationMethods.Split(";;").ToList();
            }
            else
            {
                return new List<string>();
            }
        }
    }
}
