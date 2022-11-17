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
        private readonly ILogger<UserPanelModel> logger;
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly IUserService userService;
        private readonly ICarService carService;
        private List<Car> cars;
        private User currentUser;

        public UserPanelModel(ILogger<UserPanelModel> logger, IUserService userService, UserManager<User> userManager, SignInManager<User> signInManager, ICarService carService)
        {
            this.logger = logger;
            this.userService = userService;
            this.carService = carService;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public User CurrentUser { get { return currentUser; } }
        public List<Car> Cars { get { return cars; } }

        public async Task OnGetAsync()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                if (!string.IsNullOrEmpty(User.Identity.Name))
                {
                    currentUser = await userService.GetUserByNameAsync(User.Identity.Name);
                    if (currentUser != null)
                    {
                        cars = currentUser.Cars.ToList() ?? throw new Exception("Wyst¹pi³ b³¹d dotycz¹cy konta u¿ytkownika - lista samochodów nie istnieje.");
                    }
                    else
                    {
                        logger.LogCritical($"Nie znaleziono u¿ytkownika {User.Identity.Name} w bazie danych.");
                        await signInManager.SignOutAsync();
                        throw new Exception("Wyst¹pi³ b³¹d dotycz¹cy konta u¿ytkownika.");
                    }
                }
                else
                {
                    logger.LogCritical("Nazwa u¿ytkownika jest pusta. " + User.Identity);
                    throw new Exception("Wyst¹pi³ b³¹d dotycz¹cy konta u¿ytkownika.");
                }
            }
            else
                Redirect(Url.Content("~/Login"));
        }

        public async Task<IActionResult> OnPostSaveSettingsAsync()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                if (!string.IsNullOrEmpty(User.Identity.Name))
                {
                    currentUser = await userService.GetUserByNameAsync(User.Identity.Name);
                    string check = Request.Form["showMyCarsCheckBox"];
                    if (check != null)
                        currentUser.ShowOnlyMyCars = true;
                    else
                        currentUser.ShowOnlyMyCars = false;
                    await userService.SaveSettingsAsync(currentUser);
                    return Redirect(Url.Content("~/UserPanel"));
                }
                else
                {
                    logger.LogCritical("Nazwa u¿ytkownika jest pusta." + User.Identity);
                    throw new Exception("Wyst¹pi³ b³¹d dotycz¹cy konta u¿ytkownika.");
                }
            }
            else
                return Redirect(Url.Content("~/Login"));
        }
    }
}
