using System.Text;
using ServiceStack.Text;
using set.locale.Data.Entities;
using set.locale.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using set.locale.Models;

namespace set.locale.Data.Services
{
    public class TagService : BaseService, ITagService
    {
        private readonly IAppService _appService;
        private readonly IWordService _wordService;

        public TagService(IAppService appService, IWordService wordService)
        {
            _appService = appService;
            _wordService = wordService;
        }

        public Task<PagedList<Word>> GetWords(string tagUrlName, int pageNumber)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }


            var urlSlug = tagUrlName.ToUrlSlug();
            var items = Context.Words.Where(x => x.IsActive && !x.IsDeleted && x.Tags.Any(y => y.UrlName == urlSlug));


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

        public Task<List<Word>> GetWords(string tagUrlName)
        {
            var urlSlug = tagUrlName.ToUrlSlug();
            var items = Context.Words.Where(x => x.Tags.Any(y => y.UrlName == urlSlug));
            return Task.FromResult(items.ToList());
        }

        public Task<List<Tag>> GetTags()
        {
            var tags = Context.Tags.ToList();
            return Task.FromResult(tags);
        }
        public Task<List<Tag>> GetTagsByAppId(string appId)
        {
            var tags = Context.Tags.Where(x => x.AppId == appId).ToList();
            return Task.FromResult(tags);
        }

        public async Task<string> Copy(string copyFrom, string appIds, string createdBy, bool force)
        {
            int deletedCount = 0;
            StringBuilder result = new StringBuilder();
            var toAppIdList = JsonSerializer.DeserializeFromString<List<string>>(appIds);
            var fromWordsByTag = await GetWords(copyFrom);
            foreach (var appId in toAppIdList)
            {
                var app = await _appService.Get(appId);
                var words = await _wordService.GetByAppId(appId);
                int wordsCount = words.Count;

                if (force)
                {
                    deletedCount = await _wordService.DeleteByAppId(appId, createdBy);
                }
                int createCount = await _wordService.CreateList(fromWordsByTag.Select(WordModel.Map).ToList(), appId, createdBy);

                result.AppendFormat("<h4>{0}</h4>", app.Name);
                result.AppendFormat("{0}: <span class='label label-info'>{1}</span>, ", "existing_words".Localize(), wordsCount);
                result.AppendFormat("{0}: <span class='label label-danger'>{1}</span>, ", "deleted_words".Localize(), deletedCount);
                result.AppendFormat("{0}: <span class='label label-success'>{1}</span>, ", "created_words".Localize(), createCount);
                result.AppendFormat("{0}: <span class='label label-success'>{1}</span> </br>", "new_total".Localize(), (wordsCount - deletedCount) + createCount);
            }
            return await Task.FromResult(result.ToString());
        }
    }

    public interface ITagService
    {
        Task<PagedList<Word>> GetWords(string tagUrlName, int pageNumber);
        Task<List<Word>> GetWords(string tagUrlName);
        Task<List<Tag>> GetTags();
        Task<List<Tag>> GetTagsByAppId(string appId);
        Task<string> Copy(string copyFrom, string appIds, string createdBy, bool force);
    }
}