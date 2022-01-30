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
        private readonly ConnectorRepository _repository;
        private readonly IHomeService _homeService;
        private readonly ICarService _carService;
        public List<Connector> _connectors = null;
        public User _currentUser;

        public AddNewCarModel(UserManager<User> userManager, SignInManager<User> signInManager, ConnectorRepository repository, IHomeService homeService, ICarService carService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _repository = repository;
            _homeService = homeService;
            _carService = carService;
        }

        public async Task OnGetAsync()
        {
            await GetPage();
        }
        public async Task GetPage()
        {
            _connectors = _repository.GetAllConnectors();
            _currentUser = _homeService.getUser(User.Identity.Name);
        }
        public async Task<IActionResult> OnPostAddCarAsync(string? returnUrl = null)
        {
            _currentUser = _homeService.getUser(User.Identity.Name);
            _connectors = _repository.GetAllConnectors();
            string brand = Request.Form["brand"];
            string model = Request.Form["carmodel"];
            int maximumDistance = (int.Parse(Request.Form["maximumdistance"]));
            string check;
            List<Connector> carConnectors = new List<Connector>();
            foreach(Connector connector in _connectors)
            {
                string cb = "checkbox" + connector.Id;
                check = Request.Form[cb];
                if(check != null)
                    carConnectors.Add(connector);
            }
            int x = _carService.AddUserCar(_currentUser, new Car(brand, model, maximumDistance, carConnectors));
            return Redirect(Url.Content("~/UserPanel"));
        }
    }
}
