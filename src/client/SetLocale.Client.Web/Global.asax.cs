using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Routing;

using Castle.Windsor;
using Castle.Windsor.Installer;

using SetLocale.Client.Web.Configurations;
using SetLocale.Client.Web.Helpers;

namespace SetLocale.Client.Web
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            MvcHandler.DisableMvcResponseHeader = true;

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            GlobalConfiguration.Configuration.EnsureInitialized();

            PrepareIocContainer();

            PrepareLocalizationStrings();
        }

        private void PrepareLocalizationStrings()
        {
            var enTexts = new Dictionary<string, string>();

            #region EN

            #region Menu

            enTexts.Add("menu_words", "Words");
            enTexts.Add("menu_words_words", "All Words");
            enTexts.Add("menu_words_my_words", "My Words");
            enTexts.Add("menu_words_new_word", "New Word");
            enTexts.Add("menu_words_not_translated", "NotTranslated");

            enTexts.Add("menu_apps", "Applications");
            enTexts.Add("menu_apps_apps", "My Applications");
            enTexts.Add("menu_apps_new_app", "New App");

            enTexts.Add("menu_settings", "Administrator");
            enTexts.Add("menu_settings_apps", "All Applications");
            enTexts.Add("menu_settings_users", "All Users");
            enTexts.Add("menu_settings_new_translator", "New Translator");

            enTexts.Add("menu_user_login", "Login");
            enTexts.Add("menu_user_logout", "Logout");
            enTexts.Add("menu_user_sign_up", "Sign Up");
            enTexts.Add("menu_user_reset", "Reset Password");

            enTexts.Add("menu_search", "Search");

            #endregion

            enTexts.Add("name", "Name");
            enTexts.Add("email", "Email");
            enTexts.Add("app_owner_email", "Owner Email");
            enTexts.Add("password", "Password");
            enTexts.Add("app_name", "Application Name");
            enTexts.Add("description", "Description");
            enTexts.Add("usage_count", "Usage Count");
            enTexts.Add("url", "Url");
            enTexts.Add("token", "Token");
            enTexts.Add("creation_date", "Creation Date");
            enTexts.Add("user_role", "User Role");
            enTexts.Add("word_key", "Key");
            enTexts.Add("tag", "Tag");
            enTexts.Add("translated_language", "Translated Language");
            enTexts.Add("forgot_your_password", "Forgot your password?");
            enTexts.Add("total_page_count", "Total Page Count");
            enTexts.Add("translator_name", "Translator Name");

            enTexts.Add("btn_login", "Login");
            enTexts.Add("btn_sign_up", "Sign Up");
            enTexts.Add("btn_password_reset", "Send Reset Password Link");
            enTexts.Add("btn_create_new_token", "Create New Token");
            enTexts.Add("btn_new_word", "Add New Translation");
            enTexts.Add("btn_deactivate", "Deactivate");
            enTexts.Add("btn_activate", "Activate");
            enTexts.Add("btn_delete", "Delete");
            enTexts.Add("btn_save", "Save");
            enTexts.Add("btn_edit", "Edit");
            enTexts.Add("btn_cancel", "Cancel");
            enTexts.Add("btn_ok", "Ok");

            enTexts.Add("home_summary", "SetLocale's <strong>{2}</strong> translator provided <strong>{4}</strong> translation for <strong><a href='/word/all' style='text-decoration:underline;color:red;'>{3}</a></strong> keys and <strong>{0}</strong> developer is consuming this service with <strong>{1}</strong> application");

            enTexts.Add("home_title", "Localization as a service");
            enTexts.Add("words_key_listing_title", "Key Listing");
            enTexts.Add("login_view_title", "Login to System");
            enTexts.Add("new_user_title", "New User");
            enTexts.Add("password_reset_title", "Reset Password");
            enTexts.Add("user_apps_title", "My Applications");
            enTexts.Add("all_apps_title", "All Applications");
            enTexts.Add("all_users_title", "All Users");
            enTexts.Add("word_new_key_title", "New Key");
            enTexts.Add("words_my_key_listing_title", "My Keys Listing");
            enTexts.Add("words_not_translated_key_listing_title", "Not Translated Keys Listing");
            enTexts.Add("new_app_title", "New Application");
            enTexts.Add("new_translator_title", "New Translator");
            enTexts.Add("tag_keys_title", "Keys Listing Tagged With ");

            enTexts.Add("modal_title_apps", "App Status");
            enTexts.Add("modal_body_apps", "Are you sure want to change the user's status?");
            enTexts.Add("modal_title_delete_token", "Token Delete");
            enTexts.Add("modal_body_delete_token", "Are you sure want to delete the token?");
            enTexts.Add("modal_title_users", "User Status");
            enTexts.Add("modal_body_users", "Are you sure want to change the application's status?");

            #endregion

            var trTexts = new Dictionary<string, string>();
            #region TR
            trTexts.Add("app_name", "Set Locale");

            #region Menü

            trTexts.Add("menu_words", "Kelimeler");
            trTexts.Add("menu_words_words", "Tüm Kelimeler");
            trTexts.Add("menu_words_my_words", "Kelimelerim");
            trTexts.Add("menu_words_new_word", "Yeni Kelime");
            trTexts.Add("menu_words_not_translated", "Çevrilmeyen Kelimeler");

            trTexts.Add("menu_apps", "Uygulamalar");
            trTexts.Add("menu_apps_apps", "Uygulamalarım");
            trTexts.Add("menu_apps_new_app", "Yeni Uygulama");

            trTexts.Add("menu_settings", "Yönetim");
            trTexts.Add("menu_settings_apps", "Tüm Uygulamalar");
            trTexts.Add("menu_settings_users", "Tüm Kullanıcılar");
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

            #region KULLANICILAR_Uygulama
            trTexts.Add("user_apps", "Uygulamalarım");
            trTexts.Add("user_apps_name", "Uygulama İsmi");
            trTexts.Add("user_apps_description", "Açıklama");
            trTexts.Add("user_apps_usage_count", "Kullanım Sayısı");
            trTexts.Add("user_apps_url", "Url");
            trTexts.Add("user_apps_deactivate", "Pasif");
            trTexts.Add("user_apps_activate", "Aktif");
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
            trTexts.Add("btn_menu_settings_new_translator_edit", "Düzenle");

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

            #region KELİMELER_Kelimeler

            trTexts.Add("menu_words_words_key_listing", "Anahtar Listesi");
            trTexts.Add("menu_words_words_key", "Anahtar");
            trTexts.Add("menu_words_words_description", "Açıklama");
            trTexts.Add("menu_words_words_tag", "Etiket");
            trTexts.Add("menu_words_words_translated_lang", "Çevrilmiş Dil");
            trTexts.Add("btn_menu_words_words_edit", "Düzenle");
            trTexts.Add("btn_words_new_word", "Yeni Çeviri Ekle");


            #endregion

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

            #region TAGLAR

            trTexts.Add("menu_words_words_tag_key_listing", "Tag Kelime Listesi");

            #endregion

            #region ŞİFRERESETLE

            trTexts.Add("forgot_your_password", "Şifremi unuttum");

            #endregion

            #endregion


            #region Paylaşılan
            trTexts.Add("Cancel", "Hayır");
            trTexts.Add("Ok", "Evet");
            trTexts.Add("delete", "Sil");
            trTexts.Add("modal_body", "Silmek İstediğinize Eminmisiniz ?");
            trTexts.Add("modal_title_delete_token", "Token Sil");
            trTexts.Add("modal_body_users", "Durumu Değiştirmek İstediğinize Eminmisiniz ?");
            trTexts.Add("modal_title_users", "Kullanıcı Durumu");
            trTexts.Add("modal_title_apps", "Uygulama Durumu");
            trTexts.Add("updating_key_view_title", "Kelime Çevirisi Güncelleme Ekranı");
            trTexts.Add("save_and_close", "Kaydet & Kapat");

            trTexts.Add("home_summary", "set-locale <strong>{0}</strong> geliştirici tarafından eklenen <strong>{1}</strong> uygulamaya <strong>{2}</strong> çevirmen ile <strong>{3}</strong> farklı kelime için <strong>{4}</strong> adet çeviri sunmaktadır");
            trTexts.Add("home_title", "set-locale servisine hoş geldiniz");
            #endregion
            #endregion

            Application.Add(ConstHelper.en_txt, enTexts);
            Application.Add(ConstHelper.tr_txt, trTexts);
        }

        private static void PrepareIocContainer()
        {
            var container = new WindsorContainer().Install(FromAssembly.This());
            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container.Kernel));
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator), new WindsorAPIControllerFactory(container));
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