using RoutePlanner.Data;
using EipaUdtImportingTools.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static RoutePlanner.Data.ChargingPoint;
using static RoutePlanner.Data.ChargingStation;

namespace EipaUdtImportingTools.Tools
{
    public static class EIPAConverter
    {
        private static readonly Dictionary<string, string> ApiUrls = new()
        {
            { "Providers","https://eipa.udt.gov.pl/reader/export-data/operator/741e1ead52e180631ea6bd9ac33112e0" },
            { "Pools","https://eipa.udt.gov.pl/reader/export-data/pool/741e1ead52e180631ea6bd9ac33112e0" },
            { "ChargingStations","https://eipa.udt.gov.pl/reader/export-data/station/741e1ead52e180631ea6bd9ac33112e0" },
            { "ChargingPoints","https://eipa.udt.gov.pl/reader/export-data/point/741e1ead52e180631ea6bd9ac33112e0" },
            { "Dictionaries","https://eipa.udt.gov.pl/reader/export-data/dictionary/741e1ead52e180631ea6bd9ac33112e0" },
            { "Dynamic","https://eipa.udt.gov.pl/reader/export-data/dynamic/741e1ead52e180631ea6bd9ac33112e0" }
        };

        private static readonly HttpClient client = new HttpClient();
        private static readonly ILogger logger = ApplicationLogging.CreateLogger("EIPAConverter");
        public async static Task<bool> TransformApiDataToInternal()
        {
            logger.LogTrace("Pobieranie danych z API.");
            List<ProviderData>? providersData = await GetProvidersData() ?? throw new ArgumentNullException(nameof(providersData));
            List<PoolData>? poolsData = await GetPoolsData() ?? throw new ArgumentNullException(nameof(poolsData));
            List<ChargingStationData>? chargingStationsData = await GetChargingStationsData() ?? throw new ArgumentNullException(nameof(chargingStationsData));
            List<ChargingPointData>? chargingPointsData = await GetChargingPointsData() ?? throw new ArgumentNullException(nameof(chargingPointsData));
            List<DynamicData>? dynamicData = await GetDynamicData() ?? throw new ArgumentNullException(nameof(dynamicData));
            Dictionaries? dictionaries = await GetDictionaries() ?? throw new ArgumentNullException(nameof(dictionaries));
            logger.LogTrace("Pobieranie danych z API zakończone.");

            try
            {
                // Initialize
                HashSet<RoutePlanner.Data.ConnectorInterface> internalConnectorInterfaces = new();
                HashSet<RoutePlanner.Data.Provider> internalProviders = new();
                HashSet<RoutePlanner.Data.ConnectorInterface> connectorInterfacesInUsage = new();

                logger.LogTrace("Pobieranie interfejsów ładowania ze słownika.");
                // Get connector interfaces from dictionaries
                foreach (var connectorInterface in dictionaries.ConnectorInterface)
                {
                    internalConnectorInterfaces.Add(
                        new RoutePlanner.Data.ConnectorInterface
                        {
                            Id = connectorInterface.Id,
                            Name = connectorInterface.Name,
                            Description = connectorInterface.Description
                        });
                }

                logger.LogTrace("Przetwarzanie stacji ładowania.");

                // Create charging stations
                HashSet<RoutePlanner.Data.ChargingStation> internalChargingStations = new();
                foreach (var poolData in poolsData)
                {
                    ChargingStationData? chargingStationData = null;

                    // Get first charging station in pool, as the pool may have more than one charging stations in the same place (don't want to have duplicates)
                    foreach (var data in chargingStationsData)
                    {
                        if (data.PoolId == poolData.Id)
                        {
                            chargingStationData = data;
                            break;
                        }
                    }

                    // E is a type for charging stations (exclude the unnecessary gas stations)
                    if (chargingStationData == null || !chargingStationData.Type.Equals("E"))
                        continue;

                    if (chargingStationData.Latitude == 0 || chargingStationData.Longitude == 0)
                        continue;

                    // Get charging station's services provider (operator)
                    RoutePlanner.Data.Provider? internalProvider = null;
                    internalProvider = internalProviders.Where(provider => provider.Id == poolData.OperatorId).SingleOrDefault();
                    if (internalProvider == null)
                    {
                        ProviderData? providerData = providersData.FirstOrDefault(providerData => providerData.Id == poolData.OperatorId);
                        if (providerData != null && providerData.Name != null)
                        {
                            string? companyType = dictionaries.CompanyType.FirstOrDefault(companyType => companyType.Id.Equals(providerData.Type))?.Name;
                            internalProvider = new RoutePlanner.Data.Provider()
                            {
                                Id = providerData.Id,
                                Name = providerData.Name,
                                ShortName = providerData.ShortName,
                                Type = companyType ?? "Brak",
                                Phone = providerData.Phone,
                                Code = providerData.Code,
                                Email = providerData.Email,
                                Website = providerData.Website,
                                Country = providerData.Country
                            };
                            internalProviders.Add(internalProvider);
                        }
                        else
                            continue;
                    }

                    // Get operating hours of station
                    List<RoutePlanner.Data.ChargingStation.OperatingHour> internalOperatingHours = new();
                    foreach (var operatingHourData in poolData.OperatingHours)
                    {
                        string? weekday = dictionaries.Weekday.FirstOrDefault(wkd => wkd.Id == operatingHourData.Weekday)?.Name;
                        if (!string.IsNullOrEmpty(weekday))
                        {
                            var operatingHours = new RoutePlanner.Data.ChargingStation.OperatingHour()
                            {
                                FromTime = operatingHourData.FromTime,
                                ToTime = operatingHourData.ToTime,
                                Weekday = weekday
                            };
                            internalOperatingHours.Add(operatingHours);
                        }
                    }

                    bool allTimeOpen = true;
                    // Find out, if station is opened all time - if yes, operating hours won't be added (unnecessary)
                    foreach (var operatingHours in internalOperatingHours)
                    {
                        if (!operatingHours.OpenedWholeDay)
                            allTimeOpen = false;
                    }

                    // Get charging points for station
                    List<RoutePlanner.Data.ChargingPoint> internalChargingPoints = new();
                    foreach (ChargingPointData chargingPointData in chargingPointsData.Where(point => point.StationId == chargingStationData.Id))
                    {
                        double? chargingPointPrice = null;
                        string? chargingPointPriceUnit = null;
                        bool chargingPointStatus = true;

                        var dynamicRecord = dynamicData.FirstOrDefault(dynamicRecord => dynamicRecord.PointId == chargingPointData.Id);
                        if (dynamicRecord != null)
                        {
                            if (dynamicRecord.Prices.Count > 0)
                            {
                                try
                                {
                                    var priceArray = dynamicRecord.Prices.First();
                                    if (priceArray.PricePrice != null)
                                    {
                                        chargingPointPrice = Convert.ToDouble(priceArray.PricePrice.Replace(".", ","));
                                    }
                                    if (priceArray.Unit != null)
                                    {
                                        chargingPointPriceUnit = priceArray.Unit;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    logger.LogError($"Problem z przypisaniem ceny dla punktu ładowania o ID: {chargingPointData.Id}. Treść błędu: \n{ex.Message}");
                                }
                            }
                            if (dynamicRecord.Status != null && dynamicRecord.Status.Availability != 1)
                            {
                                chargingPointStatus = false;
                            }
                        }

                        // Get connectors for charging point
                        HashSet<RoutePlanner.Data.ChargingPoint.Connector> internalConnectors = new();
                        foreach (EipaUdtImportingTools.Data.Connector connectorData in chargingPointData.Connectors)
                        {
                            // Get interfaces for connector
                            HashSet<RoutePlanner.Data.ConnectorInterface> interfacesForConnector = new();
                            foreach (var connectorInterface in internalConnectorInterfaces.Where(cni => connectorData.Interfaces.Contains(cni.Id)))
                            {
                                interfacesForConnector.Add(connectorInterface);
                                if (!connectorInterfacesInUsage.Contains(connectorInterface))
                                    connectorInterfacesInUsage.Add(connectorInterface);
                            }

                            internalConnectors.Add(
                                new RoutePlanner.Data.ChargingPoint.Connector
                                {
                                    CableAttached = connectorData.CableAttached,
                                    ChargingPower = connectorData.Power != 0 ? connectorData.Power : null,
                                    Interfaces = interfacesForConnector
                                });
                        }

                        RoutePlanner.Data.ChargingPoint internalChargingPoint = new()
                        {
                            Id = chargingPointData.Id,
                            Price = chargingPointPrice,
                            PriceUnit = chargingPointPriceUnit,
                            Status = chargingPointStatus,
                            Connectors = internalConnectors
                        };
                        foreach (var chargingSolution in chargingPointData.ChargingSolutions)
                        {
                            var chargingMode = dictionaries.ChargingMode.FirstOrDefault(cm => cm.Id == chargingSolution.Mode); // solution.Mode is Id
                            if (chargingMode != null && chargingMode.Name != null)
                            {
                                internalChargingPoint.AddChargingMode(chargingMode.Name);
                            }
                        }
                        internalChargingPoints.Add(internalChargingPoint);
                    }

                    RoutePlanner.Data.ChargingStation internalChargingStation = new()
                    {
                        Id = chargingStationData.Id,
                        Provider = internalProvider,
                        Name = poolData.Name ?? "Stacja ładowania",
                        Latitude = chargingStationData.Latitude,
                        Longitude = chargingStationData.Longitude,
                        City = chargingStationData.Location.City,
                        PostalCode = poolData.PostalCode,
                        Street = poolData.Street,
                        HouseNumber = poolData.HouseNumber,
                        Community = chargingStationData.Location.Community,
                        District = chargingStationData.Location.District,
                        Province = chargingStationData.Location.Province,
                        OperatingHours = allTimeOpen ? new List<RoutePlanner.Data.ChargingStation.OperatingHour>() : internalOperatingHours,
                        AllTimeOpen = allTimeOpen,
                        Accessibility = poolData.Accessibility,
                        ChargingPoints = internalChargingPoints
                    };
                    foreach (var paymentMethodId in chargingStationData.PaymentMethods)
                    {
                        var paymentMethod = dictionaries.StationPaymentMethod.FirstOrDefault(pm => pm.Id == paymentMethodId);
                        if (paymentMethod != null)
                            internalChargingStation.AddPaymentMethod(paymentMethod.Description);
                    }
                    foreach (var authMethodId in chargingStationData.AuthenticationMethods)
                    {
                        var authenticationMethod = dictionaries.StationAuthenticationMethod.FirstOrDefault(am => am.Id == authMethodId);
                        if (authenticationMethod != null)
                            internalChargingStation.AddAuthenticationMethod(authenticationMethod.Description);
                    }
                    internalChargingStations.Add(internalChargingStation);
                }

                logger.LogTrace("Przetwarzanie stacji ładowania zakończone.");

                // Save data into static updater properties
                DatabaseUpdater.ChargingStations = internalChargingStations ?? throw new ArgumentNullException(nameof(internalChargingStations));
                DatabaseUpdater.ConnectorInterfacesInUsage = connectorInterfacesInUsage ?? throw new ArgumentNullException(nameof(connectorInterfacesInUsage));
                return true;
            }
            catch (ArgumentNullException ex)
            {
                logger.LogCritical($"Wystąpił wyjątek podczas przetwarzania danych z API na obiekty klas wewnętrznych. Obiekt był nullem: {ex.ParamName}");
                return false;
            }
            catch (Exception ex)
            {
                logger.LogCritical($"Wystąpił wyjątek podczas przetwarzania danych z API na obiekty klas wewnętrznych. Treść wyjątku:\n{ex.Message}");
                return false;
            }
        }

        public static async Task<List<ProviderData>?> GetProvidersData()
        {
            logger.LogTrace("Pobieranie danych dostawców usług.");
            try
            {
                if (ApiUrls.TryGetValue("Providers", out string? url))
                {
                    // Get response from EIPA UDT API
                    HttpResponseMessage providersResponse = await client.GetAsync(url);
                    providersResponse.EnsureSuccessStatusCode();
                    string providersResponseBody = await providersResponse.Content.ReadAsStringAsync();

                    // Deserialize JSON to object
                    EipaUdtImportingTools.Data.Provider? providersInstance = JsonSerializer.Deserialize<EipaUdtImportingTools.Data.Provider?>(providersResponseBody);

                    if (providersInstance != null && providersInstance.Data != null && providersInstance.Data.Count > 0)
                        return providersInstance.Data;
                    else
                        throw new Exception("ProviderInstance lub jego lista danych jest pusta lub jest nullem.");
                }
                else
                    throw new Exception("Nie znaleziono URL dostawców w słowniku.");
            }
            catch (Exception ex)
            {
                logger.LogCritical($"Wystąpił błąd podczas importowania danych.\n{ex.Message}");
                return null;
            }
        }

        public static async Task<List<PoolData>?> GetPoolsData()
        {
            logger.LogTrace("Pobieranie danych baz.");
            try
            {
                if (ApiUrls.TryGetValue("Pools", out string? url))
                {
                    // Get response from EIPA UDT API
                    HttpResponseMessage poolsResponse = await client.GetAsync(url);
                    poolsResponse.EnsureSuccessStatusCode();
                    string poolsResponseBody = await poolsResponse.Content.ReadAsStringAsync();

                    // Deserialize JSON to object
                    Pool? poolsInstance = JsonSerializer.Deserialize<Pool?>(poolsResponseBody);

                    if (poolsInstance != null && poolsInstance.Data != null && poolsInstance.Data.Count > 0)
                        return poolsInstance.Data;
                    else
                        throw new Exception("PoolInstance lub jego lista danych jest pusta lub jest nullem.");
                }
                else
                    throw new Exception("Nie znaleziono URL baz w słowniku.");
            }
            catch (Exception ex)
            {
                logger.LogCritical($"Wystąpił błąd podczas importowania danych.\n{ex.Message}");
                return null;
            }
        }

        public static async Task<List<ChargingStationData>?> GetChargingStationsData()
        {
            logger.LogTrace("Pobieranie danych stacji ładowania.");
            try
            {
                if (ApiUrls.TryGetValue("ChargingStations", out string? url))
                {
                    // Get response from EIPA UDT API
                    HttpResponseMessage chargingStationsResponse = await client.GetAsync(url);
                    chargingStationsResponse.EnsureSuccessStatusCode();
                    string chargingStationsResponseBody = await chargingStationsResponse.Content.ReadAsStringAsync();

                    // Deserialize JSON to object
                    EipaUdtImportingTools.Data.ChargingStation? chargingStationsInstance = JsonSerializer.Deserialize<EipaUdtImportingTools.Data.ChargingStation?>(chargingStationsResponseBody);

                    if (chargingStationsInstance != null && chargingStationsInstance.Data != null)
                        return chargingStationsInstance.Data;
                    else
                        throw new Exception("ChargingStationInstance lub jego lista danych jest pusta lub jest nullem.");
                }
                else
                    throw new Exception("Nie znaleziono URL stacji ładowania w słowniku.");
            }
            catch (Exception ex)
            {
                logger.LogCritical($"Wystąpił błąd podczas importowania danych.\n{ex.Message}");
                return null;
            }
        }

        public static async Task<List<ChargingPointData>?> GetChargingPointsData()
        {
            logger.LogTrace("Pobieranie danych punktów ładowania.");
            try
            {
                if (ApiUrls.TryGetValue("ChargingPoints", out string? url))
                {
                    // Get response from EIPA UDT API
                    HttpResponseMessage chargingPointsResponse = await client.GetAsync(url);
                    chargingPointsResponse.EnsureSuccessStatusCode();
                    string chargingPointsResponseBody = await chargingPointsResponse.Content.ReadAsStringAsync();

                    // Deserialize JSON to object
                    EipaUdtImportingTools.Data.ChargingPoint? chargingPointsInstance = JsonSerializer.Deserialize<EipaUdtImportingTools.Data.ChargingPoint?>(chargingPointsResponseBody);

                    if (chargingPointsInstance != null && chargingPointsInstance.Data != null && chargingPointsInstance.Data.Count > 0)
                        return chargingPointsInstance.Data;
                    else
                        throw new Exception("ChargingPointsInstance lub jego lista danych jest pusta lub jest nullem.");
                }
                else
                    throw new Exception("Nie znaleziono URL punktów ładowania w słowniku.");
            }
            catch (Exception ex)
            {
                logger.LogCritical($"Wystąpił błąd podczas importowania danych.\n{ex.Message}");
                return null;
            }
        }

        public static async Task<Dictionaries?> GetDictionaries()
        {
            logger.LogTrace("Pobieranie danych słowników.");
            try
            {
                if (ApiUrls.TryGetValue("Dictionaries", out string? url))
                {
                    // Get response from EIPA UDT API
                    HttpResponseMessage dictionariesResponse = await client.GetAsync(url);
                    dictionariesResponse.EnsureSuccessStatusCode();
                    string dictionariesResponseBody = await dictionariesResponse.Content.ReadAsStringAsync();

                    // Deserialize JSON to object
                    Dictionaries? dictionariesInstance = JsonSerializer.Deserialize<Dictionaries?>(dictionariesResponseBody);

                    if (dictionariesInstance != null)
                        return dictionariesInstance;
                    else
                        throw new Exception("Dictionaries jest nullem.");
                }
                else
                    throw new Exception("Nie znaleziono URL słowników w słowniku.");
            }
            catch (Exception ex)
            {
                logger.LogCritical($"Wystąpił błąd podczas importowania danych.\n{ex.Message}");
                return null;
            }
        }

        public static async Task<List<DynamicData>?> GetDynamicData()
        {
            logger.LogTrace("Pobieranie danych dynamicznych.");
            try
            {
                if (ApiUrls.TryGetValue("Dynamic", out string? url))
                {
                    // Get response from EIPA UDT API
                    HttpResponseMessage dynamicResponse = await client.GetAsync(url);
                    dynamicResponse.EnsureSuccessStatusCode();
                    string dynamicResponseBody = await dynamicResponse.Content.ReadAsStringAsync();

                    // Deserialize JSON to object
                    Dynamic? dynamicInstance = JsonSerializer.Deserialize<Dynamic?>(dynamicResponseBody);

                    if (dynamicInstance != null && dynamicInstance.Data != null && dynamicInstance.Data.Count > 0)
                        return dynamicInstance.Data;
                    else
                        throw new Exception("DynamicInstance lub jego lista danych jest pusta lub jest nullem.");
                }
                else
                    throw new Exception("Nie znaleziono URL danych dynamicznych w słowniku.");
            }
            catch (Exception ex)
            {
                logger.LogCritical($"Wystąpił błąd podczas importowania danych.\n{ex.Message}");
                return null;
            }
        }
    }
}
