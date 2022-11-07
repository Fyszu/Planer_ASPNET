namespace EipaUdtImportingTools.Data
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using System.Text.Json.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Dynamic
    {
        [JsonPropertyName("data")]
        public List<DynamicData> Data { get; set; }

        [JsonPropertyName("generated")]
        public DateTimeOffset Generated { get; set; }
    }

    public partial class DynamicData
    {
        [JsonPropertyName("point_id")]
        public long PointId { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("prices")]
        public List<Price> Prices { get; set; }

        [JsonPropertyName("status")]
        public Status Status { get; set; }
    }

    public partial class Price
    {
        [JsonPropertyName("unit")]
        public string Unit { get; set; }

        [JsonPropertyName("ts")]
        public DateTimeOffset Ts { get; set; }

        [JsonPropertyName("literal")]
        public string Literal { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("price")]
        public string PricePrice { get; set; }
    }

    public partial class Status
    {
        [JsonPropertyName("availability")]
        public long Availability { get; set; }

        [JsonPropertyName("status")]
        public long StatusStatus { get; set; }

        [JsonPropertyName("ts")]
        public DateTimeOffset Ts { get; set; }
    }
}
