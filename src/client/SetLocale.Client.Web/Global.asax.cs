using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using Castle.Windsor;
using Castle.Windsor.Installer;

using SetLocale.Client.Web.Configurations;
using SetLocale.Util;

namespace SetLocale.Client.Web
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            MvcHandler.DisableMvcResponseHeader = true;

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            PrepareIocContainer();

            PrepareLocalizationStrings();
        }

        private void PrepareLocalizationStrings()
        {
            var enTexts = new Dictionary<string, string>();
            enTexts.Add("Edit", "Edit");
            enTexts.Add("Delete", "Delete");
            enTexts.Add("Login", "Login");
            enTexts.Add("Email", "Email");
            enTexts.Add("Password", "Password");
            enTexts.Add("Reset", "Send Reset Password Link");
            enTexts.Add("ResetTitle", "Reset Password");
            enTexts.Add("NewTranslator", "New Translator User");
            enTexts.Add("Name", "Name");
            enTexts.Add("Save", "Save");
            enTexts.Add("Users", "Users");
            enTexts.Add("Role", "Role");
            enTexts.Add("Deactivate", "Deactivate");
            enTexts.Add("Activate", "Activate");
            enTexts.Add("NewUser", "NewUser");
            enTexts.Add("UserName", "UserName");
            enTexts.Add("SignUp", "Sign Up");
            enTexts.Add("NewApp", "NewApp");
            enTexts.Add("AppName", "Application Name");
            enTexts.Add("Url", "Url");
            enTexts.Add("Description", "Description");
            enTexts.Add("NewKey", "NewKey");
            enTexts.Add("KeyListing", "KeyListing");
            enTexts.Add("Key", "Key");
            enTexts.Add("Tag", "Tag");
            enTexts.Add("Translated", "Translated");
            enTexts.Add("NotTranslated", "NotTranslated");
            enTexts.Add("TranslatedLang", "TranslatedLang");
            enTexts.Add("Apps", "Applications");
            enTexts.Add("App", "Application");
            enTexts.Add("Token", "Token");
            enTexts.Add("CreationDate", "Creation Date");
            enTexts.Add("CreateNewToken", "Create New Token");
            enTexts.Add("Cancel", "Hayır");
            enTexts.Add("Ok", "Evet");
            enTexts.Add("ModalBody", "Silmek İstediğinize Eminmisiniz ?");
            enTexts.Add("ModalTitle", "Token Sil");
            enTexts.Add("ModalBodyUsers", "Deactivate Etmek İstediğinize Eminmisiniz ?");
            enTexts.Add("ModalTitleUsers", "User Deactivate");
            enTexts.Add("ModalTitleApps", "App Deactivate");
            enTexts.Add("UsageCount", "Usage Count");
            

            var trTexts = new Dictionary<string, string>();
            trTexts.Add("Edit", "Düzenle");
            trTexts.Add("Delete", "Sil");
            trTexts.Add("Login", "Giriş");
            trTexts.Add("Email", "Eposts");
            trTexts.Add("Password", "Şifre");
            trTexts.Add("Reset", "Şifre Sıfırlama Linkini Gönder");
            trTexts.Add("ResetTitle", "Reset Password");
            trTexts.Add("NewTranslator", "New Translator User");
            trTexts.Add("Name", "Name");
            trTexts.Add("Save", "Save");
            trTexts.Add("Users", "Users");
            trTexts.Add("Role", "Role");
            trTexts.Add("Deactivate", "Deactivate");
            trTexts.Add("Activate", "Activate");
            trTexts.Add("NewUser", "NewUser");
            trTexts.Add("UserName", "UserName");
            trTexts.Add("SignUp", "Sign Up");
            trTexts.Add("NewApp", "NewApp");
            trTexts.Add("AppName", "Application Name");
            trTexts.Add("Url", "Url");
            trTexts.Add("Description", "Description");
            trTexts.Add("NewKey", "NewKey");
            trTexts.Add("KeyListing", "KeyListing");
            trTexts.Add("Key", "Key");
            trTexts.Add("Tag", "Tag");
            trTexts.Add("Translated", "Translated");
            trTexts.Add("NotTranslated", "NotTranslated");
            trTexts.Add("TranslatedLang", "TranslatedLang");
            trTexts.Add("Apps", "Applications");
            trTexts.Add("App", "Application");
            trTexts.Add("Token", "Token");
            trTexts.Add("CreationDate", "Creation Date");
            trTexts.Add("CreateNewToken", "Create New Token");
            trTexts.Add("Cancel", "Hayır");
            trTexts.Add("Ok", "Evet");
            trTexts.Add("ModalBody", "Silmek İstediğinize Eminmisiniz ?");
            trTexts.Add("ModalTitle", "Token Sil");
            trTexts.Add("ModalBodyUsers", "Deactivate Etmek İstediğinize Eminmisiniz ?");
            trTexts.Add("ModalTitleUsers", "User Deactivate");
            trTexts.Add("ModalTitleApps", "App Deactivate");
            trTexts.Add("UsageCount", "Usage Count");


            Application.Add(ConstHelper.en_txt, enTexts);
            Application.Add(ConstHelper.tr_txt, trTexts);
        }

        private static void PrepareIocContainer()
        {
            var container = new WindsorContainer().Install(FromAssembly.This());
            var controllerFactory = new WindsorControllerFactory(container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }

        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Headers.Remove("X-Powered-By");
            HttpContext.Current.Response.Headers.Remove("X-AspNet-Version");
            HttpContext.Current.Response.Headers.Remove("X-AspNetMvc-Version");

            HttpContext.Current.Response.Headers.Set("Server", string.Format("Web Server ({0}) ", Environment.MachineName));
        }
    }
}