using ASP_MVC_NoAuthentication.Data;
using ASP_MVC_NoAuthentication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASP_MVC_NoAuthentication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CarController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IHomeService _homeService;
        private readonly ICarService _carService;

        public CarController(UserManager<User> userManager, IHomeService homeService, ICarService carService)
        {
            _userManager = userManager;
            _homeService = homeService;
            _carService = carService;
        }


         public IActionResult Index()
        {
            return null;
        }

        [HttpGet("RemoveUserCar")]
        public async Task<IActionResult> RemoveUserCar([FromQuery] int carid)
        {
            _carService.RemoveUserCar(_homeService.getUser(User.Identity.Name), carid);
            return Redirect(Url.Content("~/UserPanel"));
        }

        [HttpGet("AddUserCar")]
        public async Task<IActionResult> AddUserCar(Car car)
        {
            _carService.AddUserCar(_homeService.getUser(User.Identity.Name), car);
            return Redirect(Url.Content("~/UserPanel"));
        }
    }
}
