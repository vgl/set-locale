using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Repositories;

namespace SetLocale.Client.Web.Services
{
    public interface ISearchService
    {
        Task<List<SearchResult>> Query(string text);
    }

    public class SearchService : ISearchService
    {
        private readonly IRepository<Word> _wordRepository;
        public SearchService(
            IRepository<Word> wordRepository)
        {
            _wordRepository = wordRepository;
        }

        public Task<List<SearchResult>> Query(string text)
        {
            var result = new List<SearchResult>();
            if (string.IsNullOrWhiteSpace(text))
            {
                return Task.FromResult(result);
            }

            var keys = new[] { text };
            if (text.Contains(" "))
            {
                keys = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            }

            var words = _wordRepository.Set<Word>();
            foreach (var key in keys)
            {
                var _key = key;
                words = words.Where(x => x.Key.Contains(_key)
                                        || x.Translation_TR.Contains(_key)
                                        || x.Translation_EN.Contains(_key));
            }

            var wordResults = words.OrderByDescending(x => x.Id).Skip(0).Take(10).ToList();
            foreach (var item in wordResults)
            {
                result.Add(new SearchResult
                {
                    Url = string.Format("/word/detail/{0}", item.Key),
                    Name = string.Format("{0}, {1} ...", item.Key, 
                                                         string.Format("{0}", item.Translation_EN ?? item.Translation_TR).Substring(0, 15)),
                    ImgUrl = "/public/img/word.png"
                });
            }

            return Task.FromResult(result);
        }
    }
}