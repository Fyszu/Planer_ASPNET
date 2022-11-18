using System.Diagnostics;
using RoutePlanner.Data;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RoutePlanner.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        private readonly ILogger<ErrorModel> logger;
        private readonly SignInManager<User> signInManager;

        public ErrorModel(ILogger<ErrorModel> logger, SignInManager<User> signInManager)
        {
            this.logger = logger;
            this.signInManager = signInManager;
        }

        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public string? ExceptionMessage { get; set; }
        public string? InnerException { get; set; }
        public void OnGetAsync()
        {
            bool isLoggedIn = signInManager.IsSignedIn(User) && User.Identity != null && User.Identity.Name != null;
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            var exceptionHandlerPathFeature =
                HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionHandlerPathFeature is null)
                ExceptionMessage = "Brak szczeg��w b��du.";
            else if (exceptionHandlerPathFeature.Error is DatabaseException)
                ExceptionMessage = $"Problem dotyczy zapisu lub odczytu z bazy danych. \n{exceptionHandlerPathFeature.Error.Message}";
            else if (exceptionHandlerPathFeature.Error is NoDataInDatabaseException)
                ExceptionMessage = $"Problem dotyczy bazy danych - nie znaleziono niezb�dnych danych. \n{exceptionHandlerPathFeature.Error.Message}";
            else if (exceptionHandlerPathFeature.Error is UserIsNullException)
                ExceptionMessage = $"Problem dotyczy b��du odczytu u�ytkownika. \n{exceptionHandlerPathFeature.Error.Message}";
            else if (exceptionHandlerPathFeature.Error is ValueNotFoundInConfigurationException)
                ExceptionMessage = $"Nie znaleziono warto�ci podczas odczytu z pliku konfiguracyjnego. Problem dotyczy: {exceptionHandlerPathFeature.Error.Message}";
            else
            {
                ExceptionMessage = exceptionHandlerPathFeature.Error.Message;
                InnerException = exceptionHandlerPathFeature.Error.InnerException?.ToString();
            }

            if (string.IsNullOrEmpty(ExceptionMessage))
                ExceptionMessage = "Brak informacji o b��dzie.";

            InnerException ??= string.Empty;

            logger.LogCritical($"Wyst�pi� wyj�tek{(isLoggedIn ? $" dla u�ytkownika {User.Identity.Name}" : "")}. {ExceptionMessage}\n{InnerException}");
        }
    }
}
