using System;
using System.Data.Entity.Migrations;
using System.Linq;

using set.locale.Data.Entities;
using set.locale.Helpers;

namespace set.locale.Data
{
    public class SetDbMigrationConfiguration : DbMigrationsConfiguration<SetDbContext>
    {
        public SetDbMigrationConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(SetDbContext context)
        {
            if (context.Users.Any()) return;

            #region Users

            AddUser(context, ConstHelper.Admin, "admin@test.com", ConstHelper.Admin);
            AddUser(context, ConstHelper.User, "user@test.com", ConstHelper.User);

            #endregion

            context.SaveChanges();
        }

        private static void AddUser(SetDbContext context, string name, string email, string role)
        {
            var user = new User
            {
                Email = email,
                Name = name,
                RoleId = ConstHelper.BasicRoles[role],
                RoleName = role,
                ImageUrl = email.ToGravatar(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
                LastLoginAt = DateTime.Now,
                IsActive = true,
                Language = ConstHelper.CultureNameEN
            };
            context.Users.Add(user);
        }
    }
}