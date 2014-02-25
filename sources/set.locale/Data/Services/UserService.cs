using System;
using System.Linq;
using System.Threading.Tasks;

using set.locale.Data.Entities;
using set.locale.Helpers;
using set.locale.Models;
using System.Data.Entity;

namespace set.locale.Data.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IMsgService _msgService;

        public UserService(IMsgService msgService)
        {
            _msgService = msgService;
        }

        public Task<bool> Create(UserModel model, string roleName)
        {
            if (model.IsNotValid()) return Task.FromResult(false);

            var img = model.Email.ToGravatar();
            var user = new User
            {
                Email = model.Email,
                Name = model.Name,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password, 15),
                ImageUrl = img,
                RoleId = ConstHelper.BasicRoles[roleName],
                RoleName = roleName,
                Language = model.Language
            };
            Context.Users.Add(user);
            Context.Entry(user).State = EntityState.Added;
            return Task.FromResult(Context.SaveChanges() > 0);
        }

        public Task<PagedList<User>> GetUsers(int pageNumber)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            var query = Context.Users.Where(x => !x.IsDeleted);

            var count = query.Count();
            var items = query.OrderByDescending(x => x.Id).Skip(ConstHelper.PageSize * (pageNumber - 1)).Take(ConstHelper.PageSize).ToList();

            return Task.FromResult(new PagedList<User>(pageNumber, ConstHelper.PageSize, count, items));
        }

        public Task<User> Get(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return null;

            var user = Context.Users.FirstOrDefault(x => x.Id == userId && !x.IsDeleted);
            return Task.FromResult(user);
        }

        public Task<User> GetByEmail(string email)
        {
            if (!email.IsEmail()) return null;

            var user = Context.Users.FirstOrDefault(x => x.Email == email && !x.IsDeleted);
            return Task.FromResult(user);
        }

        public Task<bool> Authenticate(string email, string password)
        {
            if (!email.IsEmail() || string.IsNullOrEmpty(password)) return Task.FromResult(false);

            var user = Context.Users.FirstOrDefault(x => x.Email == email && x.IsActive && !x.IsDeleted);
            if (user == null) return Task.FromResult(false);

            var result = false;

            if (BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)
                && user.LoginTryCount < 5)
            {
                user.LastLoginAt = DateTime.Now;
                user.LoginTryCount = 0;
                result = true;
            }
            else
            {
                user.LoginTryCount += 1;
            }
            Context.Entry(user).State = EntityState.Modified;
            Context.SaveChanges();

            return Task.FromResult(result);
        }

        public Task<bool> RequestPasswordReset(string email)
        {
            if (!email.IsEmail()) return Task.FromResult(false);

            var user = Context.Users.FirstOrDefault(x => x.IsActive
                                                         && !x.IsDeleted
                                                         && x.Email == email);

            if (user == null) return Task.FromResult(false);

            if (user.PasswordResetRequestedAt != null
               && user.PasswordResetRequestedAt.Value.AddMinutes(-1) > DateTime.Now) return Task.FromResult(false);

            var token = Guid.NewGuid().ToNoDashString();
            user.UpdatedAt = DateTime.Now;
            user.UpdatedBy = user.Id;
            user.PasswordResetToken = token;
            user.PasswordResetRequestedAt = user.UpdatedAt;
            Context.Entry(user).State = EntityState.Modified;

            var saved = Context.SaveChanges() > 0;
            if (saved)
            {
                var subject = "password_reset_email_subject".Localize();
                _msgService.SendEMail(user.Email, subject,
                                      string.Format("password_reset_email_body".Localize(), subject, user.Name, user.Email, token));
            }

            return Task.FromResult(saved);
        }

        public Task<bool> IsPasswordResetRequestValid(string email, string token)
        {
            if (!email.IsEmail()) return Task.FromResult(false);
            var lifeTime = DateTime.Now.AddHours(-1);
            return Task.FromResult(Context.Users.Any(x => x.IsActive
                                                          && !x.IsDeleted
                                                          && x.Email == email
                                                          && x.PasswordResetToken == token
                                                          && x.PasswordResetRequestedAt >= lifeTime));
        }

        public async Task<bool> ChangePassword(string email, string token, string password)
        {
            if (!await IsPasswordResetRequestValid(email, token)) return await Task.FromResult(false);

            var user = Context.Users.FirstOrDefault(x => x.IsActive
                                                         && !x.IsDeleted
                                                         && x.Email == email);

            if (user == null) return await Task.FromResult(false);

            user.UpdatedAt = DateTime.Now;
            user.UpdatedBy = user.Id;
            user.PasswordResetToken = null;
            user.PasswordResetRequestedAt = null;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password, 15);
            user.LoginTryCount = 0;
            Context.Entry(user).State = EntityState.Modified;

            return await Task.FromResult(Context.SaveChanges() > 0);
        }

        public Task<bool> ChangeStatus(string id, bool isActive)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Task.FromResult(false);
            }

            var user = Context.Users.Find(id);
            if (user == null)
            {
                return Task.FromResult(false);
            }

            user.IsActive = !isActive;
            Context.Entry(user).State = EntityState.Modified;

            return Task.FromResult(Context.SaveChanges() > 0);
        }
    }

    public interface IUserService
    {
        Task<bool> Create(UserModel model, string roleName);

        Task<PagedList<User>> GetUsers(int pageNumber);

        Task<User> Get(string userId);
        Task<User> GetByEmail(string email);

        Task<bool> Authenticate(string email, string password);

        Task<bool> RequestPasswordReset(string email);
        Task<bool> IsPasswordResetRequestValid(string email, string token);
        Task<bool> ChangePassword(string email, string token, string password);

        Task<bool> ChangeStatus(string id, bool isActive);
    }
}