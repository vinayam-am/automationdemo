using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AzureAdWebApp.Pages
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        public void OnGet()
        {
            // Page logic here - user claims are automatically available through User property
        }
    }
}