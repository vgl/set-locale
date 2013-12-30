using System;
using System.Data.Entity;

using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Helpers;

namespace SetLocale.Client.Web.Repositories
{
    public class SetLocaleDbInitializer : CreateDatabaseIfNotExists<SetLocaleDbContext>
    {
        protected override void Seed(SetLocaleDbContext context)
        {
            AddAdmin(context, "Serdar Büyüktemiz", "hserdarb@gmail.com");

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
                LastLoginAt = DateTime.Now
            };
            context.Users.Add(user);
        }
    }
}