using ASP_MVC_NoAuthentication.Data;
using ASP_MVC_NoAuthentication.Services;
using Microsoft.AspNetCore.Mvc;

namespace ASP_MVC_NoAuthentication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChargingStationController : Controller
    {
        private readonly IChargingStationService _service;
        public ChargingStationController(IChargingStationService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return null;
        }


        [HttpGet("getAllChargingStations")]
        public async Task<JsonResult> getAllChargingStations()
        {
            return Json(await _service.GetAllChargingStations());
        }
    }
}
