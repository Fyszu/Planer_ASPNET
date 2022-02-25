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
        private readonly ICarService _carService;
        private readonly IUserService _userService;

        public CarController(UserManager<User> userManager, ICarService carService, IUserService userService)
        {
            _userManager = userManager;
            _carService = carService;
            _userService = userService;
        }


         public IActionResult Index()
        {
            return null;
        }

        [HttpGet("RemoveUserCar")]
        public async Task<IActionResult> RemoveUserCar([FromQuery] int carId)
        {
            if (_carService.CheckIfCarBelongsToUser(_userService.GetUserByName(User.Identity.Name), _carService.GetCarById(carId)))
                _carService.RemoveCarByUser(User.Identity.Name, carId);
            return Redirect(Url.Content("~/UserPanel"));
        }

        [HttpGet("AddUserCar")]
        public async Task<IActionResult> AddUserCar(Car car)
        {
            _carService.AddNewCar(car);
            return Redirect(Url.Content("~/UserPanel"));
        }
    }
}
