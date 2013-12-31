using System;
using System.Threading.Tasks;

using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Helpers;
using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Repositories;

namespace SetLocale.Client.Web.Services
{
    public interface IUserService
    {
        Task<int?> Create(UserModel model, int roleId = 3);
        Task<User> GetByEmail(string email);
        Task<bool> Authenticate(string email, string password);
    }

    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepo;
        public UserService(IRepository<User> userRepo)
        {
            _userRepo = userRepo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="roleId">default is 3 - SetLocaleRole.Developer.Value </param>
        /// <returns></returns>
        public async Task<int?> Create(UserModel model, int roleId = 3)
        {
            var img = GravatarHelper.GetGravatarURL(model.Email, 55, "mm");
            var user = new User
            {
                Email = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString(), BCrypt.Net.BCrypt.GenerateSalt(12)),
                ImageUrl = img,
                RoleId = roleId,
                RoleName = SetLocaleRole.GetString(roleId)
            };
            _userRepo.Create(user);

            if (!_userRepo.SaveChanges()) return null;

            return await Task.FromResult(user.Id);
        }

        public Task<User> GetByEmail(string email)
        {
            if (!email.IsEmail())
            {
                return null;
            }

            var user = _userRepo.FindOne(x => x.Email == email);
            return Task.FromResult(user);
        }

        public Task<bool> Authenticate(string email, string password)
        {
            var user = _userRepo.FindOne(x => x.Email == email && x.PasswordHash != null);
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

            _userRepo.Update(user);
            _userRepo.SaveChanges();

            return Task.FromResult(result);
        }
    }
}