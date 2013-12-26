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

            enTexts.Add("app_name", "Set Locale");


            #region Menu

            #endregion


            #region Login EN

            enTexts.Add("login_view_title", "Login to System");
            enTexts.Add("btn_login", "Login");
            enTexts.Add("email", "Email");
            enTexts.Add("password", "Password");

            #endregion





            enTexts.Add("Edit", "Edit");
            enTexts.Add("Delete", "Delete");
            enTexts.Add("Login", "Login");
            enTexts.Add("Email", "Email");
            enTexts.Add("Password", "Password");
            enTexts.Add("Reset", "Send Reset Password Link");
            enTexts.Add("ResetTitle", "Reset Password");
            enTexts.Add("NewTranslator", "New Translator User");
            enTexts.Add("Name", "Name");
            enTexts.Add("save", "Save");
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
            enTexts.Add("description", "Description");
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
            enTexts.Add("Language", "Language");
            enTexts.Add("translation", "Translation");
            enTexts.Add("Words", "Words");
            enTexts.Add("MyWords", "My Words");
            enTexts.Add("NewWord", "New Word");
            enTexts.Add("Settings", "Settings");
            enTexts.Add("Logout", "Logout");
            enTexts.Add("updating_key_view_title", "Updating Key");
            enTexts.Add("save_and_close", "Save & Close");
            

            var trTexts = new Dictionary<string, string>();

            trTexts.Add("app_name", "Set Locale");


            #region Login TR

            trTexts.Add("login_view_title", "Sisteme Giriş");
            trTexts.Add("btn_login", "Giriş");
            trTexts.Add("email", "Eposta Adresiniz");
            trTexts.Add("password", "Şifreniz");

            #endregion

            trTexts.Add("Edit", "Düzenle");
            trTexts.Add("Delete", "Sil");
            trTexts.Add("Login", "Giriş");
            trTexts.Add("Email", "E-posta");
            trTexts.Add("Password", "Şifre");
            trTexts.Add("Reset", "Şifre Sıfırlama Linkini Gönder");
            trTexts.Add("ResetTitle", "Şifre Sıfırla");
            trTexts.Add("NewTranslator", "Yeni Çevirmen");
            trTexts.Add("Name", "İsim");
            trTexts.Add("save", "Kaydet");
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
            trTexts.Add("description", "Açıklama");
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
            trTexts.Add("Language", "Dil");
            trTexts.Add("translation", "Çeviri");
            trTexts.Add("Words", "Kelimeler");
            trTexts.Add("MyWords", "Kelimelerim");
            trTexts.Add("NewWord", "Yeni Kelime");
            trTexts.Add("Settings", "Ayarlar");
            trTexts.Add("Logout", "Çıkış");
            trTexts.Add("updating_key_view_title", "Kelime Çevirisi Güncelleme Ekranı");
            trTexts.Add("save_and_close", "Kaydet & Kapat");

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