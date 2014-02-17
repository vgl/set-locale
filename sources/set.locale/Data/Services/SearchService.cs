using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            var items = Context.DomainObjects.Where(x => !x.IsDeleted);
            foreach (var key in keys)
            {
                var _key = key;
                items = items.Where(x => x.Name.Contains(_key));
            }

            var list = items.OrderByDescending(x => x.CreatedAt).Skip(0).Take(10).ToList();
            foreach (var item in list)
            {
                result.Add(new SearchResult
                {
                    Name = item.Name,
                    Url = string.Format("/domainobject/detail/{0}", item.Id),
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