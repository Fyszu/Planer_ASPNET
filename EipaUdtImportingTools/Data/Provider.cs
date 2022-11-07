namespace EipaUdtImportingTools.Data
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using System.Text.Json.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    // Class generated in purpose of parsing to C# object from JSON from UDT API
    public partial class Provider
    {
        [JsonPropertyName("data")]
        public List<ProviderData> Data { get; set; }

        [JsonPropertyName("generated")]
        public DateTimeOffset Generated { get; set; }
    }

    public partial class ProviderData
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("short_name")]
        public string ShortName { get; set; }

        [JsonPropertyName("type")]
        public long Type { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("website")]
        public string Website { get; set; }
    }
}
