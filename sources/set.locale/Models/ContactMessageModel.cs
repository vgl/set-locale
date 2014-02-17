using set.locale.Data.Entities;
using set.locale.Helpers;

namespace set.locale.Models
{
    public class ContactMessageModel : BaseModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Message)
                   && Email.IsEmail();
        }

        public static ContactMessageModel Map(ContactMessage message)
        {
            var model = new ContactMessageModel
            {
                Id = message.Id,
                Email = message.Email,
                Message = message.Message,
                Subject = message.Subject
            };
            return model;
        }
    }
}