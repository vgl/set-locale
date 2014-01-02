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
  
    }

    public class AppService : IAppService
    {
        private readonly IRepository<App> _AppRepository;
        public AppService(IRepository<App> AppRepository)
        {
            _AppRepository = AppRepository;
        }

        public Task<int> Create(AppModel model)
        {
            if (!model.IsValidForNew())
            {
                return null;
            }

            var app = new App
            {
                Id = model.Id,
                OwnerEmail=model.Email,
                Name=model.Name,
                Description = model.Description ?? string.Empty,
                
            };

            _AppRepository.Create(app);
            _AppRepository.SaveChanges();

            if (app.Id < 1)
            {
                return null;
            }

            return Task.FromResult(app.Id);
        }

        public Task<List<App>> GetAll()
        {
            var apps = _AppRepository.FindAll().ToList();
            return Task.FromResult(apps);
        }
        public Task<App> GetByOwnerEmail(string email)
        {
            if (!email.IsEmail())
            {
                return null;
            }

            var app = _AppRepository.FindOne(x => x.OwnerEmail == email);
            return Task.FromResult(app);
        }
    }
}