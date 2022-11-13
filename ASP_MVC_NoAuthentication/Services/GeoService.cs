using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;

namespace ASP_MVC_NoAuthentication.Services
{
    public class GeoService : IGeoService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<GeoService> _logger;
        private readonly string _googleApiKey;
        private readonly HttpClient _client = new();

        public GeoService(ILogger<GeoService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _googleApiKey = _configuration.GetValue<string>("GoogleDirectionsGeocodingApiKey");
        }


        //Parse result from google api to address name (passing coordinates)
        public async Task<string> GetAddress(string longitude, string latitude)
        {
            return await GetResultFromGoogleApi("latlng=" + latitude + "," + longitude);
        }

        //Parse result from google api to coordinates (passing address name)
        public async Task<string> GetCoordinates(string address)
        {
            return await GetResultFromGoogleApi("address=" + address);
        }

        
        //Gets result from google api in JSON and convert to response passing location parameter (coordinates or address)
        private async Task<string> GetResultFromGoogleApi(string location)
        {
            string URL = "https://maps.googleapis.com/maps/api/geocode/json?";
            string Key = "key=" + _googleApiKey;
            string Sensor = "&sensor=false&";

            URL = URL + Key + Sensor + location;

            JToken token = null;
            string status;
            Response internalResponse;

            try
            {
                HttpResponseMessage httpResponse = await _client.GetAsync(URL); //Download result from Google API
                httpResponse.EnsureSuccessStatusCode();
                string responseBody = await httpResponse.Content.ReadAsStringAsync();

                token = JToken.Parse(responseBody); //Parse JSON array
                if (token != null && token["status"] != null) status = token["status"].ToString();
                else throw new Exception("Nie znaleziono statusu odpowiedzi.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Błąd podczas przetwarzania koordynatów lub adresu. {ex.Message}\n{ex.InnerException}");
                status = "GOOGLEAPI_ERROR";
            }

            if (status.Equals("OK"))
            {
                //Results are pulled out from Google API response's JSON Array
                internalResponse = new Response(status,new Coordinates(
                    token["results"][0]["geometry"]["location"]["lat"].ToString().Replace(",","."), //Latitude, with replacement , -> .
                    token["results"][0]["geometry"]["location"]["lng"].ToString().Replace(",",".")), //Longitude, with replacement , -> .
                    token["results"][0]["formatted_address"].ToString());
            }
            else
            {
                //Send response with status only
                internalResponse = new Response(status, new Coordinates(), String.Empty);
            }

            return JsonConvert.SerializeObject(internalResponse);
        }

        private class Response
        {
            public string? Status { get; }
            public Coordinates? Coordinates { get; }
            public string? Address { get; }


            public Response(string status, Coordinates coordinates, string address)
            {
                Status = status;
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
