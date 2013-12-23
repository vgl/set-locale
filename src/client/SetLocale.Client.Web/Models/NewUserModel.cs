using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SetLocale.Util;

namespace SetLocale.Client.Web.Models
{
    public class NewUserModel : BaseModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Password)
                   && !string.IsNullOrEmpty(Email)
                   && !string.IsNullOrEmpty(UserName)
                   && Email.IsEmail();
        }
    }
}