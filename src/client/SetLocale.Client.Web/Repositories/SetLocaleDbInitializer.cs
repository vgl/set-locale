using System;
using System.Collections.Generic;
using System.Data.Entity;

using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Helpers;
using SetLocale.Client.Web.Models;

namespace SetLocale.Client.Web.Repositories
{
    public class SetLocaleDbInitializer : DropCreateDatabaseIfModelChanges<SetLocaleDbContext>
    {
        protected override void Seed(SetLocaleDbContext context)
        {
            AddAdmin(context, "Serdar Büyüktemiz", "hserdarb@gmail.com");
            AddAdmin(context, "Caner Çavuş", "canercvs@gmail.com");
            AddAdmin(context, "Ramiz Sümer", "ramiz.sumerr@gmail.com");
            AddAdmin(context, "Mehmet Sabancıoğlu", "mehmet.sabancioglu@gmail.com");
            AddAdmin(context, "Cihan Çoşkun", "cihancoskun@gmail.com");

            AddTranslator(context, "Kemal Çolak", "kml.colak@gmail.com");
            AddTranslator(context, "deneme kullanıcı", "deneme@gmail.com");
             
            AddApplication(context, "setlocale@test.com", "SetLocale", "a localization management application.", "setlocale.com");
            AddApplication(context, "setcrm@test.com", "SetCrm", "a brand new crm application.", "setcrm.com");
            AddApplication(context, "drone@test.com", "Marmara Drone", "a wireless control dashboard for humanless flying planes.", "github.com/jupre/marmaradrone");
            AddApplication(context, "collade@test.com", "Collade", "a task management and team collaboration application.", "marmaradrone.github.io");
           
            AddWord(context, "app_name", "Uyguluma ismi", "Set Locale", "Set Locale", "SetLocale", "set-locale");
            AddWord(context, "menu_words", "Kelimeler Menüsü için.", "Kelimeler", "Words", "SetLocale", "set-locale");
            AddWord(context, "menu_words_words", "Kelimeler Menüsü için.", "Tüm Kelimeler", "All Words", "SetLocale", "set-locale");
            AddWord(context, "menu_words_my_words", "Kelimeler Menüsü için.", "Kelimelerim", "My Words", "SetLocale", "set-locale");
            AddWord(context, "menu_words_new_word", "Kelimeler Menüsü için.", "Yeni Kelime", "New Word", "SetLocale", "set-locale");
            AddWord(context, "menu_words_not_translated", "Kelimeler Menüsü için.", "Çevrilmeyenler", "Not Translated", "SetLocale", "set-locale");

            AddWord(context, "menu_apps", "Uygulamalar Menüsü için.", "Uygulamalar", "Applications", "SetLocale", "set-locale");
            AddWord(context, "menu_apps_apps", "Uygulamalar Menüsü için.", "Uygulamalarım", "My Applications", "SetLocale", "set-locale");
            AddWord(context, "menu_apps_new_app", "Uygulamalar Menüsü için.", "Yeni Uygulama", "New Application", "SetLocale", "set-locale");

            AddWord(context, "menu_settings", "Admin İşlemleri Menüsü İçin.", "Admin İşlemleri", "Administrator", "SetLocale", "set-locale");
            AddWord(context, "menu_settings_apps", "Admin İşlemleri Menüsü İçin.", "Uygulamalar", "All Applications", "SetLocale", "set-locale");
            AddWord(context, "menu_settings_users", "Admin İşlemleri Menüsü İçin.", "Kullanıcılar", "All Users", "SetLocale", "set-locale");
            AddWord(context, "menu_settings_new_translator", "Admin İşlemleri Menüsü İçin.", "Yeni Çevirmen", "New Translator", "SetLocale", "set-locale");

            AddWord(context, "menu_user_login", "Kullanıcı Giriş Menüsü", "Giriş", "Login", "SetLocale", "set-locale");
            AddWord(context, "menu_user_logout", "Kullanıcı Giriş Menüsü", "Çıkış", "Logout", "SetLocale", "set-locale");
            AddWord(context, "menu_user_sign_up", "Kullanıcı Giriş Menüsü", "Kayıt Ol", "Signup", "SetLocale", "set-locale");
            AddWord(context, "menu_user_reset", "Kullanıcı Giriş Menüsü", "Şifre Sıfırla", "Reset Password", "SetLocale", "set-locale");
             

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
                ImageUrl = GravatarHelper.GetGravatarURL(email, 35, "mm"),
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
                ImageUrl = GravatarHelper.GetGravatarURL(email, 35, "mm"),
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
                        Key = Guid.NewGuid().ToString().Replace("-", ""),
                        UsageCount = new Random().Next(3, 5555),
                        CreatedBy = 1
                    }
                },
                IsActive = true
            };

            context.Apps.Add(app);
        }

        private static void AddWord(SetLocaleDbContext context, string key, string description, string tr, string en, string tagName, string tagUrlName)
        {
            var word = new Word
            {
                Key = key,
                Description = description,
                IsTranslated = true,
                Translation_EN = en,
                Translation_TR = tr,
                Tags = new List<Tag>
                {
                    new Tag
                    {
                        Name = tagName,
                        UrlName = tagUrlName,
                        CreatedBy = 1
                    }
                }
            };
            context.Words.Add(word);
        }
    }
}