using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Helpers;
using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Repositories;

namespace SetLocale.Client.Web.Services
{
    public interface IWordService
    {
        Task<string> Create(WordModel model);
        Task<List<Word>> GetByUserId(int userId);
        Task<Word> GetByKey(string key);
        Task<List<Word>> GetAll();
        Task<List<Word>> GetNotTranslated();
        Task<bool> Translate(string key, string language, string translation);
        Task<bool> Tag(string key, string tag);
    }

    public class WordService : IWordService
    {
        private readonly IRepository<Word> _wordRepository; 
        public WordService(IRepository<Word> wordRepository)
        {
            _wordRepository = wordRepository;
        }

        public Task<string> Create(WordModel model)
        {
            if (!model.IsValidForNew())
            {
                return null;
            }

            var slug = model.Key.ToUrlSlug();
            if (_wordRepository.Set<Word>().Any(x => x.Key == slug))
            {
                return null;
            }

            var tags = new List<Tag>();
            if (!string.IsNullOrEmpty(model.Tag))
            {
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

            _wordRepository.Create(word);
            _wordRepository.SaveChanges();

            if (word.Id < 1)
            {
                return null;
            }

            return Task.FromResult(word.Key);
        }

        public Task<List<Word>> GetByUserId(int userId)
        {
            var words = _wordRepository.FindAll(x => x.CreatedBy == userId, x => x.Tags).ToList();
            return Task.FromResult(words);
        }

        public Task<Word> GetByKey(string key)
        {
            var word = _wordRepository.FindOne(x => x.Key == key);
            return Task.FromResult(word);
        }

        public Task<List<Word>> GetNotTranslated()
        {
            var words = _wordRepository.FindAll(x => x.IsTranslated == false).ToList();
            return Task.FromResult(words);
        }

        public Task<bool> Translate(string key, string language, string translation)
        {
            if (string.IsNullOrEmpty(key)
               || string.IsNullOrEmpty(language)
               || string.IsNullOrEmpty(translation))
            {
                return Task.FromResult(false);
            }

            var word = _wordRepository.FindOne(x => x.Key == key);
            if (word == null)
            {
                return Task.FromResult(false);
            }

            var type = word.GetType();
            var propInfo = type.GetProperty(string.Format("Translation_{0}", language.ToUpperInvariant()), new Type[0]);
            propInfo.SetValue(word, translation);
            word.TranslationCount++;
            word.IsTranslated = true;

            _wordRepository.Update(word);
            _wordRepository.SaveChanges();

            return Task.FromResult(true);
        }

        public Task<bool> Tag(string key, string tagName)
        {
            if (string.IsNullOrEmpty(key)
               || string.IsNullOrEmpty(tagName))
            {
                return Task.FromResult(false);
            }

            var word = _wordRepository.FindOne(x => x.Key == key && x.Tags.Count(y => y.Name == tagName) == 0);
            if (word == null)
            {
                return Task.FromResult(false);
            }

            var tag = new Tag
           {
               Name = tagName,
               UrlName = tagName.ToUrlSlug(),
               CreatedBy = 1
           };

            word.Tags = new List<Tag> { tag };

            _wordRepository.Update(word);
            _wordRepository.SaveChanges();

            return Task.FromResult(true);
        }

        public Task<List<Word>> GetAll()
        {
            var words = _wordRepository.FindAll().ToList();
            return Task.FromResult(words);
        }
    }
}