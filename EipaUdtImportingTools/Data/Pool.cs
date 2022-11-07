namespace EipaUdtImportingTools.Data
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using System.Text.Json.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Pool
    {
        [JsonPropertyName("data")]
        public List<PoolData> Data { get; set; }

        [JsonPropertyName("generated")]
        public DateTimeOffset Generated { get; set; }
    }

    public partial class PoolData
    {
        [JsonPropertyName("operator_id")]
        public long OperatorId { get; set; }

        [JsonPropertyName("charging")]
        public bool Charging { get; set; }

        [JsonPropertyName("filling")]
        public bool Filling { get; set; }

        [JsonPropertyName("refilling")]
        public bool Refilling { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("closing_hours")]
        public List<ClosingHour> ClosingHours { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("elevation")]
        public long? Elevation { get; set; }

        [JsonPropertyName("features")]
        public List<string> Features { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("house_number")]
        public string HouseNumber { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("images")]
        public List<object> Images { get; set; }

        [JsonPropertyName("legalized")]
        public bool Legalized { get; set; }

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("operating_hours")]
        public List<OperatingHour> OperatingHours { get; set; }

        [JsonPropertyName("postal_code")]
        public string PostalCode { get; set; }

        [JsonPropertyName("street")]
        public string Street { get; set; }

        [JsonPropertyName("ts")]
        public DateTimeOffset Ts { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("accessibility")]
        public string Accessibility { get; set; }
    }

    public partial class ClosingHour
    {
        [JsonPropertyName("from_time")]
        public DateTimeOffset FromTime { get; set; }

        [JsonPropertyName("to_time")]
        public DateTimeOffset ToTime { get; set; }
    }

    public partial class OperatingHour
    {
        [JsonPropertyName("from_time")]
        public string FromTime { get; set; }

        [JsonPropertyName("to_time")]
        public string ToTime { get; set; }

        [JsonPropertyName("weekday")]
        public long Weekday { get; set; }
    }
}