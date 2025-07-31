using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;

namespace AzureAdWebAppFramework.Controllers
{
    public class AccountController : Controller
    {
        /// <summary>
        /// Send an OpenID Connect sign-in request.
        /// </summary>
        [AllowAnonymous]
        public void SignIn()
        {
            if (!Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties { RedirectUri = "/" },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }

        /// <summary>
        /// Send an OpenID Connect sign-out request.
        /// </summary>
        public void SignOut()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(
                OpenIdConnectAuthenticationDefaults.AuthenticationType,
                CookieAuthenticationDefaults.AuthenticationType);
        }

        /// <summary>
        /// Display user profile information
        /// </summary>
        public ActionResult Profile()
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("SignIn");
            }

            var identity = (ClaimsIdentity)User.Identity;
            ViewBag.UserClaims = identity.Claims.ToList();
            
            // Extract specific claims for easy display
            ViewBag.DisplayName = identity.FindFirst("name")?.Value ?? 
                                  identity.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value ?? 
                                  "Unknown";
            ViewBag.Email = identity.FindFirst("email")?.Value ?? 
                           identity.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value ?? 
                           "Not available";
            ViewBag.ObjectId = identity.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value ?? 
                              identity.FindFirst("oid")?.Value ?? 
                              "Not available";
            ViewBag.TenantId = identity.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid")?.Value ?? 
                              identity.FindFirst("tid")?.Value ?? 
                              "Not available";

            return View();
        }

        /// <summary>
        /// Handle errors from Azure AD authentication
        /// </summary>
        [AllowAnonymous]
        public ActionResult SignInError()
        {
            ViewBag.Error = Request.QueryString["errormessage"];
            return View();
        }
    }
}