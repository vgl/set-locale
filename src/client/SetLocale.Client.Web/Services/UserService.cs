using System.Threading.Tasks;
using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Helpers;
using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Repositories;

namespace SetLocale.Client.Web.Services
{
    public interface IUserService
    {
        int? Create(UserModel model);

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

        public Task<User> GetByEmail(string email)
        {
            if (!email.IsEmail())
            {
                return null;
            }

            var user = _userRepo.FindOne(x => x.Email == email);
            if (user == null)
            {
                return null;
            }

            return Task.FromResult(user);
        }

        public Task<bool> Authenticate(string email, string password)
        {
            throw new System.NotImplementedException();
        }
    }
}