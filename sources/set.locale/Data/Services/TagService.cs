using set.locale.Data.Entities;
using set.locale.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace set.locale.Data.Services
{
    public class TagService : BaseService, ITagService
    {

        public Task<PagedList<Word>> GetWords(string tagUrlName, int pageNumber)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }


            var urlSlug = tagUrlName.ToUrlSlug();
            var items = Context.Words.Where(x => x.Tags.Any(y => y.UrlName == urlSlug));


            long totalCount = items.Count();
            var totalPageCount = (int)Math.Ceiling(totalCount / (double)ConstHelper.PageSize);


            var itemsList = new List<Word>();
            if (pageNumber <= totalPageCount)
            {
                items = items.OrderByDescending(x => x.Id).Skip(ConstHelper.PageSize * (pageNumber - 1)).Take(ConstHelper.PageSize);
                itemsList = items.ToList();
            }


            return Task.FromResult(new PagedList<Word>(pageNumber, ConstHelper.PageSize, totalCount, itemsList));

        }

        public Task<List<Tag>> GetTags()
        {
            var tags = Context.Tags.ToList();


            return Task.FromResult(tags);

        }
    }

    public interface ITagService
    {
        Task<PagedList<Word>> GetWords(string tagUrlName, int pageNumber);
        Task<List<Tag>> GetTags();
    }
}