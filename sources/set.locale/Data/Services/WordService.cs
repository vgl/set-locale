using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Entity;

using set.locale.Data.Entities;
using set.locale.Helpers;
using set.locale.Models;

namespace set.locale.Data.Services
{
    public class WordService : BaseService, IWordService
    {
        private readonly IAppService _appService;

        public WordService(IAppService appService)
        {
            _appService = appService;
        }

        public Task<string> Create(WordModel model)
        {
            if (model.IsNotValid())
            {
                return null;
            }

            var slug = model.Key.ToUrlSlug();

            if (IsDuplicateKey(model))
            {
                return null;
            }

            var items = model.Tag.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            var tags = items.Select(item => new Tag { CreatedBy = model.CreatedBy, Name = item, UrlName = item.ToUrlSlug() }).ToList();

            var word = new Word
            {
                Key = slug,
                Description = model.Description ?? string.Empty,
                IsTranslated = false,
                TranslationCount = 0,
                CreatedBy = model.CreatedBy,
                UpdatedBy = model.CreatedBy,
                Tags = tags,
                AppId = model.AppId
            };

            Context.Words.Add(word);

            if (Context.SaveChanges() > 0)
            {
                return Task.FromResult(word.Id);
            }

            return null;
        }

        public async Task<int> CreateList(List<WordModel> model, string appId, string userId)
        {
            int result = 0;
            var app = await _appService.Get(appId);
            foreach (var word in model)
            {
                word.AppId = appId;
                word.Tag = app.Name;
                word.CreatedBy = userId;

                var task = Create(word);

                if (task == null) continue;

                result++;
                await AddTranslateList(word.Translations, word.Id);
            }
            return await Task.FromResult(result);
        }

        public Task<bool> Delete(WordModel model)
        {
            if (model.IsNotValid())
            {
                return null;
            }

            var item = Context.Words.FirstOrDefault(x => x.Id == model.Id);
            if (item == null) return Task.FromResult(false);

            item.DeletedAt = DateTime.Now;
            item.IsDeleted = true;
            item.DeletedBy = model.CreatedBy;
            Context.Entry(item).State = EntityState.Modified;

            return Task.FromResult(Context.SaveChanges() > 0);
        }

        public Task<int> DeleteByAppId(string appId, string createdBy)
        {
            if (string.IsNullOrWhiteSpace(appId) || string.IsNullOrWhiteSpace(createdBy))
            {
                return null;
            }

            var items = Context.Words.Where(x => x.AppId == appId);
            foreach (var item in items)
            {
                item.DeletedAt = DateTime.Now;
                item.IsDeleted = true;
                item.DeletedBy = createdBy;
                Context.Entry(item).State = EntityState.Modified;
            }
            return Task.FromResult(Context.SaveChanges());
        }

        public Task<string> Update(WordModel model)
        {
            if (model.IsNotValid())
            {
                return null;
            }

            var item = Context.Words.FirstOrDefault(x => x.Id == model.Id);
            if (item == null) return Create(model);

            item.DeletedAt = DateTime.Now;
            item.IsDeleted = true;
            item.DeletedBy = model.CreatedBy;
            Context.Entry(item).State = EntityState.Modified;

            Context.SaveChanges();

            return Create(model);
        }

        public Task<PagedList<Word>> GetByUserId(string userId, int pageNumber)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            var words = Context.Words.Include(x => x.Tags).Where(x => x.CreatedBy == userId && (x.IsActive && !x.IsDeleted)).ToList();

            long totalCount = words.Count();
            var totalPageCount = (int)Math.Ceiling(totalCount / (double)ConstHelper.PageSize);

            if (pageNumber > totalPageCount)
            {
                pageNumber = 1;
            }

            words = words.OrderByDescending(x => x.Id).Skip(ConstHelper.PageSize * (pageNumber - 1)).Take(ConstHelper.PageSize).ToList();

            return Task.FromResult(new PagedList<Word>(pageNumber, ConstHelper.PageSize, totalCount, words));
        }

        public Task<Word> GetByKey(string key, string appId)
        {
            key = key.ToUrlSlug();
            var word = Context.Words.FirstOrDefault(x =>
                                                    x.Key == key
                                                 && x.IsActive && !x.IsDeleted
                                                 && x.AppId == appId);
            return Task.FromResult(word);
        }

        public Task<Word> GetById(string id)
        {
            var word = Context.Words.FirstOrDefault(x => x.Id == id && (x.IsActive && !x.IsDeleted));
            return Task.FromResult(word);
        }

        public Task<PagedList<Word>> GetWords(int pageNumber)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            var items = Context.Words.Where(x => x.IsActive && !x.IsDeleted);

            long totalCount = items.Count();
            var totalPageCount = (int)Math.Ceiling(totalCount / (double)ConstHelper.PageSize);

            if (pageNumber > totalPageCount)
            {
                pageNumber = 1;
            }

            var model = items.OrderByDescending(x => x.Id).Skip(ConstHelper.PageSize * (pageNumber - 1)).Take(ConstHelper.PageSize);

            return Task.FromResult(new PagedList<Word>(pageNumber, ConstHelper.PageSize, totalCount, model));
        }

        public Task<PagedList<Word>> GetWords(string appId, int pageNumber)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            var items = Context.Words.Where(x => x.IsActive && !x.IsDeleted && x.AppId == appId);

            long totalCount = items.Count();
            var totalPageCount = (int)Math.Ceiling(totalCount / (double)ConstHelper.PageSize);

            if (pageNumber > totalPageCount)
            {
                pageNumber = 1;
            }

            var model = items.OrderByDescending(x => x.Id).Skip(ConstHelper.PageSize * (pageNumber - 1)).Take(ConstHelper.PageSize);

            return Task.FromResult(new PagedList<Word>(pageNumber, ConstHelper.PageSize, totalCount, model));
        }

        public Task<PagedList<Word>> GetNotTranslated(int pageNumber = 1)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            var words = Context.Words.Where(x => !x.IsTranslated && (x.IsActive && !x.IsDeleted)).ToList();

            long totalCount = words.Count();
            var totalPageCount = (int)Math.Ceiling(totalCount / (double)ConstHelper.PageSize);

            if (pageNumber > totalPageCount)
            {
                pageNumber = 1;
            }

            words = words.OrderByDescending(x => x.Id).Skip(ConstHelper.PageSize * (pageNumber - 1)).Take(ConstHelper.PageSize).ToList();

            return Task.FromResult(new PagedList<Word>(pageNumber, ConstHelper.PageSize, totalCount, words));
        }

        public Task<bool> AddTranslate(string id, string language, string translation)
        {
            if (string.IsNullOrEmpty(id)
               || string.IsNullOrEmpty(language))
            {
                return Task.FromResult(false);
            }

            var word = Context.Words.FirstOrDefault(x => x.Id == id && (x.IsActive && !x.IsDeleted));
            if (word == null)
            {
                return Task.FromResult(false);
            }

            var type = word.GetType();
            var propInfo = type.GetProperty(string.Format("Translation_{0}", language.ToUpperInvariant()), new Type[0]);

            if (propInfo == null)
            {
                return Task.FromResult(false);
            }

            if (string.IsNullOrEmpty(translation))
            {
                word.TranslationCount--;

                word.IsTranslated = word.TranslationCount > 0;

                propInfo.SetValue(word, null);

            }
            else
            {
                propInfo.SetValue(word, translation);
                word.TranslationCount++;
                word.IsTranslated = true;
            }
            Context.Entry(word).State = EntityState.Modified;

            return Task.FromResult(Context.SaveChanges() > 0);
        }

        public async Task<int> AddTranslateList(List<TranslationModel> model, string id)
        {
            int result = 0;
            foreach (var item in model)
            {
                if (await AddTranslate(id, item.Language.Key, item.Value)) result++;
            }
            return await Task.FromResult(result);
        }

        public Task<bool> Tag(string key, string tagName)
        {
            if (string.IsNullOrEmpty(key)
               || string.IsNullOrEmpty(tagName))
            {
                return Task.FromResult(false);
            }

            var word = Context.Words.FirstOrDefault(x => x.Key == key && (x.IsActive && !x.IsDeleted) && x.Tags.Count(y => y.Name == tagName) == 0);
            if (word == null)
            {
                return Task.FromResult(false);
            }

            var tag = new Tag
            {
                Name = tagName,
                UrlName = tagName.ToUrlSlug(),
            };

            word.Tags = new List<Tag> { tag };
            Context.Entry(word).State = EntityState.Modified;

            return Task.FromResult(Context.SaveChanges() > 0);
        }

        public Task<List<Word>> GetByAppId(string appId)
        {
            var words = Context.Words.Where(x => x.AppId == appId && (x.IsActive && !x.IsDeleted)).ToList();
            return Task.FromResult(words);
        }

        public Task<List<Word>> GetByAppName(string appName)
        {
            var words = Context.Words.Where(x => x.App.Name == appName && (x.IsActive && !x.IsDeleted)).ToList();
            return Task.FromResult(words);
        }
        public Task<List<Word>> GetAll()
        {
            var words = Context.Words.Where(x => x.IsActive && !x.IsDeleted).ToList();
            return Task.FromResult(words);
        }

        public bool IsDuplicateKey(WordModel model)
        {
            model.Key = model.Key.ToUrlSlug();
            return Context.Words.Any(
                        x =>
                                x.Key == model.Key
                            && (x.IsActive && !x.IsDeleted)
                            && (x.AppId == model.AppId || x.Tags.Any(y => y.Name == model.Tag)));
        }
    }

    public interface IWordService
    {
        Task<string> Create(WordModel model);
        Task<int> CreateList(List<WordModel> model, string appId, string userId);
        Task<string> Update(WordModel model);
        Task<bool> Delete(WordModel model);
        Task<int> DeleteByAppId(string appId, string createdBy);
        Task<PagedList<Word>> GetByUserId(string userId, int pageNumber);
        Task<Word> GetByKey(string key, string appId);
        Task<Word> GetById(string id);
        Task<PagedList<Word>> GetWords(int pageNumber);
        Task<PagedList<Word>> GetWords(string appId, int pageNumber);
        Task<PagedList<Word>> GetNotTranslated(int pageNumber);
        Task<bool> AddTranslate(string id, string language, string translation);
        Task<int> AddTranslateList(List<TranslationModel> model, string id);
        Task<bool> Tag(string key, string tag);
        Task<List<Word>> GetByAppId(string appId);
        Task<List<Word>> GetByAppName(string appName);
        Task<List<Word>> GetAll();
        bool IsDuplicateKey(WordModel model);
    }
}