using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Helpers;

namespace SetLocale.Client.Web.Repositories
{
    public class SetLocaleDbMigrationConfiguration : DbMigrationsConfiguration<SetLocaleDbContext>
    {
        public SetLocaleDbMigrationConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(SetLocaleDbContext context)
        {
            if (context.Users.Any()) return;

            #region Users
            AddAdmin(context, "Serdar Büyüktemiz", "hserdarb@gmail.com");
            AddAdmin(context, "Caner Çavuş", "canercvs@gmail.com");
            AddAdmin(context, "Ramiz Sümer", "ramiz.sumerr@gmail.com");
            AddAdmin(context, "Mehmet Sabancıoğlu", "mehmet.sabancioglu@gmail.com");
            AddAdmin(context, "Cihan Çoşkun", "cihancoskun@gmail.com");
            AddAdmin(context, "Kemal Çolak", "kml.colak@gmail.com");
            #endregion

            #region Apps
            AddApplication(context, "setlocale@test.com", "set-locale", "a localization management application.", "setlocale.com");
            AddApplication(context, "setcrm@test.com", "set-crm", "a brand new crm application.", "setcrm.com");
            AddApplication(context, "setmembership@test.com", "set-membership", "a membership management application.", "setmembership.com");
            AddApplication(context, "drone@test.com", "Marmara Drone", "a wireless control dashboard for unmanned air vehicle.", "github.com/jupre/marmaradrone");
            AddApplication(context, "collade@test.com", "Collade", "a task management and team collaboration application.", "collade.com");
            #endregion

            #region Menu

            AddWord(context, "app_name", "Uygulama ismi", "Set Locale", "Set Locale", "set-locale");

            #region Main
            AddWord(context, "search", "genel arama textboxı için", "Ara", "Search", "set-locale");

            AddWord(context, "menu_words", "Kelimeler Menüsü için.", "Kelimeler", "Words", "set-locale");
            AddWord(context, "menu_words_new_word", string.Empty, "Yeni Kelime", "New Word", "set-locale");
            AddWord(context, "menu_words_words", string.Empty, "Tüm Kelimeler", "All Words", "set-locale");
            AddWord(context, "menu_words_my_words", string.Empty, "Kelimelerimasd", "My Words", "set-locale");
            AddWord(context, "menu_words_not_translated", string.Empty, "Çevrilmeyen Kelimeler", "Not Translated", "set-locale");

            AddWord(context, "menu_apps", "Uygulamalar Menüsü için.", "Uygulamalar", "Applications", "set-locale");
            AddWord(context, "menu_apps_new_app", string.Empty, "Yeni Uygulama", "New Application", "set-locale");
            AddWord(context, "menu_apps_my_apps", string.Empty, "Uygulamalarım", "My Applications", "set-locale");

            AddWord(context, "menu_admin", "Admin İşlemleri Menüsü İçin.", "Yönetim", "Admin", "set-locale");
            AddWord(context, "menu_admin_new_translator", string.Empty, "Yeni Çevirmen", "New Translator", "set-locale");
            AddWord(context, "menu_admin_apps", string.Empty, "Tüm Uygulamalar", "All Applications", "set-locale");
            AddWord(context, "menu_admin_users", string.Empty, "Tüm Kullanıcılar", "All Users", "set-locale");

            AddWord(context, "menu_user_login", string.Empty, "Giriş", "Login", "set-locale");
            AddWord(context, "menu_user_logout", string.Empty, "Çıkış", "Logout", "set-locale");
            AddWord(context, "menu_user_sign_up", string.Empty, "Kayıt Ol", "Signup", "set-locale");
            AddWord(context, "menu_user_reset", string.Empty, "Şifre Sıfırla", "Reset Password", "set-locale");

            #endregion

            #region USER

            #region USER_Login

            AddWord(context, "login_view_title", "Kullanıcı giriş menüsü için.", "Sisteme Giriş", "Login to System", "set-locale");
            AddWord(context, "btn_login", string.Empty, "Giriş", "Login", "set-locale");
            AddWord(context, "email", string.Empty, "E-Posta", "Email", "set-locale");
            AddWord(context, "password", string.Empty, "Şifre", "Password", "set-locale");

            #endregion

            #region USER_Sign_Up
            AddWord(context, "sign_up_new_user", "Yeni Kullanıcı menüsü için.", "Yeni Kullanıcı", "New User", "set-locale");
            AddWord(context, "btn_sign_up", string.Empty, "Kayıt Ol", "Sign Up", "set-locale");
            AddWord(context, "sign_up_email", string.Empty, "E-Posta", "Email", "set-locale");
            AddWord(context, "sign_up_password", string.Empty, "Şifre", "Password", "set-locale");
            AddWord(context, "sign_up_name", string.Empty, "Ad Soyad", "Name", "set-locale");



            #endregion

            #region USER_Reset_Password
            AddWord(context, "user_password_reset_title", "Şifre sıfırlama menüsü için.", "Şifre Sıfırlama", "Reset Password", "set-locale");
            AddWord(context, "user_password_reset_email", string.Empty, "E-Posta", "Email", "set-locale");
            AddWord(context, "btn_user_password_reset", string.Empty, "Şifre Sıfırlama Linki Gönder", "Send Reset Password Link", "set-locale");


            #endregion

            #region USER_APPS
            AddWord(context, "user_apps", "Kullanıcıya ait Uygulamalar menüsü", "Uygulamalarım", "My Applications", "set-locale");
            AddWord(context, "user_apps_name", string.Empty, "Uygulama İsmi", "Application Name", "set-locale");
            AddWord(context, "user_apps_description", string.Empty, "Açıklama", "Description", "set-locale");
            AddWord(context, "user_apps_usage_count", string.Empty, "Kullanım Sayısı", "Usage Count", "set-locale");
            AddWord(context, "user_apps_url", string.Empty, "Url", "Url", "set-locale");
            AddWord(context, "user_apps_deactivate", string.Empty, "Pasif", "Deactivate", "set-locale");
            AddWord(context, "user_apps_activate", string.Empty, "Aktif", "Activate", "set-locale");
            #endregion

            #endregion

            #region ADMIN

            #region Admin_Apps

            AddWord(context, "menu_settings_apps_email", string.Empty, "E-Posta", "Email", "set-locale");
            AddWord(context, "menu_settings_apps_app_name", string.Empty, "Uygulama İsmi", "Application Name", "set-locale");
            AddWord(context, "menu_settings_apps_description", string.Empty, "Açıklama", "Description", "set-locale");
            AddWord(context, "menu_settings_apps_url", "Url", string.Empty, "Url", "set-locale");
            AddWord(context, "menu_settings_apps_usage_count", string.Empty, "Kullanım Sayısı", "Usage Count", "set-locale");
            AddWord(context, "menu_settings_apps_deactivate", string.Empty, "Pasif", "Deactivate", "set-locale");
            AddWord(context, "menu_settings_apps_activate", string.Empty, "Aktif", "Activate", "set-locale");

            #endregion

            #region Admin_New_Translator
            AddWord(context, "menu_settings_new_translator_name", string.Empty, "Ad Soyad", "Name", "set-locale");
            AddWord(context, "menu_settings_new_translator_email", string.Empty, "E-Posta", "Email", "set-locale");
            AddWord(context, "btn_menu_settings_new_translator_save", string.Empty, "Kaydet", "Save", "set-locale");
            AddWord(context, "btn_menu_settings_new_translator_edit", string.Empty, "Düzenle", "Edit", "set-locale");

            #endregion

            #region Admin_Users
            AddWord(context, "menu_settings_users_name", string.Empty, "Ad Soyad", "Name", "set-locale");
            AddWord(context, "menu_settings_users_email", string.Empty, "E-Posta", "Email", "set-locale");
            AddWord(context, "menu_settings_users_role", string.Empty, "Yetki Grubu", "RoleName", "set-locale");
            AddWord(context, "menu_settings_users_deactivate", string.Empty, "Pasif", "Deactivate", "set-locale");
            AddWord(context, "menu_settings_users_activate", string.Empty, "Aktif", "Activate", "set-locale");

            #endregion

            #endregion

            #region APPS

            #region APPS_Apps

            AddWord(context, "btn_menu_apps_apps_create_new_token", string.Empty, "Yeni Token Oluştur", "Create New Token", "set-locale");
            AddWord(context, "menu_apps_apps_token", string.Empty, "Token", "Token", "set-locale");
            AddWord(context, "menu_apps_apps_creation_date", string.Empty, "Oluşturma Tarihi", "Creation Date", "set-locale");
            AddWord(context, "menu_apps_apps_usage_count", string.Empty, "Kullanım Sayısı", "Usage Count", "set-locale");
            AddWord(context, "menu_apps_apps_delete", string.Empty, "Sil", "Delete", "set-locale");

            #endregion

            #region APPS_New_Apps

            AddWord(context, "btn_menu_apps_new_app_save", string.Empty, "Kaydet", "Save", "set-locale");
            AddWord(context, "menu_apps_new_app_app_name", string.Empty, "Uyguluama İsmi", "Application Name", "set-locale");
            AddWord(context, "menu_apps_new_app_url", string.Empty, "Url", "Url", "set-locale");
            AddWord(context, "menu_apps_new_app_description", string.Empty, "Açıklama", "Description", "set-locale");

            #endregion
            #endregion

            #region WORDS

            #region WORDS_Words

            AddWord(context, "menu_words_words_key_listing", string.Empty, "Kelime Listesi", "Key Listing", "set-locale");
            AddWord(context, "menu_words_words_key", string.Empty, "Kelime", "Key", "set-locale");
            AddWord(context, "menu_words_words_description", string.Empty, "Açıklama", "Description", "set-locale");
            AddWord(context, "menu_words_words_tag", string.Empty, "Etiket", "Tag", "set-locale");
            AddWord(context, "menu_words_words_translated_lang", string.Empty, "Çevrilmiş Dil", "TranslatedLang", "set-locale");
            AddWord(context, "btn_menu_words_words_edit", string.Empty, "Düzenle", "Edit", "set-locale");
            AddWord(context, "btn_words_new_word", string.Empty, "Yeni Çeviri Ekle", "Add New Translate", "set-locale");
            #endregion

            #region WORDS_My_Word

            AddWord(context, "menu_words_my_words_key_listing", string.Empty, "Kelime Listesi", "Key Listing", "set-locale");
            AddWord(context, "menu_words_my_words_key", string.Empty, "Kelime", "Key", "set-locale");
            AddWord(context, "menu_words_my_words_description", string.Empty, "Açıklama", "Description", "set-locale");
            AddWord(context, "menu_words_my_words_tag", string.Empty, "Etiket", "Tag", "set-locale");
            AddWord(context, "menu_words_my_words_translated_lang", string.Empty, "Çevrilmiş Dil", "TranslatedLang", "set-locale");
            AddWord(context, "btn_menu_words_my_words_edit", string.Empty, "Düzenle", "Edit", "set-locale");


            #endregion

            #region WORDS_Not_Translated
            AddWord(context, "menu_words_not_translated_key_listing", string.Empty, "Kelime Listesi", "Key Listing", "set-locale");
            AddWord(context, "menu_words_not_translated_key", string.Empty, "Kelimeler", "Key", "set-locale");
            AddWord(context, "menu_words_not_translated_description", string.Empty, "Açıklama", "Description", "set-locale");
            AddWord(context, "menu_words_not_translated_tag", string.Empty, "Etkiet", "Tag", "set-locale");
            AddWord(context, "menu_words_not_translated_translated_lang", string.Empty, "Çevrilmiş Dil", "TranslatedLang", "set-locale");
            AddWord(context, "btn_menu_words_not_translated_edit", string.Empty, "Düzenle", "Edit", "set-locale");

            #endregion

            #region WORDS_New_Word
            AddWord(context, "menu_words_new_word_new_key", string.Empty, "Yeni Kelime", "New Key", "set-locale");
            AddWord(context, "menu_words_new_word_key", string.Empty, "Kelime", "Key", "set-locale");
            AddWord(context, "menu_words_new_word_description", string.Empty, "Açıklama", "Description", "set-locale");
            AddWord(context, "menu_words_new_word_tag", string.Empty, "Etiket", "Tag", "set-locale");
            AddWord(context, "btn_menu_words_new_word_save", string.Empty, "Kaydet", "Save", "set-locale");

            #endregion

            #endregion

            #region TAGS

            AddWord(context, "menu_words_words_tag_key_listing", string.Empty, "Etiket Kelime Listesi", "Tag Key Listing", "set-locale");

            #endregion

            #endregion

            #region Shared
            AddWord(context, "Cancel", string.Empty, "Hayıt", "Cancel", "set-locale");
            AddWord(context, "Ok", string.Empty, "Evet", "Ok", "set-locale");
            AddWord(context, "delete", string.Empty, "Sil", "Delete", "set-locale");
            AddWord(context, "modal_body", string.Empty, "Silmek İstediğinize Emin misiniz ?", "Are you sure  want to delete ?", "set-locale");
            AddWord(context, "modal_title_delete_token", string.Empty, "Token Sil", "Token Delete", "set-locale");
            AddWord(context, "modal_body_users", string.Empty, "Durumu Değiştirmek İstediğinize Emin misiniz ?", "Are you sure  want to change the status ?", "set-locale");
            AddWord(context, "modal_title_users", string.Empty, "Kullanıcı Durumu", "User Status", "set-locale");
            AddWord(context, "modal_title_apps", string.Empty, "Uygulama Durumu", "App Status", "set-locale");
            AddWord(context, "updating_key_view_title", string.Empty, "Kelime Çevirisi Güncelleme Ekranı", "Updating Key", "set-locale");
            AddWord(context, "save_and_close", string.Empty, "Kaydet & Kapat", "Save & Close", "set-locale");
            AddWord(context, "total_page_count", string.Empty, "Toplam Sayfa Sayısı", "Total Number of Pages", "set-locale");

            AddWord(context, "home_summary", string.Empty, "set-locale <strong>{0}</strong> geliştirici tarafından eklenen <strong>{1}</strong> uygulamaya <strong>{2}</strong> çevirmen ile <strong>{3}</strong> farklı kelime için <strong>{4}</strong> adet çeviri sunmaktadır", "set-locale's <strong>{2}</strong> translator provides <strong>{4}</strong> translation for <strong>{3}</strong> keys and <strong>{0}</strong> developer is consuming this service with  <strong>{1}</strong> application", "set-locale");
            AddWord(context, "home_title", string.Empty, "set-locale servisine hoş geldiniz", "welcome to set-locale", "set-locale");
            #endregion

            context.SaveChanges();
        }

        private static void AddAdmin(SetLocaleDbContext context, string name, string email)
        {
            var user = new User
            {
                Email = email,
                Name = name,
                RoleId = SetLocaleRole.Admin.Value,
                RoleName = SetLocaleRole.Admin.ToString(),
                ImageUrl = GravatarHelper.GetGravatarURL(email, 35),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
                LastLoginAt = DateTime.Now,
                IsActive = true
            };
            context.Users.Add(user);
        }

        private static void AddTranslator(SetLocaleDbContext context, string name, string email)
        {
            var user = new User
            {
                Email = email,
                Name = name,
                RoleId = SetLocaleRole.Translator.Value,
                RoleName = SetLocaleRole.Translator.ToString(),
                ImageUrl = GravatarHelper.GetGravatarURL(email, 35),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
                LastLoginAt = DateTime.Now,
                IsActive = true
            };
            context.Users.Add(user);
        }

        private static void AddApplication(SetLocaleDbContext context, string email, string name, string description, string url)
        {
            var app = new App
            {
                UserEmail = email,
                Name = name,
                Description = description,
                Url = url,
                CreatedBy = 1,
                Tokens = new List<Token>
                {
                    new Token
                    {
                        Key = Guid.NewGuid().ToString().Replace("-", string.Empty),
                        UsageCount = 0,
                        CreatedBy = 1
                    }
                },
                IsActive = true
            };

            context.Apps.Add(app);
        }

        private static void AddWord(SetLocaleDbContext context, string key, string description, string tr, string en, string tagName)
        {
            var word = new Word
            {
                Key = key,
                Description = description,
                IsTranslated = true,
                Translation_EN = en,
                Translation_TR = tr,
                CreatedBy = 1,
                TranslationCount = 2,
                Tags = new List<Tag>
                {
                    new Tag
                    {
                        Name = tagName,
                        UrlName = tagName.ToUrlSlug(),
                        CreatedBy = 1
                    }
                }
            };
            context.Words.Add(word);
        }
    }
}