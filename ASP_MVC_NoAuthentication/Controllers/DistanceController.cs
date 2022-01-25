using ASP_MVC_NoAuthentication.Data;
using ASP_MVC_NoAuthentication.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        public int getRealDistance([FromQuery] int maximumDistance, int batteryLevel)
        {
            if (_signInManager.IsSignedIn(User))
                return _service.getRealMaximumDistance(batteryLevel, maximumDistance, User.Identity.Name);
            else
                return _service.getRealMaximumDistance(batteryLevel, maximumDistance, null);
        }
    }
}
