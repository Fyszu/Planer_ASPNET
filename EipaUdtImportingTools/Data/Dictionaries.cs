namespace EipaUdtImportingTools.Data
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using System.Text.Json.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Dictionaries
    {
        [JsonPropertyName("charging_mode")]
        public List<ChargingMode> ChargingMode { get; set; }

        [JsonPropertyName("connector_interface")]
        public List<ConnectorInterface> ConnectorInterface { get; set; }

        [JsonPropertyName("fuel_type")]
        public List<ConnectorInterface> FuelType { get; set; }

        [JsonPropertyName("gas_connector_interface")]
        public List<ConnectorInterface> GasConnectorInterface { get; set; }

        [JsonPropertyName("hydrogen_refill_solution")]
        public List<ConnectorInterface> HydrogenRefillSolution { get; set; }

        [JsonPropertyName("station_authentication_method")]
        public List<StationMethod> StationAuthenticationMethod { get; set; }

        [JsonPropertyName("station_payment_method")]
        public List<StationMethod> StationPaymentMethod { get; set; }

        [JsonPropertyName("weekday")]
        public List<ChargingMode> Weekday { get; set; }

        [JsonPropertyName("company_type")]
        public List<ChargingMode> CompanyType { get; set; }

        [JsonPropertyName("country")]
        public List<Country> Country { get; set; }
    }

    public partial class ChargingMode
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public partial class ConnectorInterface
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public partial class Country
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public partial class StationMethod
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }
    }
}
