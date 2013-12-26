using System.Collections.Generic;
using SetLocale.Client.Web.Models;
using SetLocale.Util;

namespace SetLocale.Client.Web.Services
{
    public interface IDemoDataService
    {
        UserModel GetAUser();
        List<AppModel> GetUsersApps();
        List<AppModel> GetAllApps();

    }
    public class DemoDataService : IDemoDataService
    {
        public UserModel GetAUser()
        {
            return new UserModel
             {
                 Language = ConstHelper.en,
                 Id = 1,
                 IsActive = true,
                 Email = "hserdarb@gmail.com",
                 Name = "Serdar Büyüktemiz",
                 Role = ConstHelper.User
             };
        }

        public List<AppModel> GetUsersApps()
        {
            var result = new List<AppModel>();
            result.Add(new AppModel
            {
                Id = 1,
                AppName = "SetLocale",
                AppDescription = "a localization management application.",
                Url = "setlocale.com",
                UsageCount = 1356,
                IsActive = true
            });
            result.Add(new AppModel
            {
                Id = 2,
                AppName = "SetCrm",
                AppDescription = "a brand new crm application.",
                Url = "setcrm.com",
                UsageCount = 64212,
                IsActive = true
            });

            return result;
        }

        public List<AppModel> GetAllApps()
        {
            var result = GetUsersApps();
            result.Add(new AppModel
            {
                Id = 2,
                AppName = "Marmara Drone",
                AppDescription = "a wireless control dashboard for humanless flying planes.",
                Url = "marmaradrone.github.io",
                UsageCount = 125493,
                IsActive = true
            });

            result.Add(new AppModel
            {
                Id = 2,
                AppName = "Collade",
                AppDescription = "a task management and team collaboration application.",
                Url = "marmaradrone.github.io",
                UsageCount = 9852,
                IsActive = true
            });

            return result;
        }
    }
}