using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP_MVC_NoAuthentication.Data
{
    public class ChargingStation
    {
        public ChargingStation()
        {
            OperatingHours = new List<OperatingHour>();
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
        public string OperatingHoursString { get { return GetOperatingHoursString(); } }
        public bool AllTimeOpen { get; set; }
        public string? Accessibility { get; set; }
        public string? PaymentMethods { get; set; }
        public string? AuthenticationMethods { get; set; }
        public ICollection<ChargingPoint> ChargingPoints { get; set; }
        public partial class OperatingHour
        {
            public long Id { get; set; }
            [JsonIgnore]
            public ChargingStation Station { get; set; }
            public bool OpenedWholeDay
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
                PaymentMethods += " | " + paymentMethod;
        }

        public void AddAuthenticationMethod(string authMethod)
        {
            if (string.IsNullOrEmpty(AuthenticationMethods))
                AuthenticationMethods = authMethod;
            else
                AuthenticationMethods += " | " + authMethod;
        }

        public string GetOperatingHoursString()
        {
            if (AllTimeOpen)
            {
                return "Czynne 24/7.";
            }
            else
            {
                OperatingHour? sampleOperatingHour = OperatingHours.FirstOrDefault();
                bool allOperatingHoursSame = true;

                if (sampleOperatingHour != null) {
                    foreach (var operatingHour in OperatingHours)
                    {
                        if (!operatingHour.FromTime.Equals(sampleOperatingHour.FromTime) || !operatingHour.ToTime.Equals(sampleOperatingHour.ToTime))
                        {
                            allOperatingHoursSame = false;
                            break;
                        }
                    }
                }
                else
                {
                    allOperatingHoursSame = false;
                }

                if (allOperatingHoursSame)
                {
                    return $"Codziennie od {sampleOperatingHour.FromTime} do {sampleOperatingHour.ToTime}";
                }

                string result = "";

                foreach (var operatingHour in OperatingHours)
                {
                    result += $"{char.ToUpper(operatingHour.Weekday[0])}{operatingHour.Weekday[1..]}: ";
                    if (operatingHour.OpenedWholeDay)
                    {
                        result += "Czynne całodobowo";
                    }
                    else
                    {
                        result += $"{operatingHour.FromTime} - {operatingHour.ToTime}";
                    }
                    result += ", ";
                }
                if (string.IsNullOrEmpty(result) || result.Length < 8)
                {
                    return "Brak danych.";
                }
                else
                {
                    return result.Remove(result.Length - 2);
                }
            }
        }
    }
}
