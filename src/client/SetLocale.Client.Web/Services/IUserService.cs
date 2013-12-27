using SetLocale.Client.Web.Models;

namespace SetLocale.Client.Web.Services
{
    public interface IUserService
    {
        int? Create(UserModel model);
    }

    public class UserService : IUserService
    {
        public int? Create(UserModel model)
        {
            return null;
        }
    }
}