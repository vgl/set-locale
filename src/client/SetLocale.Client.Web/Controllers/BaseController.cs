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

            dictionary.Add("NewTranslator", "New Translator User");
            dictionary.Add("Name", "Name");
            dictionary.Add("Save", "Save");

            dictionary.Add("Users", "Users");
            dictionary.Add("Role", "Role");
            dictionary.Add("Deactivate", "Deactivate");
            dictionary.Add("Activate", "Activate");

            dictionary.Add("NewUser", "NewUser");
            dictionary.Add("UserName", "UserName");
            dictionary.Add("Sign Up", "Sign Up");

            ViewBag.Txt = dictionary;
            base.OnActionExecuting(filterContext);
        }
    }
}