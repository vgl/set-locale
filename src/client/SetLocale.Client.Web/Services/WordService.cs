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

        public Task<List<Word>> GetAll()
        {
            var words = _wordRepository.FindAll().ToList();
            return Task.FromResult(words);
        }
    }
}