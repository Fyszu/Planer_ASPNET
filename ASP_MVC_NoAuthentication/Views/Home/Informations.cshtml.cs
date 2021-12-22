using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Authorization;

namespace ASP_MVC_NoAuthentication.Views.Home
{
    [Authorize]
    public class InformationsModel : PageModel
    {
        public void OnGet()
        {

        }
    }
}
