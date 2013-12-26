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

            enTexts.Add("menu_words", "Words");
            enTexts.Add("menu_words_words", "Words");
            enTexts.Add("menu_words_my_words", "My Words");
            enTexts.Add("menu_words_new_word", "New Word");
            enTexts.Add("menu_words_not_translated", "NotTranslated");

            enTexts.Add("menu_apps", "Applications");
            enTexts.Add("menu_apps_apps", "Applications");
            enTexts.Add("menu_apps_new_app", "New App");

            enTexts.Add("menu_settings", "Settings");
            enTexts.Add("menu_settings_apps", "Applications Settings");
            enTexts.Add("menu_settings_users", "User Settings");
            enTexts.Add("menu_settings_new_translator", "New Translator User");

            enTexts.Add("menu_user_login", "Login");
            enTexts.Add("menu_user_logout", "Logout");
            enTexts.Add("menu_user_sign_up", "Sign Up");
            enTexts.Add("menu_user_reset", "Reset Password");

            enTexts.Add("menu_search", "Search");
            #region USER

            #region USER_Login

            enTexts.Add("login_view_title", "Login to System");
            enTexts.Add("btn_login", "Login");
            enTexts.Add("email", "Email");
            enTexts.Add("password", "Password");

            #endregion

            #region USER_Sign_Up
            enTexts.Add("sign_up_new_user", "New User");
            enTexts.Add("btn_sign_up", "Sign Up");
            enTexts.Add("sign_up_email", "Email");
            enTexts.Add("sign_up_password", "Password");
            enTexts.Add("sign_up_name", "Name");



            #endregion

            #region USER_Reset_Password
            enTexts.Add("user_password_reset_title", "Reset Password");
            enTexts.Add("user_password_reset_email", "Email");
            enTexts.Add("btn_user_password_reset", "Send Reset Password Link");


            #endregion

            #endregion

            #region SETTINGS

            #region SETTINGS_Apps

            enTexts.Add("menu_settings_apps_email", "Email");
            enTexts.Add("menu_settings_apps_app_name", "Application Name");
            enTexts.Add("menu_settings_apps_description", "Description");
            enTexts.Add("menu_settings_apps_url", "Url");
            enTexts.Add("menu_settings_apps_usage_count", "Usage Count");
            enTexts.Add("menu_settings_apps_deactivate", "Deactivate");
            enTexts.Add("menu_settings_apps_activate", "Activate");

            #endregion

            #region SETTINGS_New_Translator_User
            enTexts.Add("menu_settings_new_translator_name", "Name");
            enTexts.Add("menu_settings_new_translator_email", "Email");
            enTexts.Add("btn_menu_settings_new_translator_save", "Save");

            #endregion

            #region SETTINGS_Users
            enTexts.Add("menu_settings_users_name", "Name");
            enTexts.Add("menu_settings_users_email", "Email");
            enTexts.Add("menu_settings_users_role", "Role");
            enTexts.Add("menu_settings_users_deactivate", "Deactivate");
            enTexts.Add("menu_settings_users_activate", "Activate");

            #endregion

            #endregion

            #region APPS

            #region APPS_Apps

            enTexts.Add("btn_menu_apps_apps_create_new_token", "Create New Token");
            enTexts.Add("menu_apps_apps_token", "Token");
            enTexts.Add("menu_apps_apps_creation_date", "Creation Date");
            enTexts.Add("menu_apps_apps_usage_count", "Usage Count");
            enTexts.Add("menu_apps_apps_delete", "Delete");

            #endregion

            #region APPS_New_Apps

            enTexts.Add("btn_menu_apps_new_app_save", "Save");
            enTexts.Add("menu_apps_new_app_app_name", "Application Name");
            enTexts.Add("menu_apps_new_app_url", "Url");
            enTexts.Add("menu_apps_new_app_description", "Description");

            #endregion
            #endregion

            #region WORDS

            #region WORDS_My_Word

            enTexts.Add("menu_words_my_words_key_listing", "Key Listing");
            enTexts.Add("menu_words_my_words_key", "Key");
            enTexts.Add("menu_words_my_words_description", "Description");
            enTexts.Add("menu_words_my_words_tag", "Tag");
            enTexts.Add("menu_words_my_words_translated_lang", "TranslatedLang");
            enTexts.Add("btn_menu_words_my_words_edit", "Edit");


            #endregion

            #region WORDS_Not_Translated
            enTexts.Add("menu_words_not_translated_key_listing", "Key Listing");
            enTexts.Add("menu_words_not_translated_key", "Key");
            enTexts.Add("menu_words_not_translated_description", "Description");
            enTexts.Add("menu_words_not_translated_tag", "Tag");
            enTexts.Add("menu_words_not_translated_translated_lang", "TranslatedLang");
            enTexts.Add("btn_menu_words_not_translated_edit", "Edit");

            #endregion

            #region WORDS_New_Word
            enTexts.Add("menu_words_new_word_new_key", "New Key");
            enTexts.Add("menu_words_new_word_key", "Key");
            enTexts.Add("menu_words_new_word_description", "Description");
            enTexts.Add("menu_words_new_word_tag", "Tag");
            enTexts.Add("btn_menu_words_new_word_save", "Save");

            #endregion

            #endregion

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

            enTexts.Add("home_summary", "set-locale's <strong>{2}</strong> translator provides <strong>{4}</strong> translation for <strong>{3}</strong> keys and <strong>{0}</strong> developer is consuming this service with  <strong>{1}</strong> application");
            enTexts.Add("home_title", "welcome to set-locale");


            var trTexts = new Dictionary<string, string>();

            trTexts.Add("app_name", "Set Locale");

            #region Menü

            trTexts.Add("menu_words", "Kelimeler");
            trTexts.Add("menu_words_words", "Kelimeler");
            trTexts.Add("menu_words_my_words", "Kelimelerim");
            trTexts.Add("menu_words_new_word", "Yeni Kelime");
            trTexts.Add("menu_words_not_translated", "Çevrilmeyen Kelimeler");

            trTexts.Add("menu_apps", "Uygulamalar");
            trTexts.Add("menu_apps_apps", "Uygulamalar");
            trTexts.Add("menu_apps_new_app", "Yeni Uygulama");

            trTexts.Add("menu_settings", "Ayarlar");
            trTexts.Add("menu_settings_apps", "Uygulama Ayarları");
            trTexts.Add("menu_settings_users", "Kullanıcı Ayarları");
            trTexts.Add("menu_settings_new_translator", "Yeni Çevirmen");

            trTexts.Add("menu_user_login", "Giriş");
            trTexts.Add("menu_user_logout", "Çıkış");
            trTexts.Add("menu_user_sign_up", "Kayıt Ol");
            trTexts.Add("menu_user_reset", "Şifre Sıfırla");

            trTexts.Add("menu_search", "Ara");

            #region KULLANICILAR

            #region KULLANICILAR_Giriş

            trTexts.Add("login_view_title", "Sisteme Giriş");
            trTexts.Add("btn_login", "Giriş");
            trTexts.Add("email", "Eposta Adresiniz");
            trTexts.Add("password", "Şifreniz");

            #endregion

            #region KULLANICILAR_Kayıt_Ol
            trTexts.Add("sign_up_new_user", "Kayıt Ol");
            trTexts.Add("btn_sign_up", "Kayıt Ol");
            trTexts.Add("sign_up_email", "E-Posta");
            trTexts.Add("sign_up_password", "Şifreniz");
            trTexts.Add("sign_up_name", "İsminiz");

            #endregion

            #region KULLANICILAR_Şifre_Sıfırla
            trTexts.Add("user_password_reset_title", "Şifre Sıfırla");
            trTexts.Add("user_password_reset_email", "E-Posta");
            trTexts.Add("btn_user_password_reset", "Şifre Sıfırlama Linki Gönder");


            #endregion

            #endregion

            #region AYARLAR

            #region AYARLAR_Uygulamalar
            trTexts.Add("menu_settings_apps_email", "E-Posta");
            trTexts.Add("menu_settings_apps_app_name", "Uygulama İsmi");
            trTexts.Add("menu_settings_apps_description", "Açıklama");
            trTexts.Add("menu_settings_apps_url", "Url");
            trTexts.Add("menu_settings_apps_usage_count", "Kullanım Sayısı");
            trTexts.Add("menu_settings_apps_deactivate", "Pasif");
            trTexts.Add("menu_settings_apps_activate", "Aktif");
            #endregion

            #region AYARLAR_Yeni_Çevirmen
            trTexts.Add("menu_settings_new_translator_name", "İsmi");
            trTexts.Add("menu_settings_new_translator_email", "E-Posta");
            trTexts.Add("btn_menu_settings_new_translator_save", "Kaydet");

            #endregion

            #region AYARLAR_Kullanıcı
            trTexts.Add("menu_settings_users_name", "İsim");
            trTexts.Add("menu_settings_users_email", "E-Posta");
            trTexts.Add("menu_settings_users_role", "Yetki Grubu");
            trTexts.Add("menu_settings_users_deactivate", "Pasif");
            trTexts.Add("menu_settings_users_activate", "Aktif");

            #endregion


            #endregion

            #region UYGULAMALAR

            #region UYGULAMALAR_Uygulamalar

            trTexts.Add("btn_menu_apps_apps_create_new_token", "Yeni Token Oluştur");
            trTexts.Add("menu_apps_apps_token", "Token");
            trTexts.Add("menu_apps_apps_creation_date", "Oluşturma Tarihi");
            trTexts.Add("menu_apps_apps_usage_count", "Kullanım Sayısı");
            trTexts.Add("menu_apps_apps_delete", "Sil");

            #endregion

            #region UYGULAMALAR_Yeni_Uygulama

            trTexts.Add("btn_menu_apps_new_app_save", "Kaydet");
            trTexts.Add("menu_apps_new_app_app_name", "Uygulama İsmi");
            trTexts.Add("menu_apps_new_app_url", "Url");
            trTexts.Add("menu_apps_new_app_description", "Açıklama");

            #endregion

            #endregion

            #region KELİMELER

            #region KELİMELER_Kelimelerim

            trTexts.Add("menu_words_my_words_key_listing", "Anahtar Listesi");
            trTexts.Add("menu_words_my_words_key", "Anahtar");
            trTexts.Add("menu_words_my_words_description", "Açıklama");
            trTexts.Add("menu_words_my_words_tag", "Etiket");
            trTexts.Add("menu_words_my_words_translated_lang", "Çevrilmiş Dil");
            trTexts.Add("btn_menu_words_my_words_edit", "Düzenle");


            #endregion

            #region KELİMELER_Çevrilmeyen_Kelimeler

            trTexts.Add("menu_words_not_translated_key_listing", "Anahtar Listesi");
            trTexts.Add("menu_words_not_translated_key", "Anahtar");
            trTexts.Add("menu_words_not_translated_description", "Açıklama");
            trTexts.Add("menu_words_not_translated_tag", "Etiket");
            trTexts.Add("menu_words_not_translated_translated_lang", "Çevrilmiş Dil");
            trTexts.Add("btn_menu_words_not_translated_edit", "Düzenle");

            #endregion

            #region KELİMELER_Yeni_Kelime
            trTexts.Add("menu_words_new_word_new_key", "Yeni Anahtar");
            trTexts.Add("menu_words_new_word_key", "Anahtar");
            trTexts.Add("menu_words_new_word_description", "Açıklama");
            trTexts.Add("menu_words_new_word_tag", "Etiket");
            trTexts.Add("btn_menu_words_new_word_save", "Kaydet");

            #endregion

            #endregion


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

            trTexts.Add("home_summary", "set-locale <strong>{0}</strong> geliştirici tarafından eklenen <strong>{1}</strong> uygulamaya <strong>{2}</strong> çevirmen ile <strong>{3}</strong> farklı kelime için <strong>{4}</strong> adet çeviri sunmaktadır");
            trTexts.Add("home_title","set-locale servisine hoş geldiniz");

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