using SetLocale.Util;

namespace SetLocale.Client.Web.Models
{
    public class NewUserModel : BaseModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Password)
                   && !string.IsNullOrEmpty(Email)
                   && !string.IsNullOrEmpty(Name)
                   && Email.IsEmail();
        }
    }
}