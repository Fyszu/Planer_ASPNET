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
        public List<Car> cars = null;
        public User currentUser;
        public string testMessage;
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
                    cars = await _carService.GetCarsByUser(User.Identity.Name);
                    currentUser = await _userService.GetUserByName(User.Identity.Name);
                    testMessage = "brak";
                }
                else
                {
                    _logger.LogCritical("U¿ytkownik nie posiada nazwy u¿ytkownika. " + User.Identity);
                    throw new Exception("Wyst¹pi³ b³¹d dotycz¹cy konta u¿ytkownika.");
                }
            }
            else
                Redirect(Url.Content("~/Login"));
        }

        [HttpGet("success")]
        public void OnGetSucces()
        {
            testMessage = "sukces";
        }

        [HttpGet("error")]
        public void OnGetError()
        {
            testMessage = "error";
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
