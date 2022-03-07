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
    public class AddNewCarModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly ICarService _carService;
        private readonly IConnectorService _connectorService;
        public List<Connector> _connectors = null;
        public User _currentUser;

        public AddNewCarModel(UserManager<User> userManager, SignInManager<User> signInManager, IUserService userService, ICarService carService, IConnectorService connectorService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _carService = carService;
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
        }
        public async Task<IActionResult> OnPostAddCarAsync(string? returnUrl = null)
        {
            _currentUser = await _userService.GetUserByName(User.Identity.Name);
            _connectors = await _connectorService.GetAllConnectors();
            string brand = Request.Form["brand"];
            string model = Request.Form["carmodel"];
            int maximumDistance = int.Parse(Request.Form["maximumdistance"]);
            string check;
            List<Connector> carConnectors = new List<Connector>();
            foreach(Connector connector in _connectors)
            {
                string cb = "checkbox" + connector.Id;
                check = Request.Form[cb];
                if(check != null)
                    carConnectors.Add(connector);
            }

            await _carService.AddNewCar(new Car(brand, model, maximumDistance, carConnectors, _currentUser));
            return Redirect(Url.Content("~/UserPanel"));
        }
    }
}
