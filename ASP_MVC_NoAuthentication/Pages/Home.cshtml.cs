using ASP_MVC_NoAuthentication.Data;
using ASP_MVC_NoAuthentication.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Serialization;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        public List<Car> Cars { get; set; }
        public string JsonCars { get; set; }
        public string JsonChargingStations { get; set; }
        public bool IsLoggedIn { get; set; }
        internal string? BaseUrl { get; set; }
        internal string? GeoControllerCoordinatesLocation { get; private set; }
        internal string? GeoControllerAddressLocation { get; private set; }
        internal string? GoogleMapApiKey { get; private set; }


        public HomeModel(ILogger<HomeModel> logger, SignInManager<User> signInManager, ICarService carService, IUserService userService, IChargingStationService chargingStationsService, IConfiguration configuration)
        {
            _logger = logger;
            _signInManager = signInManager;
            _carService = carService;
            _userService = userService;
            _chargingStationsService = chargingStationsService;
            _config = configuration;
            Cars = new();
        }

        public async void OnGetAsync()
        {
            IsLoggedIn = _signInManager.IsSignedIn(User) && User.Identity != null && User.Identity.Name != null;
            
            _logger.LogInformation($"Uruchomiono widok g³ówny dla u¿ytkownika: {(IsLoggedIn ? User.Identity.Name : "Niezalogowany")}.");
            _logger.LogTrace("Pobieranie listy samochodów.");

            Stopwatch stopwatch = new();

            stopwatch.Start();
            Cars = await GetCarsList();
            stopwatch.Stop();

            _logger.LogInformation($"Czas poœwiêcony na pobranie listy samochodów: {stopwatch.ElapsedMilliseconds}ms.");
            _logger.LogTrace("Pobieranie listy stacji ³adowania.");

            stopwatch.Restart();
            var chargingStations = await _chargingStationsService.GetAllChargingStations();
            stopwatch.Stop();

            _logger.LogInformation($"Czas poœwiêcony na pobranie listy stacji ³adowania: {stopwatch.ElapsedMilliseconds}ms.");

            // Serialization to JSON arrays for later in-javascript usage
            JsonSerializerOptions jsonSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            if (Cars.Count > 0)
            {
                _logger.LogTrace($"Serializacja listy samochodów ³adowania.");
                stopwatch.Restart();
                JsonCars = JsonSerializer.Serialize(Cars, jsonSerializerOptions);
                stopwatch.Stop();
                _logger.LogInformation($"Czas serializacji listy samochodów: {stopwatch.ElapsedMilliseconds}ms.");
            }
            else
            {
                _logger.LogCritical("Pobrana lista samochodów by³a pusta.");
                throw new Exception("Lista samochodów jest pusta.");
            }
            if (chargingStations.Count > 0)
            {
                try
                {
                    _logger.LogTrace($"Serializacja listy stacji ³adowania.");
                    stopwatch.Restart();
                    JsonChargingStations = JsonSerializer.Serialize(chargingStations, jsonSerializerOptions);
                    stopwatch.Stop();
                    _logger.LogInformation($"Czas serializacji stacji ³adowania: {stopwatch.ElapsedMilliseconds}ms.");
                }
                catch (Exception ex)
                {
                    _logger.LogCritical($"B³¹d podczas serializacji stacji ³adowania. {ex.Message}");
                    Redirect(BaseUrl + "/Error");
                }
            }
            else
            {
                _logger.LogCritical("Pobrana lista stacji ³adowania by³a pusta.");
                throw new Exception("Lista stacji ³adowania jest pusta.");
            }

            SetKeys();
        }
        private async Task<List<Car>> GetCarsList()
        {
            List<Car> cars = new();
            if (IsLoggedIn)
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
            GeoControllerCoordinatesLocation = _config.GetValue<string>("GeoControllerCoordinatesLocation");
            GeoControllerAddressLocation = _config.GetValue<string>("GeoControllerAddressLocation");
            GoogleMapApiKey = _config.GetValue<string>("GoogleMapApiKey");
        }
    }
}
