using RoutePlanner.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RoutePlanner.Pages.Account
{
    [Authorize]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<User> signInManager;
        private readonly ILogger<LogoutModel> logger;

        public LogoutModel(SignInManager<User> signInManager, ILogger<LogoutModel> logger)
        {
            this.signInManager = signInManager;
            this.logger = logger;
        }
        public async Task<IActionResult> OnGet(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            await signInManager.SignOutAsync();
            logger.LogInformation($"Wylogowano u¿ytkownika {User.Identity.Name}.");
            return LocalRedirect(returnUrl);
        }


    }
}
