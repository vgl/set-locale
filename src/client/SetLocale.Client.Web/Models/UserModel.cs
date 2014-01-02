using System;
using System.Collections.Generic;

using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Helpers;

namespace SetLocale.Client.Web.Models
{
    public class UserModel : BaseModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
        public string Language { get; set; }

        public bool IsValidForNewTranslator()
        {
            return !String.IsNullOrEmpty(Name)
                   && !String.IsNullOrEmpty(Email)
                   && Email.IsEmail();
        }

        public bool IsValidForNewDeveloper()
        {
            return !String.IsNullOrEmpty(Password)
                   && !String.IsNullOrEmpty(Email)
                   && !String.IsNullOrEmpty(Name)
                   && Email.IsEmail();
        }

        public static UserModel MapUserToUserModel(User user)
        {
            var model = new UserModel
            {
                Email = user.Email,
                Id = user.Id,
                Name = user.Name,
                RoleName = user.RoleName,
                Language = user.Language,
                IsActive = user.IsActive,
                RoleId = user.RoleId
            };
            return model;
        }
    }
}