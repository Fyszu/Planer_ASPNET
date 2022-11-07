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
        private readonly IConnectorInterfaceService _connectorInterfaceService;
        public List<ConnectorInterface> _connectorInterfaces = null;
        public User _currentUser;

        public AddNewCarModel(UserManager<User> userManager, SignInManager<User> signInManager, IUserService userService, ICarService carService, IConnectorInterfaceService connectorInterfaceService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _carService = carService;
            _connectorInterfaceService = connectorInterfaceService;
        }

        public async Task OnGetAsync()
        {
            await GetPage();
        }
        public async Task GetPage()
        {
            _connectorInterfaces = await _connectorInterfaceService.GetAllConnectorInterfaces();
            _currentUser = await _userService.GetUserByName(User.Identity.Name);
        }
        public async Task<IActionResult> OnPostAddCarAsync(string? returnUrl = null)
        {
            _currentUser = await _userService.GetUserByName(User.Identity.Name);
            _connectorInterfaces = await _connectorInterfaceService.GetAllConnectorInterfaces();
            string brand = Request.Form["brand"];
            string model = Request.Form["carmodel"];
            int maximumDistance = int.Parse(Request.Form["maximumdistance"]);
            string check;
            List<ConnectorInterface> carInterfaces = new List<ConnectorInterface>();
            foreach(ConnectorInterface cnInterface in _connectorInterfaces)
            {
                string cb = "checkbox" + cnInterface.Id;
                check = Request.Form[cb];
                if(check != null)
                    carInterfaces.Add(cnInterface);
            }

            await _carService.AddNewCar(new Car(brand, model, maximumDistance, carInterfaces, _currentUser));
            return Redirect(Url.Content("~/UserPanel"));
        }
    }
}
