using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASP_MVC_NoAuthentication.Pages
{
    [Authorize]
    public class UserPanelModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
