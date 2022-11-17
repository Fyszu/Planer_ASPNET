using ASP_MVC_NoAuthentication.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ASP_MVC_NoAuthentication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly ILogger<UserController> logger;
        private readonly SignInManager<User> signInManager;
        public UserController(SignInManager<User> signInManager, UserManager<User> userManager, ILogger<UserController> logger)
        {
            this.userManager = userManager;
            this.logger = logger;
            this.signInManager = signInManager;
        }

        [HttpGet("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromQuery] string userId)
        {
            User? user = await userManager.FindByIdAsync(userId);
            if (User.Identity != null && User.Identity.Name != null && User.Identity.IsAuthenticated)
            {
                if (user == null)
                {
                    logger.LogWarning($"Użytkownik {User.Identity.Name} wysłał żądanie usunięcia nieistniejącego użytkownika ID: {userId} (zablokowano).");
                    return Unauthorized();
                }
                else if (User.Identity.Name == user.UserName)
                {
                    await userManager.DeleteAsync(user);
                    logger.LogInformation($"Użytkownik {user.UserName} o ID: {user.Id} został usunięty z bazy danych.");
                    if (await userManager.FindByIdAsync(userId) != null)
                    {
                        // User found after removal - something went wrong.
                        logger.LogCritical($"Nie udało się usunąć konta użytkownika {user.UserName}.");
                        throw new Exception("Problem z usunięciem konta użytkownika.");
                    }
                    await signInManager.SignOutAsync();
                    return Redirect(Url.Content("~/"));
                }
                else
                {
                    logger.LogWarning($"Użytkownik {User.Identity.Name} wysłał żądanie usunięcia użytkownika {user.UserName} (ID: {user.Id}) (zablokowano).");
                    return Unauthorized();
                }
            }
            logger.LogError($"Niezalogowany użytkownik prbówał usunąć konto użytkownika {userId}.");
            return Unauthorized();
        }
    }
}
