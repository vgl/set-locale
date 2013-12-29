using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Repositories;

namespace SetLocale.Client.Web.Services
{
    public interface IUserService
    {
        int? Create(UserModel model);
    }

    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepo;

        public UserService(IRepository<User> userRepo)
        {
            _userRepo = userRepo;
        }

        public int? Create(UserModel model)
        {
            var user = new User { Email = model.Email, PasswordHash = model.Password };
            _userRepo.Create(user);

            if (_userRepo.SaveChanges())
            {
                return user.Id;
            }

            return null;
        }
    }
}