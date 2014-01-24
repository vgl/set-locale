using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

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
        Task<PagedList<User>> GetUsers(int pageNumber);
        Task<PagedList<User>> GetAllByRoleId(int roleId, int pageNumber);
        Task<bool> ChangeStatus(int userId, bool isActive);
    }

    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<App> _appRepository;

        public UserService(
            IRepository<User> userRepo,
            IRepository<App> appRepository)
        {
            _userRepo = userRepo;
            _appRepository = appRepository;
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
                Name = model.Name,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                ImageUrl = img,
                RoleId = roleId,
                RoleName = SetLocaleRole.GetString(roleId),
                IsActive = true,
                Language = model.Language
            };
            _userRepo.Create(user);

            if (roleId == SetLocaleRole.Developer.Value)
            {
                var app = new App
                {
                    UserEmail = model.Email,
                    Name = model.Name,
                    Url = "example.com",
                    IsActive = true,
                    Tokens = new List<Token>
                    {
                        new Token {Key = Guid.NewGuid().ToString().Replace("-", string.Empty), UsageCount = 0,IsAppActive = true}
                    }
                };

                _appRepository.Create(app);
                _appRepository.SaveChanges();
            }

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

        public Task<PagedList<User>> GetUsers(int pageNumber)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            var items = _userRepo.FindAll();

            long totalCount = items.Count();
            var totalPageCount = (int)Math.Ceiling(totalCount / (double)ConstHelper.PageSize);

            if (pageNumber > totalPageCount)
            {
                pageNumber = 1;
            }

            items = items.OrderByDescending(x => x.Id).Skip(ConstHelper.PageSize * (pageNumber - 1)).Take(ConstHelper.PageSize);

            return Task.FromResult(new PagedList<User>(pageNumber, ConstHelper.PageSize, totalCount, items.ToList()));
        }

        public Task<PagedList<User>> GetAllByRoleId(int roleId, int pageNumber)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            var items = _userRepo.FindAll(x => x.RoleId == roleId).ToList();

            long totalCount = items.Count();
            var totalPageCount = (int)Math.Ceiling(totalCount / (double)ConstHelper.PageSize);

            if (pageNumber > totalPageCount)
            {
                pageNumber = 1;
            }

            items = items.OrderByDescending(x => x.Id).Skip(ConstHelper.PageSize * (pageNumber - 1)).Take(ConstHelper.PageSize).ToList();

            return Task.FromResult(new PagedList<User>(pageNumber, ConstHelper.PageSize, totalCount, items));
        }

        public Task<bool> ChangeStatus(int userId, bool isActive)
        {
            if (userId < 1)
            {
                return Task.FromResult(false);
            }

            var user = _userRepo.FindOne(x => x.Id == userId);
            if (user == null)
            {
                return Task.FromResult(false);
            }

            user.IsActive = !isActive;
            _userRepo.Update(user);

            return Task.FromResult(_userRepo.SaveChanges());
        }
    }
}