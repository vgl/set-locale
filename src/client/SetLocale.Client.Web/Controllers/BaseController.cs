using System.Collections.Generic;
using System.Web.Mvc;

namespace SetLocale.Client.Web.Controllers
{
    public class BaseController : Controller
    {
        public ActionResult RedirectToHome()
        {
            return RedirectToAction("Index", "Home");
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //temp usage...
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("Login", "Login");
            dictionary.Add("Email", "Email");
            dictionary.Add("Password", "Password");
            dictionary.Add("NewUser", "NewUser");
            dictionary.Add("UserName", "UserName");
            dictionary.Add("Sign Up", "Sign Up");

            ViewBag.Txt = dictionary;
            base.OnActionExecuting(filterContext);
        }
    }
}