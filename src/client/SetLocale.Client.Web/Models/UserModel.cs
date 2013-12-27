using SetLocale.Util;

namespace SetLocale.Client.Web.Models
{
    public class UserModel: BaseModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public string Language { get; set; }

        public bool IsValidForAddingNewTranslator()
        {
            return !string.IsNullOrEmpty(Name)
                   && !string.IsNullOrEmpty(Email)
                   && Email.IsEmail();
        }
    }
}