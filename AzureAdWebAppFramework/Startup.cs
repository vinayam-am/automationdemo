using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AzureAdWebAppFramework.Startup))]
namespace AzureAdWebAppFramework
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}