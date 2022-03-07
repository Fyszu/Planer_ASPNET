using ASP_MVC_NoAuthentication.Data;
using ASP_MVC_NoAuthentication.Repositories;
using ASP_MVC_NoAuthentication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASP_MVC_NoAuthentication.Pages
{
    [Authorize]
    public class EditCarModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ICarService _carService;
        private readonly IUserService _userService;
        private readonly IConnectorService _connectorService;
        public List<Connector> _connectors = null;
        public User _currentUser;
        public Car _editedCar;
        public Boolean _canEditCar;
        private static int CarId { get; set; }
        public EditCarModel(UserManager<User> userManager, SignInManager<User> signInManager, ICarService carService, IUserService userService, IConnectorService connectorService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _carService = carService;
            _userService = userService;
            _connectorService = connectorService;
        }

        public async Task OnGetAsync()
        {
            await GetPage();
        }
        public async Task GetPage()
        {
            _connectors = await _connectorService.GetAllConnectors();
            _currentUser = await _userService.GetUserByName(User.Identity.Name);
            CarId = int.Parse(Request.Query["carid"]);
            _editedCar = await _carService.GetCarById(CarId);
            if (_editedCar != null)
                _canEditCar = await _carService.CheckIfCarBelongsToUser(_currentUser, _editedCar);
        }
        public async Task<IActionResult> OnPostAddCarAsync(string? returnUrl = null)
        {
            _currentUser = await _userService.GetUserByName(User.Identity.Name);
            _editedCar = await _carService.GetCarById(CarId);
            if (await _carService.CheckIfCarBelongsToUser(_currentUser, _editedCar))
            {
                _connectors = await _connectorService.GetAllConnectors();
                string brand = Request.Form["brand"];
                string model = Request.Form["carmodel"];
                int maximumDistance = (int.Parse(Request.Form["maximumdistance"]));
                string check;
                List<Connector> carConnectors = new List<Connector>();
                foreach (Connector connector in _connectors)
                {
                    string cb = "checkbox" + connector.Id;
                    check = Request.Form[cb];
                    if (check != null)
                        carConnectors.Add(connector);
                }
                await _carService.UpdateCar(new Car(_editedCar.Id, brand, model, maximumDistance, carConnectors, _currentUser));
            }
            return Redirect(Url.Content("~/UserPanel"));
        }
    }
}
