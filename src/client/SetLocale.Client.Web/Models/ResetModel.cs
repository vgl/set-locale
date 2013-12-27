using SetLocale.Util;

namespace SetLocale.Client.Web.Models
{
    public class ResetModel : BaseModel
    {
        public string Email { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Email)
                   && Email.IsEmail();
        }
    }
}