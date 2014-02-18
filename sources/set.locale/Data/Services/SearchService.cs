using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using set.locale.Helpers;
using set.locale.Models;

namespace set.locale.Data.Services
{
    public class SearchService : BaseService, ISearchService
    {
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

            var items = Context.Words.Where(x => !x.IsDeleted);
            foreach (var key in keys)
            {
                var _key = key;
                items = items.Where(x => x.Name.Contains(_key)
                                      || x.Key.Contains(_key)
                                      || x.Description.Contains(_key)
                                      || x.Translation_AZ.Contains(_key)
                                      || x.Translation_CN.Contains(_key)
                                      || x.Translation_EN.Contains(_key)
                                      || x.Translation_FR.Contains(_key)
                                      || x.Translation_GR.Contains(_key)
                                      || x.Translation_IT.Contains(_key)
                                      || x.Translation_KZ.Contains(_key)
                                      || x.Translation_RU.Contains(_key)
                                      || x.Translation_SP.Contains(_key)
                                      || x.Translation_TK.Contains(_key)
                                      || x.Translation_TR.Contains(_key)
                     );
            }

            var list = items.OrderByDescending(x => x.CreatedAt).Skip(0).Take(10).ToList();
            foreach (var item in list)
            {
                result.Add(new SearchResult
                {
                    Name = string.Format("{0}, {1}", item.Key, item.Key.Localize()),
                    Url = string.Format("/word/detail/{0}", item.Id),
                    ImgUrl = string.Empty
                });
            }

            return Task.FromResult(result);
        }
    }

    public interface ISearchService
    {
        Task<List<SearchResult>> Query(string text);
    }
}