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
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly ICarService _carService;
        public List<Car> cars = null;
        public User currentUser;
        public string testMessage;
        public string ReturnUrl { get; set; }
        public UserPanelModel(IUserService userService, UserManager<User> userManager, SignInManager<User> signInManager, ICarService carService)
        {
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
            cars = await _carService.GetCarsByUser(User.Identity.Name);
            currentUser = await _userService.GetUserByName(User.Identity.Name);
            testMessage = "brak";
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
            currentUser = await _userService.GetUserByName(User.Identity.Name);
            Boolean showOnlyMyCars = false;
            string check = Request.Form["showMyCarsCheckBox"];
            if (check != null)
                showOnlyMyCars = true;
            await _userService.SaveSettings(currentUser.Id, showOnlyMyCars);
            return Redirect(Url.Content("~/UserPanel"));
        }
    }
}
