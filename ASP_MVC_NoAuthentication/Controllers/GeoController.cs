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

    }
}
