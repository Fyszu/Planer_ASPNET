using RoutePlanner.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Win32;
using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace RoutePlanner.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly ILogger<LoginModel> logger;

        public LoginModel (UserManager<User> userManager, SignInManager<User> signInManager, ILogger<LoginModel> logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
        }

        [BindProperty]
        public InputRegisterModel InputRegister { get; set; }

        [BindProperty]
        public InputLoginModel InputLogin { get; set; }
        public bool IsReturnFromRegisterPost { get; set; }
        private static string ReturnUrl { get; set; }

        public class InputRegisterModel
        {
            [Required(ErrorMessage = "To pole jest wymagane.<br>")]
            [EmailAddress(ErrorMessage = "Podano nieprawid�owy adres e-mail.<br>")]
            [Display(Name = "E-mail")]
            public string EmailRegister { get; set; }

            [Required(ErrorMessage = "To pole jest wymagane.<br>")]
            [StringLength(20, ErrorMessage = "{0} musi posiada� od {2} do {1} znak�w.<br>", MinimumLength = 8)]
            [RegularExpression(@"(?=.*[^A-Za-z0-9])(?=.*[A-Z]).*$", ErrorMessage = "Has�o musi posiada� przynajmniej jeden znak specjalny i jedn� wielk� liter�.<br>")]
            [DataType(DataType.Password)]
            [Display(Name = "Has�o")]
            public string PasswordRegister { get; set; }

            [Required(ErrorMessage = "To pole jest wymagane.<br>")]
            [DataType(DataType.Password)]
            [Display(Name = "Powt�rz has�o")]
            [Compare("PasswordRegister", ErrorMessage = "Has�a nie s� takie same.<br>")]
            public string ConfirmPasswordRegister { get; set; }
        }

        public class InputLoginModel
        {
            [Required(ErrorMessage = "To pole jest wymagane.<br>")]
            [EmailAddress(ErrorMessage = "Podano nieprawid�owy adres e-mail.<br>")]
            [Display(Name = "E-mail")]
            public string EmailLogin { get; set; }

            [Required(ErrorMessage = "To pole jest wymagane.<br>")]
            [DataType(DataType.Password)]
            [Display(Name = "Has�o")]
            public string PasswordLogin { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string? returnUrl = null)
        {
            IsReturnFromRegisterPost = false;
            ReturnUrl = returnUrl ?? "~/";
        }

        public async Task<IActionResult> OnPostRegisterAsync()
        {
            ModelState.MarkLoginFormAsSkipped();
            if (TryValidateModel(InputRegister, nameof(InputRegister)))
            {
                var user = new User { UserName = InputRegister.EmailRegister, Email = InputRegister.EmailRegister, ShowOnlyMyCars = false };
                var result = await userManager.CreateAsync(user, InputRegister.PasswordRegister);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    logger.LogInformation($"Zosta� zarejestrowany nowy u�ytkownik o adresie email {user.UserName}.");
                    return LocalRedirect("~/UserPanel");
                }
                foreach (var error in result.Errors)
                {
                    if (error.Description.Contains("is already taken") && (error.Description.Contains("Username") || error.Description.Contains("Email")))
                    {
                        ModelState.AddModelError("Register", "Istnieje ju� konto o podanym adresie email.");
                        break;
                    }
                    else
                    {
                        logger.LogError($"Nietypowy b��d podczas rejestracji: {error.Description}.");
                        ModelState.AddModelError("Register", error.Description);
                        break;
                    }
                }
                IsReturnFromRegisterPost = true;
                return Page();
            }

            // If we got this far, something failed, redisplay form  
            ModelState.AddModelError("Register", "Co� posz�o nie tak. Spr�buj jeszcze raz.");
            logger.LogError($"Nietypowy b��d podczas rejestracji - walidacja nie powiod�a si�.");
            IsReturnFromRegisterPost = true;
            return Page();
        }

        public async Task<IActionResult> OnPostLoginAsync()
        {
            ModelState.MarkRegisterFormAsSkipped();
            if (TryValidateModel(InputLogin, nameof(InputLogin)))
            {
                var result = await signInManager.PasswordSignInAsync(InputLogin.EmailLogin, InputLogin.PasswordLogin, InputLogin.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    logger.LogInformation($"Zalogowano u�ytkownika {InputLogin.EmailLogin}.");
                    return LocalRedirect(ReturnUrl);
                }
                else if (result.IsLockedOut) // Locked - max. retry attempts and timespan configured in Program.cs
                {
                    ModelState.AddModelError("Login", "Twoje konto zosta�o chwilowo zablokowane, spr�buj ponownie za 5 minut.");
                    return Page();
                }
                else
                {
                    var user = await userManager.FindByNameAsync(InputLogin.EmailLogin);
                    if (user != null && user.AccessFailedCount == 3)
                    {
                        logger.LogInformation($"3 nieudana pr�ba zalogowania si� na konto u�ytkownika {user.UserName}.");
                        ModelState.AddModelError("Login", "Pozosta�a jedna pr�ba zalogowania si�. Po nieudanej pr�bie konto zostanie zablokowane na 5 minut.");
                    }
                    else
                    {
                        ModelState.AddModelError("Login", "B��dny login lub has�o.");
                    }
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("Login", "Co� posz�o nie tak. Spr�buj jeszcze raz.");
            logger.LogError($"Nietypowy b��d podczas logowania - walidacja nie powiod�a si�.");
            return Page();
        }
    }

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
}
