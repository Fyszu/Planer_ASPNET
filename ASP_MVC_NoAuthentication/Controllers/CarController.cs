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
        private readonly ICarService carService;
        private readonly IUserService userService;
        private readonly ILogger<CarController> logger;

        public CarController(ILogger<CarController> logger, UserManager<User> userManager, ICarService carService, IUserService userService)
        {
            this.carService = carService;
            this.userService = userService;
            this.logger = logger;
        }

        [HttpGet("RemoveUserCar")]
        public async Task<IActionResult> RemoveUserCar([FromQuery] int carId)
        {
            if (User.Identity == null || User.Identity.Name == null || !User.Identity.IsAuthenticated)
            {
                logger.LogError($"Próba usunięcia samochodu o ID: {carId} przez niezalogowanego użytkownika.");
                return Unauthorized("Użytkownik nie jest zalogowany.");
            }
            else
            {
                if (await carService.CheckIfCarBelongsToUserAsync(await userService.GetUserByNameAsync(User.Identity.Name), await carService.GetCarByIdAsync(carId)))
                {
                    await carService.RemoveCarByUserAsync(User.Identity.Name, carId);
                    return Redirect(Url.Content("~/UserPanel"));
                }
                else
                {
                    logger.LogWarning($"Próba usunięcia samochodu o ID: {carId} przez użytkownika {User.Identity.Name}.");
                    return Unauthorized("Samochód nie jest przypisany do Twojego konta.");
                }
            }
        }

        [HttpGet("AddUserCar")]
        public async Task<IActionResult> AddUserCar(Car car)
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                logger.LogError("Próba dodania samochodu przez niezalogowanego użytkownika.");
                return Unauthorized();
            }
            else
            {
                await carService.AddNewCarAsync(car);
                return Redirect(Url.Content("~/UserPanel"));
            }
        }
    }
}
