namespace RoutePlanner.Data
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public partial class WeatherAPIResponse
    {
        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("generationtime_ms")]
        public double GenerationtimeMs { get; set; }

        [JsonProperty("utc_offset_seconds")]
        public long UtcOffsetSeconds { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("timezone_abbreviation")]
        public string TimezoneAbbreviation { get; set; }

        [JsonProperty("elevation")]
        public double Elevation { get; set; }

        [JsonProperty("hourly_units")]
        public HourlyUnits HourlyUnits { get; set; }

        [JsonProperty("hourly")]
        public Hourly Hourly { get; set; }
    }

    public partial class Hourly
    {
        [JsonProperty("time")]
        public List<string> Time { get; set; }

        [JsonProperty("temperature_2m")]
        public List<double> Temperature_2M { get; set; }
    }

    public partial class HourlyUnits
    {
        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("temperature_2m")]
        public string Temperature2M { get; set; }
    }
}
