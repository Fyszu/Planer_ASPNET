namespace EipaUdtImportingTools.Data
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using System.Text.Json.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class ChargingStation
    {
        [JsonPropertyName("data")]
        public List<ChargingStationData> Data { get; set; }

        [JsonPropertyName("generated")]
        public DateTimeOffset Generated { get; set; }
    }

    public partial class ChargingStationData
    {
        [JsonPropertyName("authentication_methods")]
        public List<long> AuthenticationMethods { get; set; }

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("payment_methods")]
        public List<long> PaymentMethods { get; set; }

        [JsonPropertyName("pool_id")]
        public long PoolId { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("images")]
        public List<object> Images { get; set; }

        [JsonPropertyName("location")]
        public Location Location { get; set; }

        [JsonPropertyName("ts")]
        public DateTimeOffset Ts { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public partial class Location
    {
        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("community")]
        public string Community { get; set; }

        [JsonPropertyName("district")]
        public string District { get; set; }

        [JsonPropertyName("province")]
        public string Province { get; set; }
    }
}
