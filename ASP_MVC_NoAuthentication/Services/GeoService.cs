using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;

namespace ASP_MVC_NoAuthentication.Services
{
    public class GeoService : IGeoService
    {
        private readonly IConfiguration _configuration;
        public GeoService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetAddress(string key, string longitude, string latitude)
        {
            string properKey = _configuration.GetValue<string>("MyApiKey");
            string data = GetResultFromCoordinates(latitude, longitude);
            string result = string.Empty;
            if (data.Contains("error_message"))
            {

            }
            else if (key != _configuration.GetValue<string>("MyApiKey"))
            {

            }
            else
            {
                try
                {
                    if (data.Length > 1)
                    {
                        JToken token = JToken.Parse(data);
                        JArray array = JArray.Parse(token["results"].ToString());



                        result = array.First["formatted_address"].ToString();


                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Console.Read();
                }
            }
            return result;
        }

        public string[] GetCoordinates(string key, string address)
        {
            string data = GetResultFromAddress(address);
            string resultLat = string.Empty;
            string resultLong = string.Empty;
            string properKey = _configuration.GetValue<string>("MyApiKey");
            if (key == _configuration.GetValue<string>("MyApiKey"))
            {
                try
                {
                    if (data.Length > 1)
                    {
                        JToken token = JToken.Parse(data);
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
                    Console.Read();
                }
            }
            else
                return new string[] { "Wrong key bruh" };
            string[] result = new string[] { resultLat, resultLong };
            return result;
        }

        private string GetResultFromCoordinates(string lat, string lang)
        {

            string method = "GET";
            string URL = "https://maps.googleapis.com/maps/api/geocode/json?";
            string Key = "key=" + _configuration.GetValue<string>("AuthKey");
            string Sensor = "&sensor=false&";
            string latlng = "latlng=" + lat + "," + lang;
            string result = string.Empty;

            URL = URL + Key + Sensor + latlng;

            try
            {
                using (WebClient wc = new WebClient())
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                    wc.Encoding = Encoding.UTF8;
                    wc.Headers.Add("User-Agent: Other");

                    switch (method.ToUpper())
                    {
                        case "GET":
                            result = wc.DownloadString(URL);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                Console.Read();
            }

            return result;
        }

        private string GetResultFromAddress(string address)
        {

            string method = "GET";
            string URL = "https://maps.googleapis.com/maps/api/geocode/json?";
            string Key = "key=" + _configuration.GetValue<string>("AuthKey");
            string Sensor = "&sensor=false&";
            string addressURL = "address=" + address;
            string result = string.Empty;

            URL = URL + Key + Sensor + addressURL;

            try
            {
                using (WebClient wc = new WebClient())
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                    wc.Encoding = Encoding.UTF8;
                    wc.Headers.Add("User-Agent: Other");

                    switch (method.ToUpper())
                    {
                        case "GET":
                            result = wc.DownloadString(URL);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                Console.Read();
            }

            return result;
        }
    }
}
