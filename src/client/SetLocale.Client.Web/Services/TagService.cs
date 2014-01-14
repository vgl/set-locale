using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.UI;
using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Helpers;
using SetLocale.Client.Web.Repositories;

namespace SetLocale.Client.Web.Services
{
    public interface ITagService
    {
        Task<List<Word>> GetWords(string tagUrlName);
        Task<PagedList<Word>> GetWords(string tagUrlName, int pageNumber);
    }

    public class TagService : ITagService
    {
        private readonly IRepository<Word> _wordRepository;

        public TagService(
            IRepository<Word> wordRepository)
        {
            _wordRepository = wordRepository;
        }

        public Task<List<Word>> GetWords(string tagUrlName)
        {
            var words = _wordRepository.FindAll(x => x.Tags.Any(y => y.UrlName == tagUrlName)).Distinct().ToList();
            return Task.FromResult(words);
        }

        public Task<PagedList<Word>> GetWords(string tagUrlName, int pageNumber)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            var items = _wordRepository.FindAll(x => x.Tags.Any(y => y.UrlName == tagUrlName));

            long totalCount = items.Count();
            var totalPageCount = (int)Math.Ceiling(totalCount / (double)ConstHelper.PageSize);

            if (pageNumber > totalPageCount)
            {
                pageNumber = 1;
            }

            items = items.OrderByDescending(x => x.Id).Skip(ConstHelper.PageSize * (pageNumber - 1)).Take(ConstHelper.PageSize);

            return Task.FromResult(new PagedList<Word>(pageNumber, ConstHelper.PageSize, totalCount, items.ToList()));
        }
    }
}