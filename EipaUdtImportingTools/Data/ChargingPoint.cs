namespace EipaUdtImportingTools.Data
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using System.Text.Json.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class ChargingPoint
    {
        [JsonPropertyName("data")]
        public List<ChargingPointData> Data { get; set; }

        [JsonPropertyName("generated")]
        public DateTimeOffset Generated { get; set; }
    }

    public partial class ChargingPointData
    {
        [JsonPropertyName("fuel_types")]
        public List<object> FuelTypes { get; set; }

        [JsonPropertyName("gas_connector_interfaces")]
        public List<object> GasConnectorInterfaces { get; set; }

        [JsonPropertyName("hydrogen_refill_solutions")]
        public List<object> HydrogenRefillSolutions { get; set; }

        [JsonPropertyName("station_id")]
        public long StationId { get; set; }

        [JsonPropertyName("charging_solutions")]
        public List<ChargingSolution> ChargingSolutions { get; set; }

        [JsonPropertyName("connectors")]
        public List<Connector> Connectors { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("ts")]
        public DateTimeOffset Ts { get; set; }
    }

    public partial class ChargingSolution
    {
        [JsonPropertyName("mode")]
        public long Mode { get; set; }

        [JsonPropertyName("power")]
        public long Power { get; set; }
    }

    public partial class Connector
    {
        [JsonPropertyName("interfaces")]
        public List<long> Interfaces { get; set; }

        [JsonPropertyName("cable_attached")]
        public bool CableAttached { get; set; }

        [JsonPropertyName("power")]
        public long Power { get; set; }

        [JsonPropertyName("ts")]
        public DateTimeOffset Ts { get; set; }
    }
}
