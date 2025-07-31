using System;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;

namespace AzureAdWebAppFramework
{
    public partial class Startup
    {
        // Azure AD Configuration keys
        private static string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        private static string clientSecret = ConfigurationManager.AppSettings["ida:ClientSecret"];
        private static string aadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
        private static string tenantId = ConfigurationManager.AppSettings["ida:TenantId"];
        private static string postLogoutRedirectUri = ConfigurationManager.AppSettings["ida:PostLogoutRedirectUri"];
        private static string redirectUri = ConfigurationManager.AppSettings["ida:RedirectUri"];

        // Constructed authority URL
        string authority = aadInstance + tenantId;

        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            // Configure cookie authentication
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = CookieAuthenticationDefaults.AuthenticationType,
                LoginPath = new Microsoft.Owin.PathString("/Account/SignIn"),
                ExpireTimeSpan = TimeSpan.FromMinutes(60),
                SlidingExpiration = true
            });

            // Configure OpenID Connect authentication with Azure AD
            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = clientId,
                    Authority = authority,
                    PostLogoutRedirectUri = postLogoutRedirectUri,
                    RedirectUri = redirectUri,
                    Scope = "openid profile email",
                    ResponseType = "id_token",
                    TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        // In production, set ValidateIssuer = true and specify the issuer
                        // ValidIssuer = "https://sts.windows.net/{tenantId}/"
                    },
                    Notifications = new OpenIdConnectAuthenticationNotifications
                    {
                        AuthenticationFailed = OnAuthenticationFailed,
                        SecurityTokenValidated = OnSecurityTokenValidated
                    }
                }
            );
        }

        /// <summary>
        /// Handle failed authentication requests by redirecting the user to the home page with an error in the query string
        /// </summary>
        /// <param name="context">The authentication failure context</param>
        /// <returns>A task</returns>
        private Task OnAuthenticationFailed(AuthenticationFailedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> context)
        {
            context.HandleResponse();
            context.Response.Redirect("/?errormessage=" + context.Exception.Message);
            return Task.FromResult(0);
        }

        /// <summary>
        /// Handle successful token validation
        /// </summary>
        /// <param name="context">The security token validation context</param>
        /// <returns>A task</returns>
        private Task OnSecurityTokenValidated(SecurityTokenValidatedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> context)
        {
            // Add custom claims processing if needed
            var claims = context.AuthenticationTicket.Identity.Claims;
            
            // You can add custom claims or modify existing ones here
            // For example:
            // context.AuthenticationTicket.Identity.AddClaim(new Claim("CustomClaim", "CustomValue"));

            return Task.FromResult(0);
        }
    }
}