using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASP_MVC_NoAuthentication.Views.Home
{
    [Authorize]
    public class InfoModel : PageModel
    {
        public void OnGet()
        {
            Console.WriteLine("OK");
        }
    }
}
