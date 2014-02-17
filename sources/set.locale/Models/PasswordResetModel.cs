using set.locale.Helpers;

namespace set.locale.Models
{
    public class PasswordResetModel : BaseModel
    {
        public string Email { get; set; }

        public bool IsValid()
        {
            return Email.IsEmail();
        }

        public bool IsNotValid()
        {
            return !IsValid();
        }
    }
}