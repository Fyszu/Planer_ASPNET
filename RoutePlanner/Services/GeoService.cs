﻿using RoutePlanner.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RoutePlanner.Services
{
    public class GeoService : IGeoService
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<GeoService> logger;
        private readonly string googleApiKey;
        private readonly HttpClient client = new();
        private static readonly int retryCount = 3;
        private static readonly int retryWaitTime = 1500; // ms
        private int currentRetry;

        public GeoService(ILogger<GeoService> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
            googleApiKey = this.configuration.GetValue<string>("GoogleDirectionsGeocodingApiKey")
                ?? throw new ValueNotFoundInConfigurationException("Klucz google api dla directions/geocoding.");
        }

        //Parse result from google api to address name (passing coordinates)
        public async Task<string> GetAddressAsync(string longitude, string latitude)
        {
            currentRetry = 0;
            return await GetResultFromGoogleApi("latlng=" + latitude + "," + longitude);
        }

        //Parse result from google api to coordinates (passing address name)
        public async Task<string> GetCoordinatesAsync(string address)
        {
            currentRetry = 0;
            return await GetResultFromGoogleApi("address=" + address);
        }

        
        //Gets result from google api in JSON and convert to response passing location parameter (coordinates or address)
        private async Task<string> GetResultFromGoogleApi(string location)
        {
            currentRetry++;

            string URL = "https://maps.googleapis.com/maps/api/geocode/json?";
            string Key = "key=" + googleApiKey;
            string Sensor = "&sensor=false&";

            URL = URL + Key + Sensor + location;

            JToken? token = null;
            InternalApiResponse.StatusCode status = InternalApiResponse.StatusCode.None;
            GeoResponse? locationResponse = null;

            try
            {
                HttpResponseMessage httpResponse = await client.GetAsync(URL); //Download result from Google API
                httpResponse.EnsureSuccessStatusCode();
                string responseBody = await httpResponse.Content.ReadAsStringAsync();

                token = JToken.Parse(responseBody); //Parse JSON array

                if (token == null || token["status"] == null || token["status"].ToString() == null)
                {
                    logger.LogError("Nie udało się odczytać statusu odpowiedzi");
                    status = InternalApiResponse.StatusCode.UnknownError;
                }
                else
                {

                    // Statuses are provided by Google Geocoding API
                    // Documentation here: https://developers.google.com/maps/documentation/geocoding/requests-geocoding?hl=pl

                    // If there's 'error_message' node occured in response, it means some error has occured.
                    if (token["error_message"] != null && token["error_message"].ToString() != null)
                    {
                        switch (token["status"].ToString())
                        {
                            case "OVER_DAILY_LIMIT":
                                status = InternalApiResponse.StatusCode.GoogleLimitReached;
                                break;

                            case "OVER_QUERY_LIMIT":
                                status = InternalApiResponse.StatusCode.GoogleLimitReached;
                                break;

                            case "REQUEST_DENIED":
                                if (token["error_message"].ToString().Contains("The provided API key is invalid."))
                                {
                                    status = InternalApiResponse.StatusCode.GoogleInvalidApiKey;
                                    break;
                                }
                                else if (token["error_message"].ToString().Contains("This API key is not authorized to use this service or API."))
                                {
                                    status = InternalApiResponse.StatusCode.GoogleUnauthorizedApiKey;
                                    break;
                                }
                                else
                                {
                                    status = InternalApiResponse.StatusCode.GoogleRequestDenied;
                                    break;
                                }

                            case "INVALID_REQUEST":
                                if (token["error_message"].ToString().Contains("Missing the 'address', 'components', 'latlng' or 'place_id' parameter."))
                                {
                                    status = InternalApiResponse.StatusCode.GoogleNoParametersPassedError;
                                    break;
                                }
                                else
                                {
                                    status = InternalApiResponse.StatusCode.GoogleInvalidRequest;
                                    break;
                                }

                            case "UNKNOWN_ERROR":
                                status = InternalApiResponse.StatusCode.GoogleServerSideUnknownError;
                                break;

                            default:
                                status = InternalApiResponse.StatusCode.UnknownError;
                                break;
                        }
                    }
                    else
                    {
                        switch (token["status"].ToString())
                        {
                            case "OK":
                                status = InternalApiResponse.StatusCode.OK;
                                break;

                            case "ZERO_RESULTS":
                                status = InternalApiResponse.StatusCode.GoogleNoResults;
                                break;

                            default:
                                status = InternalApiResponse.StatusCode.UnknownError;
                                break;
                        }
                    }

                    // Error occured at google server's side - retry
                    if ((status == InternalApiResponse.StatusCode.GoogleServerSideUnknownError || status == InternalApiResponse.StatusCode.GoogleLimitReached) && currentRetry <= retryCount)
                    {
                        Thread.Sleep(retryWaitTime);
                        return await GetResultFromGoogleApi(location);
                    }

                    if (status == InternalApiResponse.StatusCode.OK)
                    {
                        //Results are pulled out from Google API response's JSON Array
                        locationResponse = new GeoResponse(
                            new Coordinates(
                                token["results"][0]["geometry"]["location"]["lat"].ToString().Replace(",", "."), // Latitude, with replacement , -> .
                                token["results"][0]["geometry"]["location"]["lng"].ToString().Replace(",", ".") // Longitude, with replacement , -> .
                            ), token["results"][0]["formatted_address"].ToString() // Address
                        );

                        if (locationResponse.Coordinates == null || string.IsNullOrEmpty(locationResponse.Coordinates.Value.Lat) || string.IsNullOrEmpty(locationResponse.Coordinates.Value.Lng) || string.IsNullOrEmpty(locationResponse.Address))
                        {
                            status = InternalApiResponse.StatusCode.UnknownError;
                        }
                    }
                    else
                    {
                        locationResponse = new(new Coordinates(), null);
                    }
                }

                InternalApiResponse internalApiResponse = new(status, JsonConvert.SerializeObject(locationResponse));
                if (internalApiResponse.Status == InternalApiResponse.StatusCode.GoogleNoResults)
                {
                    internalApiResponse.ErrorMessage = "Nie odnaleziono wyników dla lokalizacji: " + location;
                }
                return internalApiResponse.GetInternalResponseJson();
            }
            catch (Exception ex)
            {
                logger.LogError($"Błąd podczas przetwarzania koordynatów lub adresu. {ex.Message}\n{ex.InnerException}");
                status = InternalApiResponse.StatusCode.UnknownError;
                InternalApiResponse internalApiResponse = new(status, null);
                return internalApiResponse.GetInternalResponseJson();
            }
        }

        private sealed class GeoResponse
        {
            public Coordinates? Coordinates { get; }
            public string? Address { get; }


            public GeoResponse(Coordinates coordinates, string address)
            {
                Coordinates = coordinates;
                Address = address;
            }
        }

        private struct Coordinates
        {
            public string? Lat { get; }
            public string? Lng { get; }

            public Coordinates(string lat, string lng)
            {
                this.Lat = lat;
                this.Lng = lng;
            }
        }
    }
}
