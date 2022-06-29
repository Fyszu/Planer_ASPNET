using ASP_MVC_NoAuthentication.Data;
using ASP_MVC_NoAuthentication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using ASP_MVC_NoAuthentication.Services;
using ASP_MVC_NoAuthentication.Repositories;
using Microsoft.AspNetCore;

namespace ASP_MVC_NoAuthentication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICarService _carService;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        public HomeController(ILogger<HomeController> logger, UserManager<User> userManager, SignInManager<User> signInManager, ICarService carService, IUserService userService)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _carService = carService;
            _userService = userService;
        }


        public async Task<ActionResult> Index()
        {
            //Returns view with List of cars as a model (No user signed - default cars, user signed in - user cars)
            List<Car> cars = new List<Car>();
            if (_signInManager.IsSignedIn(User))
            {
                User loggedUser = await _userService.GetUserByName(User.Identity.Name);
                cars = await _carService.GetCarsByUser(loggedUser.UserName);
                if (loggedUser.ShowOnlyMyCars)
                {
                    if (cars.Count > 0) { return View(cars); }
                }
                else if (cars.Count > 0) { return View(cars.Concat(await _carService.GetDefaultCars()).OrderBy(car => car.Brand).ToList()); }
            }
            cars = await _carService.GetDefaultCars();
            return View(cars);
         }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}