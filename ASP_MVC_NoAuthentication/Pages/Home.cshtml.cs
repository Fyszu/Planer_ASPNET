using ASP_MVC_NoAuthentication.Controllers;
using ASP_MVC_NoAuthentication.Data;
using ASP_MVC_NoAuthentication.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Configuration;

namespace ASP_MVC_NoAuthentication.Pages
{
    public class HomeModel : PageModel
    {
        private readonly ILogger<HomeModel> _logger;
        private readonly ICarService _carService;
        private readonly IUserService _userService;
        private readonly IChargingStationService _chargingStationsService;
        private readonly IConfiguration _config;
        private readonly SignInManager<User> _signInManager;

        public HomeModel(ILogger<HomeModel> logger, SignInManager<User> signInManager, ICarService carService, IUserService userService, IChargingStationService chargingStationsService, IConfiguration configuration)
        {
            _logger = logger;
            _signInManager = signInManager;
            _carService = carService;
            _userService = userService;
            _chargingStationsService = chargingStationsService;
            _config = configuration;
            Cars = new();
            ChargingStations = new();
        }
        public List<Car> Cars { get; set; }
        public List<ChargingStation> ChargingStations { get; set; }
        public bool IsLoggedIn { get; set; }

        internal string? BaseUrl { get; set; }
        internal string? GeoControllerLocation { get; set; }
        internal string? GeoKey { get; set; }
        internal string? Key { get; set; }
        public async void OnGetAsync()
        {
            Cars = await GetCarsList();
            ChargingStations = await GetChargingStationsList();
            IsLoggedIn = _signInManager.IsSignedIn(User);
            SetKeys();
        }
        private async Task<List<Car>> GetCarsList()
        {
            List<Car> cars = new();
            if (_signInManager.IsSignedIn(User) && User.Identity != null && User.Identity.Name != null)
            {
                User loggedUser = await _userService.GetUserByName(User.Identity.Name);
                if (loggedUser != null)
                {
                    cars = await _carService.GetCarsByUser(loggedUser.UserName);
                    if (loggedUser.ShowOnlyMyCars)
                    {
                        if (cars.Count > 0)
                        {
                            return cars;
                        }
                    }
                    else if (cars.Count > 0)
                    {
                        return cars.Concat(await _carService.GetDefaultCars()).OrderBy(car => car.Brand).ToList();
                    }
                }
            }
            cars = await _carService.GetDefaultCars();
            return cars;
        }

        private void SetKeys()
        {
            BaseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            GeoControllerLocation = _config.GetValue<string>("GeoControllerLocation");
            GeoKey = _config.GetValue<string>("MyApiKey");
            Key = _config.GetValue<string>("AuthKey");
        }

        private async Task<List<ChargingStation>> GetChargingStationsList()
        {
            List<ChargingStation> chargingStations = await _chargingStationsService.GetAllChargingStations();
            return chargingStations;
        }
    }
}
