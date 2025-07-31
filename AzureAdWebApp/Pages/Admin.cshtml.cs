using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AzureAdWebApp.Pages
{
    [Authorize]
    public class AdminModel : PageModel
    {
        public void OnGet()
        {
            // Admin page logic here
            // In a real application, you might add role-based authorization like:
            // [Authorize(Roles = "Admin")] or [Authorize(Policy = "AdminPolicy")]
        }
    }
}