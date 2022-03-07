using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;

namespace ASP_MVC_NoAuthentication.Services
{
    public class GeoService : IGeoService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<GeoService> _logger;
        private readonly string googleApiKey;
        private readonly string geoControllerKey;
        private string GoogleApiKey { get { return googleApiKey; } }
        private string GeoControllerKey { get { return geoControllerKey; } }

        public GeoService(ILogger<GeoService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            googleApiKey = _configuration.GetValue<string>("AuthKey");
            geoControllerKey = _configuration.GetValue<string>("MyApiKey");
        }


        //Parse result from google api to address name (passing coordinates)
        public async Task<string> GetAddress(string key, string longitude, string latitude)
        {
            return await GetResultFromGoogleApi("latlng=" + latitude + "," + longitude, key);
        }

        //Parse result from google api to coordinates (passing address name)
        public async Task<string> GetCoordinates(string key, string address)
        {
            return await GetResultFromGoogleApi("address=" + address, key);
        }

        
        //Gets result from google api in JSON and convert to response passing location parameter (coordinates or address)
        private async Task<string> GetResultFromGoogleApi(string location, string geoKey)
        {
            string URL = "https://maps.googleapis.com/maps/api/geocode/json?";
            string Key = "key=" + this.GoogleApiKey;
            string Sensor = "&sensor=false&";

            URL = URL + Key + Sensor + location;

            JToken token = null;
            Response response;
            string googleApiResult = string.Empty;
            string status = string.Empty;

            if (geoKey.Equals(geoControllerKey))
            {
                try
                {
                    using (WebClient webClient = new WebClient())
                    {
                        webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                        webClient.Encoding = Encoding.UTF8;
                        webClient.Headers.Add("User-Agent: Other");
                        googleApiResult = await webClient.DownloadStringTaskAsync(new Uri(URL)); //Download result from Google API
                        token = JToken.Parse(googleApiResult); //Parse JSON array
                        status = token["status"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                    status = "GOOGLEAPI_ERROR";
                }
            }
            else status = "WRONG_KEY";

            if (status.Equals("OK"))
            {
                //Results are pulled out from Google API response's JSON Array
                response = new Response(status,new Coordinates(
                        token["results"][0]["geometry"]["location"]["lat"].ToString().Replace(",","."), //Latitude, with replacement , -> .
                        token["results"][0]["geometry"]["location"]["lng"].ToString().Replace(",",".")), //Longitude, with replacement , -> .
                    token["results"][0]["formatted_address"].ToString());
            } else
            {
                //Send response with status only
                response = new Response(status, new Coordinates(), String.Empty);
            }

            return JsonConvert.SerializeObject(response);
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
