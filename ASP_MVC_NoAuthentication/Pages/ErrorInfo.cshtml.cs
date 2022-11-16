using System.Diagnostics;
using ASP_MVC_NoAuthentication.Data;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASP_MVC_NoAuthentication.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public string? ExceptionMessage { get; set; }
        public string? InnerException { get; set; }
        public void OnGetAsync()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            var exceptionHandlerPathFeature =
                HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionHandlerPathFeature is null)
                ExceptionMessage = "Brak szczegó³ów o b³êdzie.";
            else if (exceptionHandlerPathFeature.Error is JsonDeserializationException)
                ExceptionMessage = $"JSON deserialization problem. \n{exceptionHandlerPathFeature.Error.Message}";
            else
            {
                ExceptionMessage = exceptionHandlerPathFeature.Error.Message;
                InnerException = exceptionHandlerPathFeature.Error.InnerException?.ToString();
            }

            if (string.IsNullOrEmpty(ExceptionMessage))
                ExceptionMessage = "Brak informacji o b³êdzie.";

            if (string.IsNullOrEmpty(InnerException))
                InnerException = "Brak dalszych szczegó³ów.";
        }
    }
}
