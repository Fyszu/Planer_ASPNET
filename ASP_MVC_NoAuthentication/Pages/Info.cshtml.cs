using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASP_MVC_NoAuthentication.Views.Home
{
    public class InfoModel : PageModel
    {
        public string Message { get; private set; } = "PageModel in C#";
        public void OnGet()
        {
            Message += $" Server time is { DateTime.Now }";
        }
    }
}
