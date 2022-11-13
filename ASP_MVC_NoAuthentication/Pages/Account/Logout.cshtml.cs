using ASP_MVC_NoAuthentication.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASP_MVC_NoAuthentication.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(SignInManager<User> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }
        public async Task<IActionResult> OnGet(string? returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (User.Identity.IsAuthenticated)
            {
                await _signInManager.SignOutAsync();
                _logger.LogInformation("User logged out.");
                if (returnUrl != null)
                {
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    return RedirectToPage();
                }
            }
            else
            {
                return LocalRedirect(returnUrl);
            }
        }


    }
}
