using System;
using System.Collections.Generic;
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
            enTexts.Add("NewUser", "New User");
            enTexts.Add("UserName", "User Name");
            enTexts.Add("SignUp", "Sign Up");
            enTexts.Add("NewApp", "New App");
            enTexts.Add("AppName", "Application Name");
            enTexts.Add("Url", "Url");
            enTexts.Add("Description", "Description");
            enTexts.Add("NewKey", "New Key");
            enTexts.Add("KeyListing", "Key Listing");
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
            enTexts.Add("Cancel", "Cancel");
            enTexts.Add("Ok", "Ok");
            enTexts.Add("ModalBody", "Are you sure  want to delete ?");
            enTexts.Add("ModalTitle", "Token Delete");
            enTexts.Add("ModalBodyUsers", "Are you sure  want to change the status ?");
            enTexts.Add("ModalTitleUsers", "User Status");
            enTexts.Add("ModalTitleApps", "App Status");
            enTexts.Add("UsageCount", "Usage Count");
            enTexts.Add("Search", "Search");
            

            var trTexts = new Dictionary<string, string>();
            trTexts.Add("Edit", "Düzenle");
            trTexts.Add("Delete", "Sil");
            trTexts.Add("Login", "Giriş");
            trTexts.Add("Email", "E-posta");
            trTexts.Add("Password", "Şifre");
            trTexts.Add("Reset", "Şifre Sıfırlama Linkini Gönder");
            trTexts.Add("ResetTitle", "Şifre Sıfırla");
            trTexts.Add("NewTranslator", "Yeni Çevirmen");
            trTexts.Add("Name", "İsim");
            trTexts.Add("Save", "Kaydet");
            trTexts.Add("Users", "Kullanıcılar");
            trTexts.Add("Role", "Yetki Grubu");
            trTexts.Add("Deactivate", "Pasif");
            trTexts.Add("Activate", "Aktif");
            trTexts.Add("NewUser", "Yeni Kullanıcı");
            trTexts.Add("UserName", "Kullanıcı Adı");
            trTexts.Add("SignUp", "Kayıt Ol");
            trTexts.Add("NewApp", "Yeni Uygulama");
            trTexts.Add("AppName", "Uygulama İsmi");
            trTexts.Add("Url", "Url");
            trTexts.Add("Description", "Açıklama");
            trTexts.Add("NewKey", "Yeni Anahtar");
            trTexts.Add("KeyListing", "Anahtar Listesi");
            trTexts.Add("Key", "Anahtar");
            trTexts.Add("Tag", "Etiket");
            trTexts.Add("Translated", "Çevrildi");
            trTexts.Add("NotTranslated", "Çevrilmedi");
            trTexts.Add("TranslatedLang", "Çevrilmiş Dil");
            trTexts.Add("Apps", "Uygulamalar");
            trTexts.Add("App", "Uygulama");
            trTexts.Add("Token", "Token");
            trTexts.Add("CreationDate", "Oluşturma Tarihi");
            trTexts.Add("CreateNewToken", "Yeni Token Oluştur");
            trTexts.Add("Cancel", "Hayır");
            trTexts.Add("Ok", "Evet");
            trTexts.Add("ModalBody", "Silmek İstediğinize Eminmisiniz ?");
            trTexts.Add("ModalTitle", "Token Sil");
            trTexts.Add("ModalBodyUsers", "Durumu Değiştirmek İstediğinize Eminmisiniz ?");
            trTexts.Add("ModalTitleUsers", "Kullanıcı Durumu");
            trTexts.Add("ModalTitleApps", "Uygulama Durumu");
            trTexts.Add("UsageCount", "Kullanım Sayısı");
            trTexts.Add("Search", "Ara");


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