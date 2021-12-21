using ASP_MVC_NoAuthentication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;

namespace ASP_MVC_NoAuthentication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GeoController : Controller
    {
        private readonly IGeoService _service;
        private readonly IConfiguration _configuration;
        public GeoController(IGeoService service)
        {
            _service = service;
        }
        public ActionResult Index()
        {
            return null;
        }

        [HttpGet("getAddress")]
        public string GetAddress([FromQuery] string key, [FromQuery] string longitude, [FromQuery] string latitude)
        {
            return _service.GetAddress(key, longitude, latitude);
        }

        [HttpGet("getCoordinates")]
        public string[] GetCoordinates([FromQuery] string key, [FromQuery] string address)
        {
            return _service.GetCoordinates(key, address);
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
