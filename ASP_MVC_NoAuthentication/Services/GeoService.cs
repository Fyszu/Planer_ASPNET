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
        public string GetAddress(string key, string longitude, string latitude)
        {
            string googleApiResult = GetResultFromGoogleApi("latlng=" + latitude + "," + longitude);
            string result = string.Empty;
            JToken token = JToken.Parse(googleApiResult); //Parse JSON array

            switch (token["status"].ToString())
            {
                case "INVALID_REQUEST":
                    return token["error_message"].ToString();
                case "ZERO_RESULTS":
                    return "ZERO_RESULTS";
                case "OK":
                    break;
                default:
                    return "UNKNOWN";
            }

            try
            {
                //Find formatted_address in result JSON array
                result = token["results"][0]["formatted_address"].ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            if(String.IsNullOrEmpty(result))
                return "UNKNOWN";

            return result;
        }

        //Parse result from google api to coordinates (passing address name)
        public string[] GetCoordinates(string key, string address)
        {
            string googleApiResult = GetResultFromGoogleApi("address=" + address);
            string resultLat = string.Empty;
            string resultLong = string.Empty;
            JToken token = JToken.Parse(googleApiResult); //Parse JSON array

            switch(token["status"].ToString())
            {
                case "INVALID_REQUEST":
                    return new string[] { token["error_message"].ToString() };
                case "ZERO_RESULTS":
                    return new string[] { "ZERO_RESULTS" };
                case "OK":
                    break;
                default:
                    return new string[] { "UNKNOWN" };
            }

            try
            {
                //find coordinates in JSON array
                resultLat = token["results"][0]["geometry"]["location"]["lat"].ToString();
                resultLong = token["results"][0]["geometry"]["location"]["lng"].ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            string[] result = new string[] { resultLat, resultLong };

            if (String.IsNullOrEmpty(resultLat) || String.IsNullOrEmpty(resultLong))
            {
                Console.WriteLine("Something went wrong while parsing address to coordinates - lat or lng are empty.");
                return new string[] { "error_unknown" };
            }

            return result;
        }

        
        //Gets result from google api in JSON passing location parameter (coordinates or address)
        private string GetResultFromGoogleApi(string location)
        {
            string result = string.Empty;
            string URL = "https://maps.googleapis.com/maps/api/geocode/json?";
            string Key = "key=" + GoogleApiKey;
            string Sensor = "&sensor=false&";

            URL = URL + Key + Sensor + location;

            try
            {
                using (WebClient wc = new WebClient())
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                    wc.Encoding = Encoding.UTF8;
                    wc.Headers.Add("User-Agent: Other");
                    result = wc.DownloadString(URL); //Download result from Google API
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            return result;
        }
    }
}
