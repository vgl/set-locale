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

        private static void AddApplication(SetLocaleDbContext context, string useremail, string name, string description, string url)
        {
            var app = new App
            {
                UserEmail = useremail,
                Name = name,
                Description = description,
                Url = url,
                Tokens = new List<Token> 
                { new Token()
                    {
                        AppId = 123,
                        App = new App(),
                        Key = Guid.NewGuid().ToString().Replace("-", ""),
                        UsageCount = new Random().Next(3, 5555)
                    },
                 new Token()
                    {
                        AppId = 432,
                        App = new App(),
                        Key = Guid.NewGuid().ToString().Replace("-", ""),
                        UsageCount = new Random().Next(3, 5555)
                    }
                },
                IsActive = true
            };

            context.Apps.Add(app);
        }

        private static void AddWord(SetLocaleDbContext context, string key, string description, string translation_tr, string translation_en)
        {
            var word = new Word
            {
                Key = key,
                Description = description,
                IsTranslated = true,
                Translation_EN = translation_en ,
                Translation_TR = translation_tr
            };
            context.Words.Add(word);
        }

    }
}