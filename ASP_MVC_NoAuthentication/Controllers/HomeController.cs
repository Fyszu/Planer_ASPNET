using ASP_MVC_NoAuthentication.Data;
using ASP_MVC_NoAuthentication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using ASP_MVC_NoAuthentication.Services;
using ASP_MVC_NoAuthentication.Repositories;

namespace ASP_MVC_NoAuthentication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICarService _carService;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        public HomeController(ILogger<HomeController> logger, UserManager<User> userManager, SignInManager<User> signInManager, ICarService carService)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _carService = carService;
        }


        public IActionResult Index()
        {
            //Returns view with List of cars as a model (No user signed - default cars, user signed in - default + user cars)
            List<Car> cars = new List<Car>();
            cars = _carService.GetDefaultCars();
            if (_signInManager.IsSignedIn(User))
                cars = (_carService.GetCarsByUser(User.Identity.Name)).Concat(cars).OrderBy(car => car.Brand).ToList();
            return View(cars);
         }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}