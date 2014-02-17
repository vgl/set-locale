using set.locale.Data.Entities;

namespace set.locale.Models
{
    public class FeedbackModel : BaseModel
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public string Email { get; set; }

        public static FeedbackModel Map(Feedback feedback)
        {
            var model = new FeedbackModel
            {
                Email = feedback.Email,
                Id = feedback.Id,
                Message = feedback.Message
            };
            return model;
        }
    }
}