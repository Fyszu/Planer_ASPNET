using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASP_MVC_NoAuthentication.Pages
{
    public class InfoModel : PageModel
    {
        public string Message { get; private set; } = "PageModel in C#";
        public void OnGet()
        {
           
        }
    }
}
