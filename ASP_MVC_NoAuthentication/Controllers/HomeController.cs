using ASP_MVC_NoAuthentication.Data;
using ASP_MVC_NoAuthentication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ASP_MVC_NoAuthentication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MyDbContext _context;
        public HomeController(ILogger<HomeController> logger, MyDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var cars = _context.Cars.ToList();
            return View(cars);
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Informations()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}