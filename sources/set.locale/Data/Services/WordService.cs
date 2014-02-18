using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using set.locale.Data.Entities;
using set.locale.Helpers;
using set.locale.Models;

namespace set.locale.Data.Services
{
    public class WordService : BaseService, IWordService
    {
        public Task<string> Create(WordModel model)
        {
            if (model.IsNotValid())
            {
                return null;
            }

            var slug = model.Key.ToUrlSlug();

            // Farklı uygulamada aynı key ler olabilir diye kaldırıldı.
            //if (Context.Words.Any(x => x.Key == slug))
            //{
            //    return null;
            //}

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
                return Task.FromResult(word.Key);
            }

            return null;
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

            var words = Context.Words.Include("Tags").Where(x => x.CreatedBy == userId).ToList();

            long totalCount = words.Count();
            var totalPageCount = (int)Math.Ceiling(totalCount / (double)ConstHelper.PageSize);

            if (pageNumber > totalPageCount)
            {
                pageNumber = 1;
            }

            words = words.OrderByDescending(x => x.Id).Skip(ConstHelper.PageSize * (pageNumber - 1)).Take(ConstHelper.PageSize).ToList();

            return Task.FromResult(new PagedList<Word>(pageNumber, ConstHelper.PageSize, totalCount, words));
        }

        public Task<Word> GetByKey(string key)
        {
            var word = Context.Words.FirstOrDefault(x => x.Key == key);
            return Task.FromResult(word);
        }

        public Task<PagedList<Word>> GetWords(int pageNumber)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            var items = Context.Words;

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

            var items = Context.Words.Where(x => x.AppId == appId);

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

            var words = Context.Words.Where(x => x.IsTranslated == false).ToList();

            long totalCount = words.Count();
            var totalPageCount = (int)Math.Ceiling(totalCount / (double)ConstHelper.PageSize);

            if (pageNumber > totalPageCount)
            {
                pageNumber = 1;
            }

            words = words.OrderByDescending(x => x.Id).Skip(ConstHelper.PageSize * (pageNumber - 1)).Take(ConstHelper.PageSize).ToList();

            return Task.FromResult(new PagedList<Word>(pageNumber, ConstHelper.PageSize, totalCount, words));
        }

        public Task<bool> Translate(string key, string language, string translation)
        {
            if (string.IsNullOrEmpty(key)
               || string.IsNullOrEmpty(language))
            {
                return Task.FromResult(false);
            }

            var word = Context.Words.FirstOrDefault(x => x.Key == key);
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

            return Task.FromResult(Context.SaveChanges() > 0);
        }

        public Task<bool> Tag(string key, string tagName)
        {
            if (string.IsNullOrEmpty(key)
               || string.IsNullOrEmpty(tagName))
            {
                return Task.FromResult(false);
            }

            var word = Context.Words.FirstOrDefault(x => x.Key == key && x.Tags.Count(y => y.Name == tagName) == 0);
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

            return Task.FromResult(Context.SaveChanges() > 0);
        }

        public Task<List<Word>> GetAll()
        {
            var words = Context.Words.ToList();

            return Task.FromResult(words);
        }

        public Task<string> Update(WordModel model)
        {
            if (model.IsNotValid())
            {
                return null;
            }

            var slug = model.Key.ToUrlSlug();

            var softDelete = Context.Words.FirstOrDefault(x => x.Key == slug);
            if (softDelete == null) return Create(model);


            softDelete.DeletedAt = DateTime.Now;
            softDelete.IsDeleted = true;
            softDelete.DeletedBy = model.CreatedBy;

            Context.SaveChanges();

            return Create(model);
        }
    }

    public interface IWordService
    {
        Task<string> Create(WordModel model);
        Task<string> Update(WordModel model);
        Task<PagedList<Word>> GetByUserId(string userId, int pageNumber);
        Task<Word> GetByKey(string key);
        Task<PagedList<Word>> GetWords(int pageNumber);
        Task<PagedList<Word>> GetNotTranslated(int pageNumber);
        Task<bool> Translate(string key, string language, string translation);
        Task<bool> Tag(string key, string tag);
        Task<List<Word>> GetAll();
    }
}