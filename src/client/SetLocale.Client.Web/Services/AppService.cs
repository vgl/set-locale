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
        Task<PagedList<App>> GetApps(int pageNumber);
        Task<List<App>> GetByUserEmail(string email);
        Task<PagedList<App>> GetByUserId(int userId, int pageNumber);
        Task<App> Get(int appId);
        Task<bool> CreateToken(TokenModel token);
        Task<bool> ChangeStatus(int appId, bool isActive);
        Task<bool> DeleteToken(string token, int deletedBy);
    }

    public class AppService : IAppService
    {
        private readonly IRepository<Token> _tokenRepository;
        private readonly IRepository<App> _appRepository;

        public AppService(
            IRepository<Token> tokenRepository,
            IRepository<App> appRepository)
        {
            _tokenRepository = tokenRepository;
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
                IsActive = true,
                CreatedBy = model.CreatedBy,
                Description = model.Description ?? string.Empty,
                Tokens = new List<Token>
                {
                    new Token { CreatedBy = model.CreatedBy, Key = Guid.NewGuid().ToString().Replace("-", string.Empty), UsageCount = 0 }
                }
            };

            _appRepository.Create(app);
            _appRepository.SaveChanges();

            if (app.Id < 1)
            {
                return null;
            }

            return Task.FromResult(app.Id);
        }

        public Task<PagedList<App>> GetApps(int pageNumber)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            var items = _appRepository.FindAll();

            long totalCount = items.Count();

            items = items.OrderByDescending(x => x.Id).Skip(ConstHelper.PageSize * (pageNumber - 1)).Take(ConstHelper.PageSize);

            return Task.FromResult(new PagedList<App>(pageNumber, ConstHelper.PageSize, totalCount, items.ToList()));
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

        public Task<PagedList<App>> GetByUserId(int userId, int pageNumber)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            if (userId < 1)
            {
                return null;
            }

            var apps = _appRepository.FindAll(x => x.CreatedBy == userId).ToList();

            long totalCount = apps.Count();

            apps = apps.OrderByDescending(x => x.Id).Skip(ConstHelper.PageSize * (pageNumber - 1)).Take(ConstHelper.PageSize).ToList();

            return Task.FromResult(new PagedList<App>(pageNumber, ConstHelper.PageSize, totalCount, apps));
        }

        public Task<App> Get(int appId)
        {
            if (appId < 1)
            {
                return null;
            }

            var app = _appRepository.FindOne(x => x.Id == appId, x => x.Tokens);
            return Task.FromResult(app);
        }

        public Task<bool> CreateToken(TokenModel model)
        {
            if (!model.IsValid())
            {
                return Task.FromResult(false);
            }

            if (!_appRepository.Set<App>().Any(x => x.Id == model.AppId))
            {
                return Task.FromResult(false);
            }

            var entity = new Token
            {
                CreatedBy = model.CreatedBy,
                AppId = model.AppId,
                Key = model.Token,
                UsageCount = 0
            };
            _tokenRepository.Create(entity);
            _tokenRepository.SaveChanges();

            if (entity.Id < 0)
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        public Task<bool> ChangeStatus(int appId, bool isActive)
        {
            if (appId < 1)
            {
                return Task.FromResult(false);
            }

            var app = _appRepository.FindOne(x => x.Id == appId);
            if (app == null)
            {
                return Task.FromResult(false);
            }

            app.IsActive = !isActive;
            _appRepository.Update(app);
            _appRepository.SaveChanges();

            return Task.FromResult(true);
        }

        public Task<bool> DeleteToken(string token, int deletedBy)
        {
            if (string.IsNullOrEmpty(token))
            {
                return Task.FromResult(false);
            }

            if (!_tokenRepository.Set<Token>().Any(x => x.Key == token))
            {
                return Task.FromResult(false);
            }

            _tokenRepository.SoftDelete(x => x.Key == token, deletedBy);
            _tokenRepository.SaveChanges();

            return Task.FromResult(true);
        }
    }
}