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
        private readonly ICarService _carService;
        private readonly IUserService _userService;
        private readonly ILogger<CarController> _logger;

        public CarController(ILogger<CarController> logger, UserManager<User> userManager, ICarService carService, IUserService userService)
        {
            _carService = carService;
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("RemoveUserCar")]
        public async Task<IActionResult> RemoveUserCar([FromQuery] int carId)
        {
            if (User.Identity == null || User.Identity.Name == null || !User.Identity.IsAuthenticated)
            {
                _logger.LogError($"Próba usunięcia samochodu o ID: {carId} przez niezalogowanego użytkownika.");
                return Unauthorized("Użytkownik nie jest zalogowany.");
            }
            else
            {
                if (await _carService.CheckIfCarBelongsToUserAsync(await _userService.GetUserByNameAsync(User.Identity.Name), await _carService.GetCarByIdAsync(carId)))
                {
                    await _carService.RemoveCarByUserAsync(User.Identity.Name, carId);
                    return Redirect(Url.Content("~/UserPanel"));
                }
                else
                {
                    _logger.LogWarning($"Próba usunięcia samochodu o ID: {carId} przez użytkownika {User.Identity.Name}.");
                    return Unauthorized("Samochód nie jest przypisany do Twojego konta.");
                }
            }
        }

        [HttpGet("AddUserCar")]
        public async Task<IActionResult> AddUserCar(Car car)
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                _logger.LogError("Próba dodania samochodu przez niezalogowanego użytkownika.");
                return Unauthorized();
            }
            else
            {
                await _carService.AddNewCarAsync(car);
                return Redirect(Url.Content("~/UserPanel"));
            }
        }
    }
}
