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
        private readonly IHomeService _service;
        public List<Car> cars = null;
        public User currentUser;
        public string ReturnUrl { get; set; }
        public UserPanelModel(IHomeService service, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _service = service;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task OnGetAsync()
        {
            await GetPage();
        }

        public async Task GetPage()
        {
            cars = _service.getPersonalisableCars(User.Identity.Name);
            currentUser = _service.getUser(User.Identity.Name);
        }

        public async Task<IActionResult> OnPostSaveSettingsAsync(string? returnUrl = null)
        {
            currentUser = _service.getUser(User.Identity.Name);
            var summer = Request.Form["summerFactorRange"];
            var summer2 = int.Parse(summer);
            double summer3 = (double)summer2 / 100;
            double summerFactor = ((double)(int.Parse(Request.Form["summerFactorRange"])) / 100);
            double winterFactor = ((double)(int.Parse(Request.Form["winterFactorRange"])) / 100);
            string drivingStyle = Request.Form["drivingStyleSelect"];
            _service.saveSettings(summerFactor, winterFactor, drivingStyle, currentUser.Id);
            return Redirect(Url.Content("~/UserPanel"));
        }
    }
}
