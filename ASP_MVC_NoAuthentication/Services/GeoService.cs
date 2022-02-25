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

            if (googleApiResult.Contains("error_message"))
            {
                Console.WriteLine("Error while parsing coordinates to address - google api returned error message as result.");
                return "error_googleapi";
            }
            else if (key != GeoControllerKey)
            {
                Console.WriteLine("Error while parsing coordinates to address - wrong Geo Key.");
                return "error_bad_geokey";
            }

            else
            {
                try
                {
                    if (googleApiResult.Length > 1)
                    {
                        //Find formatted_address in result JSON matrix
                        JToken token = JToken.Parse(googleApiResult);
                        JArray array = JArray.Parse(token["results"].ToString());
                        result = array.First["formatted_address"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            if(String.IsNullOrEmpty(result))
            {
                Console.WriteLine("Something went wrong while parsing address to coordinates - result is empty.");
                return "error_unknown";
            }

            return result;
        }

        //Parse result from google api to coordinates (passing address name)
        public string[] GetCoordinates(string key, string address)
        {
            string googleApiResult = GetResultFromGoogleApi("address=" + address);
            string resultLat = string.Empty;
            string resultLong = string.Empty;

            if (googleApiResult.Contains("error_message"))
            {
                Console.WriteLine("Error while parsing address to coordinates - google api returned error message as result.");
                return new string[] { "error_googleapi" };
            }
            else if (key != GeoControllerKey)
            {
                Console.WriteLine("Error while parsing address to coordinates - wrong Geo Key.");
                return new string[] { "error_bad_geokey" };
            }

            else
            {
                try
                {
                    //Find lat & lng in JSON result matrix
                    if (googleApiResult.Length > 1)
                    {
                        JToken token = JToken.Parse(googleApiResult);
                        JArray array = JArray.Parse(token["results"].ToString());

                        JToken arraySecond = array.First["geometry"];
                        arraySecond = arraySecond.First;
                        if (JToken.ReferenceEquals(arraySecond.First["northeast"], null))
                        {
                            resultLat = arraySecond.First["lat"].ToString();
                            resultLong = arraySecond.First["lng"].ToString();
                        }
                        else
                        {
                            if (JToken.ReferenceEquals(arraySecond.First["bounds"], null))
                            {
                                arraySecond = arraySecond.First;
                                arraySecond = arraySecond.First;
                                resultLat = arraySecond.First["lat"].ToString();
                                resultLong = arraySecond.First["lng"].ToString();
                            }
                            else
                            {
                                arraySecond = arraySecond.First["bounds"];
                                arraySecond = arraySecond.First["northeast"];
                                resultLat = arraySecond.First["lat"].ToString();
                                resultLong = arraySecond.First["lng"].ToString();
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
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
