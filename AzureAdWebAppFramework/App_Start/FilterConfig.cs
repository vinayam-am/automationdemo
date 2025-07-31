using System.Web;
using System.Web.Mvc;

namespace AzureAdWebAppFramework
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            
            // Add global authorization filter
            // This requires users to be authenticated for all controllers/actions
            // unless they have [AllowAnonymous] attribute
            filters.Add(new AuthorizeAttribute());
        }
    }
}