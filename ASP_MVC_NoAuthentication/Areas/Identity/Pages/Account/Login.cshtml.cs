using ASP_MVC_NoAuthentication.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;

namespace ASP_MVC_NoAuthentication.Views.Home
{
    public static class ModelStateExtensions
    {
        public static ModelStateDictionary MarkAllFieldsAsSkipped(this ModelStateDictionary modelState)
        {
            foreach (var state in modelState.Select(x => x.Value))
            {
                state.Errors.Clear();
                state.ValidationState = ModelValidationState.Skipped;
            }
            return modelState;
        }
    }

    public class LoginModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel (UserManager<User> userManager, SignInManager<User> signInManager, ILogger<LoginModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputRegisterModel InputRegister { get; set; }

        [BindProperty]
        public InputLoginModel InputLogin { get; set; }

        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputRegisterModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "E-mail")]
            public string EmailRegister { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Has³o")]
            public string PasswordRegister { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Powtórz has³o")]
            [Compare("PasswordRegister", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPasswordRegister { get; set; }

            public string DrivingStyle = "mieszany";
            public double WinterFactor = 0.35;
            public double SummerFactor = 0.15;
            public string HighwaySpeed = "test";
        }

        public class InputLoginModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "E-mail")]
            public string EmailLogin { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Has³o")]
            public string PasswordLogin { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostRegisterAsync(string? returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ModelState.MarkAllFieldsAsSkipped();
            if (TryValidateModel(InputRegister, nameof(InputRegister)))
            {
                var user = new User { UserName = InputRegister.EmailRegister, Email = InputRegister.EmailRegister, DrivingStyle = "mieszany", WinterFactor = 0.35, SummerFactor = 0.15, HighwaySpeed = "test"};
                var result = await _userManager.CreateAsync(user, InputRegister.PasswordRegister);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form  
            return Page();
        }

        public async Task<IActionResult> OnPostLoginAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ModelState.MarkAllFieldsAsSkipped();
            if (TryValidateModel(InputLogin, nameof(InputLogin)))
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(InputLogin.EmailLogin, InputLogin.PasswordLogin, InputLogin.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
