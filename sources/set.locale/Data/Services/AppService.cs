using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;

using set.locale.Data.Entities;
using set.locale.Helpers;
using set.locale.Models;

namespace set.locale.Data.Services
{
    public class AppService : BaseService, IAppService
    {
        public async Task<string> Create(AppModel model)
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
                    new Token { CreatedBy = model.CreatedBy, Key = Guid.NewGuid().ToNoDashString(), UsageCount = 0,IsAppActive = true }
                }
            };

            Context.Apps.Add(app);
            if (Context.SaveChanges() > 0) return null;

            return await Task.FromResult(app.Id);
        }

        public Task<bool> CreateToken(TokenModel model)
        {
            if (!model.IsValid())
            {
                return Task.FromResult(false);
            }

            if (!Context.Apps.Any(x => x.Id == model.AppId))
            {
                return Task.FromResult(false);
            }

            var entity = new Token
            {
                CreatedBy = model.CreatedBy,
                AppId = model.AppId,
                Key = model.Token,
                UsageCount = 0,
                IsAppActive = true
            };
            Context.Tokens.Add(entity);

            if (Context.SaveChanges() > 0) Task.FromResult(true);

            return Task.FromResult(true);
        }

        public Task<PagedList<App>> GetApps(int pageNumber)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            var items = Context.Set<App>();

            long totalCount = items.Count();
            var totalPageCount = (int)Math.Ceiling(totalCount / (double)ConstHelper.PageSize);

            if (pageNumber > totalPageCount)
            {
                pageNumber = 1;
            }

            var model = items.OrderByDescending(x => x.Id).Skip(ConstHelper.PageSize * (pageNumber - 1)).Take(ConstHelper.PageSize);

            return Task.FromResult(new PagedList<App>(pageNumber, ConstHelper.PageSize, totalCount, model.ToList()));
        }

        public Task<PagedList<App>> GetByUserId(string userId, int pageNumber)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            var apps = Context.Set<App>().Where(x => x.CreatedBy == userId).ToList();

            long totalCount = apps.Count();
            var totalPageCount = (int)Math.Ceiling(totalCount / (double)ConstHelper.PageSize);

            if (pageNumber > totalPageCount)
            {
                pageNumber = 1;
            }

            apps = apps.OrderByDescending(x => x.Id).Skip(ConstHelper.PageSize * (pageNumber - 1)).Take(ConstHelper.PageSize).ToList();

            return Task.FromResult(new PagedList<App>(pageNumber, ConstHelper.PageSize, totalCount, apps));
        }

        public Task<App> Get(string appId)
        {
            if (string.IsNullOrEmpty(appId))
            {
                return null;
            }

            var app = Context.Set<App>().FirstOrDefault(x => x.Id == appId && x.Tokens.Any());
            return Task.FromResult(app);
        }

        public Task<bool> ChangeStatus(string appId, bool isActive)
        {
            if (string.IsNullOrEmpty(appId)) return Task.FromResult(false);

            var app = Context.Set<App>().FirstOrDefault(x => x.Id == appId && x.Tokens.Any());
            if (app == null) return Task.FromResult(false);

            foreach (var token in app.Tokens)
            {
                token.IsAppActive = !isActive;
            }

            app.IsActive = !isActive;

            return Task.FromResult(Context.SaveChanges() > 0);
        }

        public Task<bool> DeleteToken(string token, string deletedBy)
        {
            if (string.IsNullOrEmpty(token))
            {
                return Task.FromResult(false);
            }

            if (!Context.Set<Token>().Any(x => x.Key == token))
            {
                return Task.FromResult(false);
            }

            var softDelete = Context.Set<Token>().FirstOrDefault(x => x.Key == token);
            if (softDelete != null)
            {
                softDelete.IsDeleted = true;
                softDelete.DeletedBy = deletedBy;
                return Task.FromResult(Context.SaveChanges() > 0);
            }

            return Task.FromResult(false);
        }

        public Task<bool> IsTokenValid(string token)
        {
            return Task.FromResult(Context.Set<Token>().Any(x => x.Key == token && x.IsAppActive));
        }
    }

    public interface IAppService
    {
        Task<string> Create(AppModel model);
        Task<PagedList<App>> GetApps(int pageNumber);
        Task<PagedList<App>> GetByUserId(string userId, int pageNumber);
        Task<App> Get(string appId);
        Task<bool> CreateToken(TokenModel token);
        Task<bool> ChangeStatus(string appId, bool isActive);
        Task<bool> DeleteToken(string token, string deletedBy);

        Task<bool> IsTokenValid(string token);
    }
}