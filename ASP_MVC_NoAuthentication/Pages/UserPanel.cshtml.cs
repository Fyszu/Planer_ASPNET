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
    public class UserPanelModel : PageModel
    {
        private readonly ILogger<UserPanelModel> _logger;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly ICarService _carService;
        private List<Car> cars;
        private User currentUser;
        public User CurrentUser { get { return currentUser; } }
        public List<Car> Cars { get { return cars; } }
        public string ReturnUrl { get; set; }
        public UserPanelModel(ILogger<UserPanelModel> logger, IUserService userService, UserManager<User> userManager, SignInManager<User> signInManager, ICarService carService)
        {
            _logger = logger;
            _userService = userService;
            _carService = carService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task OnGetAsync()
        {
            await GetPage();
        }

        public async Task GetPage()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                if (!string.IsNullOrEmpty(User.Identity.Name))
                {
                    currentUser = await _userService.GetUserByName(User.Identity.Name);
                    if (currentUser != null)
                    {
                        cars = currentUser.Cars.ToList();
                        if (cars == null)
                        {
                            throw new Exception("Wyst¹pi³ b³¹d dotycz¹cy konta u¿ytkownika.");
                        }
                    }
                    else
                    {
                        _logger.LogCritical($"Nie znaleziono u¿ytkownika {User.Identity.Name} w bazie danych.");
                        await _signInManager.SignOutAsync();
                        throw new Exception("Wyst¹pi³ b³¹d dotycz¹cy konta u¿ytkownika.");
                    }
                }
                else
                {
                    _logger.LogCritical("Nazwa u¿ytkownika jest pusta. " + User.Identity);
                    throw new Exception("Wyst¹pi³ b³¹d dotycz¹cy konta u¿ytkownika.");
                }
            }
            else
                Redirect(Url.Content("~/Login"));
        }

        public async Task<IActionResult> OnPostSaveSettingsAsync(string? returnUrl = null)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                if (!string.IsNullOrEmpty(User.Identity.Name))
                {
                    currentUser = await _userService.GetUserByName(User.Identity.Name);
                    string check = Request.Form["showMyCarsCheckBox"];
                    if (check != null)
                        currentUser.ShowOnlyMyCars = true;
                    else
                        currentUser.ShowOnlyMyCars = false;
                    await _userService.SaveSettings(currentUser);
                    return Redirect(Url.Content("~/UserPanel"));
                }
                else
                {
                    _logger.LogCritical("U¿ytkownik nie posiada nazwy u¿ytkownika. " + User.Identity);
                    throw new Exception("Wyst¹pi³ b³¹d dotycz¹cy konta u¿ytkownika.");
                }
            }
            else
                return Redirect(Url.Content("~/Login"));
        }
    }
}
