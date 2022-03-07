using ASP_MVC_NoAuthentication.Data;
using ASP_MVC_NoAuthentication.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace ASP_MVC_NoAuthentication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DistanceController : Controller
    {

        private readonly IDistanceService _service;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public DistanceController(IDistanceService service, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _service = service;
            _signInManager = signInManager;
            _userManager = userManager;
        }



        [HttpGet("getRealDistance")]
        public async Task<int> getRealDistance([FromQuery] int maximumDistance, int batteryLevel)
        {
            if (_signInManager.IsSignedIn(User))
                return await _service.getRealMaximumDistance(batteryLevel, maximumDistance, User.Identity.Name);
            else
                return await _service.getRealMaximumDistance(batteryLevel, maximumDistance, "default");
        }


        [HttpGet("getNumberOfRecharges")]
        public int getNumberOfRecharges([FromQuery] string routeDistance, int maxDistance, int batteryLevel)
        { 
            if(string.IsNullOrEmpty(routeDistance)) return -1;

            else {
                //Regex cleaning string leaving only number
                Regex regex = new Regex(@"[^\d]+");
                string cleaned = regex.Replace(routeDistance, "");
                return _service.getNumberOfRecharges(Double.Parse(cleaned), maxDistance, batteryLevel);
            }
        }

    }
}
