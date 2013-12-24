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
            dictionary.Add("Edit", "Edit");
            dictionary.Add("Delete", "Delete");

            dictionary.Add("Login", "Login");
            dictionary.Add("Email", "Email"); 
            dictionary.Add("Password", "Password");

            dictionary.Add("Reset", "Send Reset Password Link");
            dictionary.Add("ResetTitle", "Reset Password");

            dictionary.Add("NewTranslator", "New Translator User");
            dictionary.Add("Name", "Name");
            dictionary.Add("Save", "Save");

            dictionary.Add("Users", "Users");
            dictionary.Add("Role", "Role");
            dictionary.Add("Deactivate", "Deactivate");
            dictionary.Add("Activate", "Activate");

            dictionary.Add("NewUser", "NewUser");
            dictionary.Add("UserName", "UserName");
            dictionary.Add("SignUp", "Sign Up");

            dictionary.Add("NewApp", "NewApp");
            dictionary.Add("AppName", "Application Name");
            dictionary.Add("Url", "Url");
            dictionary.Add("Description", "Description");

            dictionary.Add("NewKey", "NewKey");
            dictionary.Add("KeyListing", "KeyListing");
            dictionary.Add("Key", "Key");
            dictionary.Add("Tag", "Tag");
            dictionary.Add("Translated", "Translated");
            dictionary.Add("NotTranslated", "NotTranslated");
            dictionary.Add("TranslatedLang", "TranslatedLang");

            dictionary.Add("Apps", "Applications");
            dictionary.Add("App", "Application");
            dictionary.Add("Token", "Token");
            dictionary.Add("CreationDate", "Creation Date");
            dictionary.Add("CreateNewToken", "Create New Token"); 






            dictionary.Add("UsageCount", "Usage Count"); 


            ViewBag.Txt = dictionary;
            base.OnActionExecuting(filterContext);
        }
    }
}