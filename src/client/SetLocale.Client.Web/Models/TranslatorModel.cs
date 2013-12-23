using SetLocale.Util;

namespace SetLocale.Client.Web.Models
{
    public class TranslatorModel : BaseModel
    {
        public string Email { get; set; }
        public string Name { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Name)
                   && !string.IsNullOrEmpty(Email)
                   && Email.IsEmail();
        }
    }
}