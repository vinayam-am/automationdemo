using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace AzureAdWebAppFramework.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Your Azure AD authentication demo application.";
            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        // This action requires authentication (no [AllowAnonymous] attribute)
        public ActionResult Secure()
        {
            var identity = (ClaimsIdentity)User.Identity;
            ViewBag.UserClaims = identity.Claims.ToList();
            return View();
        }
    }
}