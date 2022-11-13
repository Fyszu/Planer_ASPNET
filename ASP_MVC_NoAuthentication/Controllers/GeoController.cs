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
    [RestrictDomain("localhost")]
    public class GeoController : Controller
    {
        private readonly IGeoService _service;
        public GeoController(IGeoService service)
        {
            _service = service;
        }

        [HttpGet("GetAddress")]
        public async Task<string> GetAddress([FromQuery] string longitude, [FromQuery] string latitude)
        {
            return await _service.GetAddress(longitude, latitude);
        }

        [HttpGet("GetCoordinates")]
        public async Task<string> GetCoordinates([FromQuery] string address)
        {
            return await _service.GetCoordinates(address);
        }

    }
}
