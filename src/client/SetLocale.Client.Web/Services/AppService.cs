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
    public interface IAppService
    {
        Task<int> Create(AppModel model);
        Task<List<App>> GetAll();
        Task<List<App>> GetByUserEmail(string email);
        Task<List<App>> GetByUserId(int userId);
        Task<App> Get(int appId);

        Task<List<App>> GetAll(int id);
    }

    public class AppService : IAppService
    {
        private readonly IRepository<App> _appRepository;
        public AppService(IRepository<App> appRepository)
        {
            _appRepository = appRepository;
        }

        public Task<int> Create(AppModel model)
        {
            if (!model.IsValidForNew())
            {
                return null;
            }

            var app = new App
            {
                UserEmail = model.Email,
                Name = model.Name,
                Url = model.Url,
                CreatedBy = model.CreatedBy,
                Description = model.Description ?? string.Empty,
                Tokens = new List<Token>() { new Token{ CreatedBy = model.CreatedBy, Key = Guid.NewGuid().ToString().Replace("-",""),  UsageCount  = 0} }

            };

            _appRepository.Create(app);
            _appRepository.SaveChanges();

            if (app.Id < 1)
            {
                return null;
            }

            return Task.FromResult(app.Id);
        }

        public Task<List<App>> GetAll()
        {
            var apps = _appRepository.FindAll().ToList();
            return Task.FromResult(apps);
        }

        public Task<List<App>> GetByUserEmail(string email)
        {
            if (!email.IsEmail())
            {
                return null;
            }

            var apps = _appRepository.FindAll(x => x.UserEmail == email).ToList();
            return Task.FromResult(apps);
        }

        public Task<List<App>> GetByUserId(int userId)
        {
            if (userId < 1)
            {
                return null;
            }

            var apps = _appRepository.FindAll(x => x.CreatedBy == userId).ToList();
            return Task.FromResult(apps);
        }

        public Task<App> Get(int appId)
        {
            if (appId < 1)
            {
                return null;
            }

            var apps = _appRepository.FindById(appId);
            return Task.FromResult(apps);
        }
    }
}