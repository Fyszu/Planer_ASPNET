using ASP_MVC_NoAuthentication.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Win32;
using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace ASP_MVC_NoAuthentication.Pages.Account
{
    public static class ModelStateExtensions
    {
        public static ModelStateDictionary MarkLoginFormAsSkipped(this ModelStateDictionary modelState)
        {
            foreach (var state in modelState.Where(state => state.Key.Contains("Login")).Select(state => state.Value))
            {
                state.Errors.Clear();
                state.ValidationState = ModelValidationState.Skipped;
            }
            return modelState;
        }

        public static ModelStateDictionary MarkRegisterFormAsSkipped(this ModelStateDictionary modelState)
        {
            foreach (var state in modelState.Where(state => state.Key.Contains("Register")).Select(state => state.Value))
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

        [BindProperty(SupportsGet = true)]
        public string Handler { get; set; }
        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public bool IsReturnFromRegisterPost { get { return Handler != null && Handler.Equals("Register"); } }

        public class InputRegisterModel
        {
            [Required(ErrorMessage = "To pole jest wymagane.<br>")]
            [EmailAddress(ErrorMessage = "Podano nieprawid³owy adres e-mail.<br>")]
            [Display(Name = "E-mail")]
            public string EmailRegister { get; set; }

            [Required(ErrorMessage = "To pole jest wymagane.<br>")]
            [StringLength(20, ErrorMessage = "{0} musi posiadaæ od {2} do {1} znaków.<br>", MinimumLength = 8)]
            [RegularExpression(@"(?=.*[^A-Za-z0-9])(?=.*[A-Z]).*$", ErrorMessage = "Has³o musi posiadaæ przynajmniej jeden znak specjalny i jedn¹ wielk¹ literê.<br>")]
            [DataType(DataType.Password)]
            [Display(Name = "Has³o")]
            public string PasswordRegister { get; set; }

            [Required(ErrorMessage = "To pole jest wymagane.<br>")]
            [DataType(DataType.Password)]
            [Display(Name = "Powtórz has³o")]
            [Compare("PasswordRegister", ErrorMessage = "Has³a nie s¹ takie same.<br>")]
            public string ConfirmPasswordRegister { get; set; }
        }

        public class InputLoginModel
        {
            [Required(ErrorMessage = "To pole jest wymagane.<br>")]
            [EmailAddress(ErrorMessage = "Podano nieprawid³owy adres e-mail.<br>")]
            [Display(Name = "E-mail")]
            public string EmailLogin { get; set; }

            [Required(ErrorMessage = "To pole jest wymagane.<br>")]
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
            ModelState.MarkLoginFormAsSkipped();
            if (TryValidateModel(InputRegister, nameof(InputRegister)))
            {
                var user = new User { UserName = InputRegister.EmailRegister, Email = InputRegister.EmailRegister, ShowOnlyMyCars = false };
                var result = await _userManager.CreateAsync(user, InputRegister.PasswordRegister);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    if (error.Description.Contains("is already taken") && (error.Description.Contains("Username") || error.Description.Contains("Email")))
                    {
                        ModelState.AddModelError("Register", "Istnieje ju¿ konto o podanym adresie email.");
                        break;
                    }
                    else
                    {
                        ModelState.AddModelError("Register", error.Description);
                        break;
                    }
                }
            }

            // If we got this far, something failed, redisplay form  
            return Page();
        }

        public async Task<IActionResult> OnPostLoginAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ModelState.MarkRegisterFormAsSkipped();
            if (TryValidateModel(InputLogin, nameof(InputLogin)))
            {
                var result = await _signInManager.PasswordSignInAsync(InputLogin.EmailLogin, InputLogin.PasswordLogin, InputLogin.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }
                else if (result.IsLockedOut) // Locked - max. retry attempts and timespan configured in Program.cs
                {
                    ModelState.AddModelError("Login", "Twoje konto zosta³o chwilowo zablokowane, spróbuj ponownie za 5 minut.");
                    return Page();
                }
                else
                {
                    var user = await _userManager.FindByNameAsync(InputLogin.EmailLogin);
                    if (user != null && user.AccessFailedCount == 3)
                    {
                        ModelState.AddModelError("Login", "Pozosta³a jedna próba zalogowania siê. Po nieudanej próbie konto zostanie zablokowane na 5 minut.");
                        return Page();
                    }
                    ModelState.AddModelError("Login", "B³êdny login lub has³o.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
