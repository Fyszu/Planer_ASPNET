﻿using ASP_MVC_NoAuthentication.Data;
using ASP_MVC_NoAuthentication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using ASP_MVC_NoAuthentication.Services;

namespace ASP_MVC_NoAuthentication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeService _service;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        public HomeController(IHomeService service, ILogger<HomeController> logger, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _service = service;
        }

        public IActionResult Index()
        {
            List<Car> cars = new List<Car>();
            if (_signInManager.IsSignedIn(User))
                cars = _service.getUserCars(User.Identity.Name);
            else
                cars = _service.getDefaultCars();
            return View(cars);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}