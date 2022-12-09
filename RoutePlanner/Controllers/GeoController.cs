using RoutePlanner.Services;
using Microsoft.AspNetCore.Mvc;

namespace RoutePlanner.Controllers
{
    //GeoController - provider of responses from google api (parsing coordinates and addresses)
    [ApiController]
    [Route("[controller]")]
    [Filters.RestrictDomain("localhost", "planertras.com")]
    public class GeoController : Controller
    {
        private readonly IGeoService service;
        public GeoController(IGeoService service)
        {
            this.service = service;
        }

        [HttpGet("GetAddress")]
        public async Task<string> GetAddress([FromQuery] string longitude, [FromQuery] string latitude)
        {
            return await service.GetAddressAsync(longitude, latitude);
        }

        [HttpGet("GetCoordinates")]
        public async Task<string> GetCoordinates([FromQuery] string address)
        {
            return await service.GetCoordinatesAsync(address);
        }

    }
}
