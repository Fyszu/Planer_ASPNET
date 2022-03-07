using ASP_MVC_NoAuthentication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;

namespace ASP_MVC_NoAuthentication.Controllers
{
    //GeoController - provider of responses from google api (parsing coordinates and addresses)
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
        public async Task<string> GetAddress([FromQuery] string key, [FromQuery] string longitude, [FromQuery] string latitude)
        {
            return await _service.GetAddress(key, longitude, latitude);
        }

        [HttpGet("getCoordinates")]
        public async Task<string> GetCoordinates([FromQuery] string key, [FromQuery] string address)
        {
            return await _service.GetCoordinates(key, address);
        }

    }
}
