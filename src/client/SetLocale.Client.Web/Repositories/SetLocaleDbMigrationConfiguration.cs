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
            AddAdmin(context, "Duygu Sevim", "sevimduygu@gmail.com");

            AddTranslator(context, "Translator", "translator@test.com");

            #endregion

            #region Apps

            AddApplication(context, "setlocale@test.com", "set-locale", "a localization management application.",
                "setlocale.com");
            AddApplication(context, "setcrm@test.com", "set-crm", "a brand new crm application.", "setcrm.com");
            AddApplication(context, "setmembership@test.com", "set-membership", "a membership management application.",
                "setmembership.com");
            AddApplication(context, "drone@test.com", "Marmara Drone",
                "a wireless control dashboard for unmanned air vehicle.", "github.com/jupre/marmaradrone");
            AddApplication(context, "collade@test.com", "Collade",
                "a task management and team collaboration application.", "collade.com");

            #endregion

            #region Words
            AddWord(context, "menu_words", string.Empty, "Kelimeler", "Words", "set-locale");
            AddWord(context, "menu_words_words", string.Empty, "Tüm Kelimeler", "All Words", "set-locale");
            AddWord(context, "menu_words_my_words", string.Empty, "Kelimelerim", "My Words", "set-locale");
            AddWord(context, "menu_words_new_word", string.Empty, "Yeni Kelime", "New Word", "set-locale");
            AddWord(context, "menu_words_not_translated", string.Empty, "Çevrilmeyen Kelime", "Word Not Translated", "set-locale");
            AddWord(context, "menu_apps", string.Empty, "Uygulamalar", "Applications", "set-locale");
            AddWord(context, "menu_apps_apps", string.Empty, "Uygulamalarım", "My Applications", "set-locale");
            AddWord(context, "menu_apps_new_app", string.Empty, "Yeni Uygulama", "New Application", "set-locale");
            AddWord(context, "menu_settings", string.Empty, "Yönetim", "Administration", "set-locale");
            AddWord(context, "menu_settings_apps", string.Empty, "Tüm Uygulamalar", "All Applications", "set-locale");
            AddWord(context, "menu_settings_users", string.Empty, "Tüm Kullanıcılar", "All Users", "set-locale");
            AddWord(context, "menu_settings_new_translator", string.Empty, "Yeni Çevirmen", "New Translator", "set-locale");
            AddWord(context, "menu_user_login", string.Empty, "Giriş", "Login", "set-locale");
            AddWord(context, "menu_user_logout", string.Empty, "Çıkış", "Logout", "set-locale");
            AddWord(context, "menu_user_sign_up", string.Empty, "Kayıt Ol", "Sign Up", "set-locale");
            AddWord(context, "menu_user_reset", string.Empty, "Şifre Sıfırla", "Reset Password", "set-locale");
            AddWord(context, "menu_search", string.Empty, "Ara", "Search", "set-locale");

            AddWord(context, "email", string.Empty, "E-posta", "Email", "set-locale");
            AddWord(context, "name", string.Empty, "İsim", "Name", "set-locale");
            AddWord(context, "app_owner_email", string.Empty, "Uygulama Sahibinin E-Postası", "Application of the E-mail owners", "set-locale");
            AddWord(context, "password", string.Empty, "Şifre", "Password", "set-locale");
            AddWord(context, "app_name", string.Empty, "Uygulama İsmi", "Application Name", "set-locale");
            AddWord(context, "description", string.Empty, "Açıklama", "Description", "set-locale");
            AddWord(context, "usage_count", string.Empty, "Kullanım Sayısı", "Usage Count", "set-locale");
            AddWord(context, "url", string.Empty, "Url", "Url", "set-locale");
            AddWord(context, "token", string.Empty, "Token", "Token", "set-locale");
            AddWord(context, "cretion_date", string.Empty, "Oluşturma Tarihi", "Creation Date", "set-locale");
            AddWord(context, "user_role", string.Empty, "Yetki Grubu", "Role", "set-locale");
            AddWord(context, "word_key", string.Empty, "Anahtar", "Key", "set-locale");
            AddWord(context, "tag", string.Empty, "Etiket", "Tag", "set-locale");
            AddWord(context, "translated_language", string.Empty, "Çevrilmiş Dil", "Translated Language", "set-locale");
            AddWord(context, "translation", string.Empty, "Çeviri", "Translation", "set-locale");
            AddWord(context, "language", string.Empty, "Dil", "Language", "set-locale");
            AddWord(context, "forgot_your_password", string.Empty, "Şifremi Unuttum", "Forgot My Password", "set-locale");
            AddWord(context, "total_page_count", string.Empty, "Toplam Sayfa Sayısı", "Total Page Count", "set-locale");
            AddWord(context, "translator_name", string.Empty, "İsim", "Name", "set-locale");

            AddWord(context, "btn_login", string.Empty, "Giriş", "Login", "set-locale");
            AddWord(context, "btn_sign_up", string.Empty, "Kayıt Ol", "Sign Up", "set-locale");
            AddWord(context, "btn_password_reset", string.Empty, "Şifre Sıfırlama Linki Gönder", "Send Password Reset Link", "set-locale");
            AddWord(context, "btn_create_new_token", string.Empty, "Yeni Token Oluştur", "Create New Token", "set-locale");
            AddWord(context, "btn_new_word", string.Empty, "Yeni Çeviri Ekle", "Add New Translation", "set-locale");
            AddWord(context, "btn_deactivate", string.Empty, "Pasif", "Deactivate", "set-locale");
            AddWord(context, "btn_activate", string.Empty, "Aktif", "Activate", "set-locale");
            AddWord(context, "btn_delete", string.Empty, "Sil", "Delete", "set-locale");
            AddWord(context, "btn_save", string.Empty, "Kaydet", "Save", "set-locale");
            AddWord(context, "btn_edit", string.Empty, "Düzenle", "Edit", "set-locale");
            AddWord(context, "btn_cancel", string.Empty, "İptal", "Cancel", "set-locale");
            AddWord(context, "btn_ok", string.Empty, "Tamam", "Ok", "set-locale");

            AddWord(context, "home_title", string.Empty, "Set-locale Servisine Hoş Geldiniz.", "Welcome To Set-locale.", "set-locale");
            AddWord(context, "words_key_listing_title", string.Empty, "Anahtar Listesi", "Key Listing", "set-locale");
            AddWord(context, "login_view_title", string.Empty, "Sisteme Giriş", "İnto The System", "set-locale");
            AddWord(context, "new_user_title", string.Empty, "Yeni Kullanıcı", "New User", "set-locale");
            AddWord(context, "password_reset_title", string.Empty, "Şifre Sıfırla", "Reset Password", "set-locale");
            AddWord(context, "user_apps_title", string.Empty, "Uygulamalarım", "My Applications", "set-locale");
            AddWord(context, "all_apps_title", string.Empty, "Tüm Uygulamalar", "All Applications", "set-locale");
            AddWord(context, "all_users_title", string.Empty, "Tüm Kullanıcılar", "All Users", "set-locale");
            AddWord(context, "word_new_key_title", string.Empty, "Yeni Anahtar", "New Key", "set-locale");
            AddWord(context, "menu_words_my_words_key_listing_title", string.Empty, "Anahtar Listesi", "My Key Listing", "set-locale");
            AddWord(context, "new_app_title", string.Empty, "Yeni Uygulama", "New Application", "set-locale");
            AddWord(context, "new_translator_title", string.Empty, "Yeni Çevirmen", "New Translator", "set-locale");
            AddWord(context, "tag_keys_title", string.Empty, "Tag Anahtar Listesi", "Tag Keys Listing", "set-locale");

            AddWord(context, "modal_title_apps", string.Empty, "Uygulama Durumu", "Application Status", "set-locale");
            AddWord(context, "modal_body_apps", string.Empty, "Silmek İstediğinize Emin misiniz?", "Are You Sure You Want To Delete?", "set-locale");
            AddWord(context, "modal_title_delete_token", string.Empty, "Token Sil", "Delete Token", "set-locale");
            AddWord(context, "modal_body_delete_token", string.Empty, "Bu Anahtarı Silmek İstediğinize Emin misiniz?", "Are You Sure You Want To Delete This Token?", "set-locale");
            AddWord(context, "modal_title_users", string.Empty, "Kullanıcı Durumu", "User Status", "set-locale");
            AddWord(context, "modal_body_users", string.Empty, "Durumu Değiştirmek İstediğinize Emin misiniz?", "Are You Sure You Want To Change The Status", "set-locale"); 
            #endregion
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
                        CreatedBy = 1,
                        IsAppActive = true
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