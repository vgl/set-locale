using System.Linq;
using System.Threading.Tasks;

using set.locale.Data.Entities;
using set.locale.Helpers;

namespace set.locale.Data.Services
{
    public class FeedbackService : BaseService, IFeedbackService
    {
        public Task<bool> CreateFeedback(string message, string email)
        {
            if (string.IsNullOrWhiteSpace(message)) return Task.FromResult(false);

            if (string.IsNullOrWhiteSpace(email))
            {
                email = ConstHelper.Anonymous;
            }

            var feedback = new Feedback
            {
                Message = message,
                Email = email
            };

            var user = Context.Set<User>().FirstOrDefault(x => x.Email == email);
            if (user != null)
            {
                feedback.CreatedBy = user.Id;
            }

            Context.Set<Feedback>().Add(feedback);

            return Task.FromResult(Context.SaveChanges() > 0);
        }

        public Task<bool> CreateContactMessage(string subject, string email, string message)
        {
            var contact = new ContactMessage
            {
                Subject = subject,
                Email = email,
                Message = message
            };

            var user = Context.Set<User>().FirstOrDefault(x => x.Email == email);
            if (user != null)
            {
                contact.CreatedBy = user.Id;
            }

            Context.Set<ContactMessage>().Add(contact);

            return Task.FromResult(Context.SaveChanges() > 0);
        }

        public Task<PagedList<Feedback>> GetFeedbacks(int pageNumber)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            var query = Context.Feedbacks.Where(x => !x.IsDeleted);

            var count = query.Count();
            var items = query.OrderByDescending(x => x.Id).Skip(ConstHelper.PageSize * (pageNumber - 1)).Take(ConstHelper.PageSize).ToList();

            return Task.FromResult(new PagedList<Feedback>(pageNumber, ConstHelper.PageSize, count, items));
        }

        public Task<PagedList<ContactMessage>> GetContactMessages(int pageNumber)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            var query = Context.ContactMessages.Where(x => !x.IsDeleted);

            var count = query.Count();
            var items = query.OrderByDescending(x => x.Id).Skip(ConstHelper.PageSize * (pageNumber - 1)).Take(ConstHelper.PageSize).ToList();

            return Task.FromResult(new PagedList<ContactMessage>(pageNumber, ConstHelper.PageSize, count, items));
        }
    }

    public interface IFeedbackService
    {
        Task<bool> CreateFeedback(string message, string email);
        Task<bool> CreateContactMessage(string subject, string email, string message);

        Task<PagedList<Feedback>> GetFeedbacks(int pageNumber);
        Task<PagedList<ContactMessage>> GetContactMessages(int pageNumber);
    }
}