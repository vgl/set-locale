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

            items = keys.Aggregate(items, (current, key) =>
                    current.Where(x => x.Name.Contains(key)
                               || x.Key.Contains(key)
                               || x.Description.Contains(key)
                               || x.Translation_AZ.Contains(key)
                               || x.Translation_CN.Contains(key)
                               || x.Translation_EN.Contains(key)
                               || x.Translation_FR.Contains(key)
                               || x.Translation_GR.Contains(key)
                               || x.Translation_IT.Contains(key)
                               || x.Translation_KZ.Contains(key)
                               || x.Translation_RU.Contains(key)
                               || x.Translation_SP.Contains(key)
                               || x.Translation_TK.Contains(key)
                               || x.Translation_TR.Contains(key)
                            ));

            var list = items.OrderByDescending(x => x.CreatedAt).Skip(0).Take(10).ToList();

            result.AddRange(list.Select(item =>
            {
                var tag = item.Tags.FirstOrDefault();
                return tag != null ? new SearchResult
                       {
                           Name = string.Format("{0}, {1}", item.Key, item.Key.Localize()),
                           Url = string.Format("/word/detail/{0}", item.Id),
                           Tag = tag.Name
                       } : null;
            }));

            return Task.FromResult(result);
        }
    }

    public interface ISearchService
    {
        Task<List<SearchResult>> Query(string text);
    }
}