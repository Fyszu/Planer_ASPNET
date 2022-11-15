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
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserController> _logger;
        private readonly SignInManager<User> _signInManager;
        public UserController(SignInManager<User> signInManager, UserManager<User> userManager, ILogger<UserController> logger)
        {
            _userManager = userManager;
            _logger = logger;
            _signInManager = signInManager;
        }

        [HttpGet("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromQuery] string userId)
        {
            User? user = await _userManager.FindByIdAsync(userId);
            if (User.Identity != null && User.Identity.Name != null && User.Identity.IsAuthenticated)
            {
                if (user == null)
                {
                    _logger.LogWarning($"Użytkownik {User.Identity.Name} wysłał żądanie usunięcia nieistniejącego użytkownika ID: {userId} (zablokowano).");
                    return Unauthorized();
                }
                else if (User.Identity.Name == user.UserName)
                {
                    await _userManager.DeleteAsync(user);
                    _logger.LogInformation($"Użytkownik {user.UserName} o ID: {user.Id} został usunięty z bazy danych.");
                    if (await _userManager.FindByIdAsync(userId) != null)
                    {
                        // User found after removal - something went wrong.
                        _logger.LogCritical($"Nie udało się usunąć konta użytkownika {user.UserName}.");
                        throw new Exception("Problem z usunięciem konta użytkownika.");
                    }
                    await _signInManager.SignOutAsync();
                    return Redirect(Url.Content("~/"));
                }
                else
                {
                    _logger.LogWarning($"Użytkownik {User.Identity.Name} wysłał żądanie usunięcia użytkownika {user.UserName} (ID: {user.Id}) (zablokowano).");
                    return Unauthorized();
                }
            }
            _logger.LogError($"Niezalogowany użytkownik prbówał usunąć konto użytkownika {userId}.");
            return Unauthorized();
        }
    }
}
