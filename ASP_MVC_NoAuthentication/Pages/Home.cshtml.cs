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
        private readonly ILogger<HomeModel> logger;
        private readonly ICarService carService;
        private readonly IUserService userService;
        private readonly IChargingStationService chargingStationsService;
        private readonly IConfiguration configuration;
        private readonly SignInManager<User> signInManager;

        public HomeModel(ILogger<HomeModel> logger, SignInManager<User> signInManager, ICarService carService, IUserService userService, IChargingStationService chargingStationsService, IConfiguration configuration)
        {
            this.logger = logger;
            this.signInManager = signInManager;
            this.carService = carService;
            this.userService = userService;
            this.chargingStationsService = chargingStationsService;
            this.configuration = configuration;
        }

        public List<Car> Cars { get; set; }
        public string JsonCars { get; set; }
        public string JsonChargingStations { get; set; }
        public bool IsLoggedIn { get; private set; }
        public string? BaseUrl { get; private set; }
        public string? GeoControllerCoordinatesLocation { get; private set; }
        public string? GeoControllerAddressLocation { get; private set; }
        public string? GoogleMapApiKey { get; private set; }

        public async void OnGetAsync()
        {
            IsLoggedIn = signInManager.IsSignedIn(User) && User.Identity != null && User.Identity.Name != null;
            
            logger.LogInformation($"Uruchomiono widok g³ówny dla u¿ytkownika: {(IsLoggedIn ? User.Identity.Name : "Niezalogowany")}.");

            Stopwatch stopwatch = new();

            stopwatch.Start();
            Cars = await GetCarsList();
            stopwatch.Stop();

            logger.LogInformation($"Czas poœwiêcony na pobranie listy samochodów: {stopwatch.ElapsedMilliseconds}ms.");

            List<ChargingStation> chargingStations;
            stopwatch.Restart();
            try
            {
                chargingStations = await chargingStationsService.GetAllChargingStationsAsync();
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Problem z pobraniem stacji ³adowania.", new Exception($"{ex.Message}\n{ex.InnerException}"));
            }
            stopwatch.Stop();

            logger.LogInformation($"Czas poœwiêcony na pobranie listy stacji ³adowania: {stopwatch.ElapsedMilliseconds}ms.");

            if (chargingStations == null || chargingStations.Count == 0)
            {
                throw new NoDataInDatabaseException("Nie uda³o siê pobraæ listy stacji ³adowania.", new Exception($"Lista by³a {(chargingStations == null ? "nullem" : "pusta")}."));
            }

            stopwatch.Restart();

            // Serialization to JSON arrays for later in-javascript usage
            JsonSerializerOptions jsonSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            try
            {
                JsonCars = JsonSerializer.Serialize(Cars, jsonSerializerOptions);
                JsonChargingStations = JsonSerializer.Serialize(chargingStations, jsonSerializerOptions);
            }
            catch (Exception ex)
            {
                throw new Exception("Nie uda³o siê przetworzyæ obiektów na JSON.", new Exception($"{ex.Message}\n{ex.InnerException}"));
            }

            stopwatch.Stop();
            logger.LogInformation($"Czas poœwiêcony na serializacjê: {stopwatch.ElapsedMilliseconds}ms.");

            SetKeys();
        }

        private async Task<List<Car>> GetCarsList()
        {
            List<Car> cars = new();

            if (IsLoggedIn)
            {
                User? loggedUser;

                try
                {
                    loggedUser = await userService.GetUserByNameAsync(User.Identity.Name);
                }
                catch(Exception ex)
                {
                    logger.LogError($"Wyst¹pi³ problem z pobraniem u¿ytkownika {User.Identity.Name}.\n{ex.Message}\n{ex.InnerException}");
                    loggedUser = null;
                }

                if (loggedUser == null)
                {
                    logger.LogError("SignInService zwróci³ informacjê ¿e u¿ytkownik jest zalogowany, lecz nie znaleziono go w bazie danych korzystaj¹c z UserService.");
                    throw new DatabaseException("Nie znaleziono zalogowanego u¿ytkownika w bazie danych.");
                }

                try
                {
                    cars = await carService.GetCarsByUserAsync(loggedUser.UserName);
                }
                catch(Exception ex)
                {
                    logger.LogError($"Wyst¹pi³ problem z pobraniem listy samochodów dla u¿ytkownika {User.Identity.Name}.\n{ex.Message}\n{ex.InnerException}");
                }

                if (cars.Count == 0)
                {
                    cars = await carService.GetDefaultCarsAsync();
                }
                else if (!loggedUser.ShowOnlyMyCars)
                {
                    cars = cars.Concat(await carService.GetDefaultCarsAsync()).OrderBy(car => car.Brand).ToList();
                }
            }
            else
            {
                cars = await carService.GetDefaultCarsAsync();
            }

            if (cars == null || cars.Count == 0)
            {
                throw new NoDataInDatabaseException("Nie uda³o siê pobraæ listy samochodów, lub baza danych samochodów jest pusta.");
            }

            return cars;
        }

        private void SetKeys()
        {
            BaseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}" 
                ?? throw new Exception("Nie uda³o siê ustaliæ bazowego url.");

            GeoControllerCoordinatesLocation = configuration.GetValue<string>("GeoControllerCoordinatesLocation") 
                ?? throw new ValueNotFoundInConfigurationException("Url metody kontrolera (koordynaty).");

            GeoControllerAddressLocation = configuration.GetValue<string>("GeoControllerAddressLocation")
                ?? throw new ValueNotFoundInConfigurationException("Url metody kontrolera (adres).");

            GoogleMapApiKey = configuration.GetValue<string>("GoogleMapApiKey")
                ?? throw new ValueNotFoundInConfigurationException("Klucz google api dla map.");
        }
    }
}
