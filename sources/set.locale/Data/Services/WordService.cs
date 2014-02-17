using set.locale.Data.Entities;
using set.locale.Helpers;
using set.locale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace set.locale.Data.Services
{
    public class WordService : BaseService, IWordService
    {
        public async Task<string> Create(WordModel model)
        {
            if (!model.IsValid())
            {
                return null;
            }

            var slug = model.Key.ToUrlSlug();
            var tags = new List<Tag>();
            var items = model.Tag.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in items)
            {
                tags.Add(new Tag
                {
                    CreatedBy = model.CreatedBy,
                    Name = item,
                    UrlName = item.ToUrlSlug()
                });
            }

            if (Context.Words.Any(x => x.Key == slug))
            {
                return null;
            }

            var word = new Word
            {
                Key = slug,
                Description = model.Description ?? string.Empty,
                IsTranslated = false,
                TranslationCount = 0,
                CreatedBy = model.CreatedBy,
                UpdatedBy = model.CreatedBy,
                Tags = tags
            };


            Context.Words.Add(word);
            if (Context.SaveChanges() > 0)
            {
                return await Task.FromResult(word.Id);
            }

            return null;
        }
        public Task<Word> GetByKey(string key)
        {
            if (key == string.Empty)
                return null;

            var items = Context.Words;
            var word = items.FirstOrDefault(x => x.Key == key);

            return Task.FromResult(word);
        }
        public Task<PagedList<Word>> GetWords(int pageNumber)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }


            var items = Context.Words.Where(x => !x.IsDeleted);

            long totalCount = items.Count();
            var totalPageCount = (int)Math.Ceiling(totalCount / (double)ConstHelper.PageSize);


            if (pageNumber > totalPageCount)
            {
                pageNumber = 1;
            }


            var words = items.OrderByDescending(x => x.Id).Skip(ConstHelper.PageSize * (pageNumber - 1)).Take(ConstHelper.PageSize);

            return Task.FromResult(new PagedList<Word>(pageNumber, ConstHelper.PageSize, totalCount, words.ToList()));
        }
        public Task<PagedList<Word>> GetNotTranslated(int pageNumber = 1)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }


            var items = Context.Words.Where(x => !x.IsTranslated && !x.IsDeleted);

            long totalCount = items.Count();
            var totalPageCount = (int)Math.Ceiling(totalCount / (double)ConstHelper.PageSize);


            if (pageNumber > totalPageCount)
            {
                pageNumber = 1;
            }


            var notTranslatedwords = items.OrderByDescending(x => x.Id).Skip(ConstHelper.PageSize * (pageNumber - 1)).Take(ConstHelper.PageSize);

            return Task.FromResult(new PagedList<Word>(pageNumber, ConstHelper.PageSize, totalCount, notTranslatedwords.ToList()));
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
                CreatedBy = "1"
            };


            word.Tags = new List<Tag> { tag };

            return Task.FromResult(Context.SaveChanges() > 0);

        }
        public Task<List<Word>> GetAll()
        {
            var words = Context.Set<Word>().ToList();

            return Task.FromResult(words);
        }
        public Task<string> Update(WordModel model)
        {
            if (!model.IsValid())
            {
                return null;
            }

            var slug = model.Key.ToUrlSlug();


            var wordEntity = Context.Set<Word>().FirstOrDefault(x => x.Key == slug);
            if (wordEntity == null) return Create(model);

            wordEntity.IsDeleted = true;
            Context.SaveChanges();

            return Create(model);
        }
        public Task<PagedList<Word>> GetByUserId(string userId, int pageNumber)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            if (userId == string.Empty)
            {
                return null;
            }

            var items = Context.Set<Word>().Where(x => x.CreatedBy == userId && !x.IsDeleted);
            long totalCount = items.Count();
            var totalPageCount = (int)Math.Ceiling(totalCount / (double)ConstHelper.PageSize);

            if (pageNumber > totalPageCount)
            {
                pageNumber = 1;
            }

            var words = items.OrderByDescending(x => x.Id).Skip(ConstHelper.PageSize * (pageNumber - 1)).Take(ConstHelper.PageSize);

            return Task.FromResult(new PagedList<Word>(pageNumber, ConstHelper.PageSize, totalCount, words));

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