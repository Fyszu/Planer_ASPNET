namespace ASP_MVC_NoAuthentication.Controllers
{
    using ASP_MVC_NoAuthentication.Data;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
        [ApiController]
        [Route("/api/[controller]")]
        public class LocationController : ControllerBase
        {

        private readonly ILogger<LocationController> _logger;
        private readonly MyDbContext _context;
        public LocationController(ILogger<LocationController> logger, MyDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("getAddress")]
        public string GetAddress([FromQuery] string longitude, [FromQuery] string latitude)
        {
            string data = GetResultFromCoordinates(latitude, longitude);
                string result = string.Empty;
                if (data.Contains("error_message"))
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


            public static string[] GetCoordinates(string data)
            {
                string resultLat = string.Empty;
                string resultLong = string.Empty;
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
                string[] result = new string[] { resultLat, resultLong };
                return result;
            }

            public static string GetResultFromCoordinates(string lat, string lang)
            {

                string method = "GET";
                string URL = "https://maps.googleapis.com/maps/api/geocode/json?";
                string Key = "key=AIzaSyACwQzyeU_1trUKE_ErjX5BwT5ALqdqYSM&";
                string Sensor = "sensor=false&";
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


            public static string GetResultFromAddress(string address)
            {

                string method = "GET";
                string URL = "https://maps.googleapis.com/maps/api/geocode/json?";
                string Key = "key=AIzaSyACwQzyeU_1trUKE_ErjX5BwT5ALqdqYSM&";
                string Sensor = "sensor=false&";
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
