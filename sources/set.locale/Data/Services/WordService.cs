using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using OfficeOpenXml;
using OfficeOpenXml.Style;
using ServiceStack.Text;

using set.locale.Data.Entities;
using set.locale.Helpers;
using set.locale.Models;

namespace set.locale.Data.Services
{
    public class WordService : BaseService, IWordService
    {
        private readonly IAppService _appService;
        public WordService(IAppService appService)
        {
            _appService = appService;
        }

        public Task<string> Create(WordModel model)
        {
            if (model.IsNotValid())
            {
                return null;
            }

            var slug = model.Key.ToUrlSlug();
            if (IsDuplicateKey(model))
            {
                return null;
            }

            var items = model.Tag.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            var tags = items.Select(item => new Tag
            {
                CreatedBy = model.CreatedBy,
                AppId = model.AppId,

                Name = item,
                UrlName = item.ToUrlSlug()

            }).ToList();

            var word = new Word
            {
                Key = slug,
                Description = model.Description ?? string.Empty,
                IsTranslated = false,
                TranslationCount = 0,
                CreatedBy = model.CreatedBy,
                UpdatedBy = model.CreatedBy,
                Tags = tags,

                AppId = model.AppId
            };
            Context.Words.Add(word);
            Context.Entry(word).State = EntityState.Added;

            return Context.SaveChanges() > 0 ? Task.FromResult(word.Id) : null;
        }

        public async Task<int> CreateList(List<WordModel> model, string appId, string userId)
        {
            var result = 0;
            var app = await _appService.Get(appId);
            foreach (var word in model)
            {
                word.AppId = appId;
                word.Tag = app.Name.ToUrlSlug();
                word.CreatedBy = userId;

                var task = Create(word);

                if (task == null) continue;

                var newWordId = task.Result;

                result++;
                await AddTranslateList(word.Translations, newWordId);
            }
            return await Task.FromResult(result);
        }

        public Task<bool> Delete(WordModel model)
        {
            if (model.IsNotValid())
            {
                return null;
            }

            var item = Context.Words.FirstOrDefault(x => x.Id == model.Id);
            if (item == null) return Task.FromResult(false);

            item.DeletedAt = DateTime.Now;
            item.IsDeleted = true;
            item.DeletedBy = model.CreatedBy;
            Context.Entry(item).State = EntityState.Modified;

            return Task.FromResult(Context.SaveChanges() > 0);
        }

        public Task<int> DeleteByAppId(string appId, string createdBy)
        {
            if (string.IsNullOrWhiteSpace(appId) || string.IsNullOrWhiteSpace(createdBy))
            {
                return null;
            }

            var items = Context.Words.Where(x => x.AppId == appId);
            foreach (var item in items)
            {
                item.DeletedAt = DateTime.Now;
                item.IsDeleted = true;
                item.DeletedBy = createdBy;
                Context.Entry(item).State = EntityState.Modified;
            }
            return Task.FromResult(Context.SaveChanges());
        }

        public Task<string> Update(WordModel model)
        {
            if (model.IsNotValid())
            {
                return null;
            }

            var item = Context.Words.FirstOrDefault(x => x.Id == model.Id);
            if (item == null) return Create(model);

            item.DeletedAt = DateTime.Now;
            item.IsDeleted = true;
            item.DeletedBy = model.CreatedBy;
            Context.Entry(item).State = EntityState.Modified;

            Context.SaveChanges();

            return Create(model);
        }

        public Task<PagedList<Word>> GetByUserId(string userId, int pageNumber)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            var words = Context.Words.Include(x => x.Tags).Where(x => x.CreatedBy == userId && (x.IsActive && !x.IsDeleted)).ToList();

            long totalCount = words.Count();
            var totalPageCount = (int)Math.Ceiling(totalCount / (double)ConstHelper.PageSize);

            if (pageNumber > totalPageCount)
            {
                pageNumber = 1;
            }

            words = words.OrderByDescending(x => x.Id).Skip(ConstHelper.PageSize * (pageNumber - 1)).Take(ConstHelper.PageSize).ToList();

            return Task.FromResult(new PagedList<Word>(pageNumber, ConstHelper.PageSize, totalCount, words));
        }

        public Task<Word> GetByKeyAndAppId(string key, string appId)
        {
            key = key.ToUrlSlug();
            var word = Context.Words.FirstOrDefault(x =>
                                                    x.Key == key
                                                 && x.IsActive && !x.IsDeleted
                                                 && x.AppId == appId);
            return Task.FromResult(word);
        }

        public Task<Word> GetByKeyAndAppName(string key, string appUrl)
        {
            key = key.ToUrlSlug();
            var word = Context.Words.FirstOrDefault(x =>
                                                    x.Key == key
                                                 && x.IsActive && !x.IsDeleted
                                                 && x.Name.ToUrlSlug() == appUrl);
            return Task.FromResult(word);
        }

        public Task<Word> GetById(string id)
        {
            var word = Context.Words.FirstOrDefault(x => x.Id == id && (x.IsActive && !x.IsDeleted));
            return Task.FromResult(word);
        }

        public Task<PagedList<Word>> GetWords(int pageNumber)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            var items = Context.Words.Where(x => x.IsActive && !x.IsDeleted);

            long totalCount = items.Count();
            var totalPageCount = (int)Math.Ceiling(totalCount / (double)ConstHelper.PageSize);

            if (pageNumber > totalPageCount)
            {
                pageNumber = 1;
            }

            var model = items.OrderByDescending(x => x.Id).Skip(ConstHelper.PageSize * (pageNumber - 1)).Take(ConstHelper.PageSize);

            return Task.FromResult(new PagedList<Word>(pageNumber, ConstHelper.PageSize, totalCount, model));
        }

        public Task<PagedList<Word>> GetWords(string appId, int pageNumber)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            var items = Context.Words.Where(x => x.IsActive && !x.IsDeleted && x.AppId == appId);
            long totalCount = items.Count();

            var model = items.OrderByDescending(x => x.Id).Skip(ConstHelper.PageSize * (pageNumber - 1)).Take(ConstHelper.PageSize);

            return Task.FromResult(new PagedList<Word>(pageNumber, ConstHelper.PageSize, totalCount, model));
        }

        public Task<PagedList<Word>> GetNotTranslated(int pageNumber = 1)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            var words = Context.Words.Where(x => !x.IsTranslated && (x.IsActive && !x.IsDeleted)).ToList();
            long totalCount = words.Count();

            words = words.OrderByDescending(x => x.Id).Skip(ConstHelper.PageSize * (pageNumber - 1)).Take(ConstHelper.PageSize).ToList();

            return Task.FromResult(new PagedList<Word>(pageNumber, ConstHelper.PageSize, totalCount, words));
        }

        public Task<bool> AddTranslate(string id, string language, string translation)
        {
            if (string.IsNullOrEmpty(id)
               || string.IsNullOrEmpty(language))
            {
                return Task.FromResult(false);
            }

            var word = Context.Words.FirstOrDefault(x => x.Id == id && (x.IsActive && !x.IsDeleted));
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
            Context.Entry(word).State = EntityState.Modified;

            return Task.FromResult(Context.SaveChanges() > 0);
        }

        public async Task<int> AddTranslateList(List<TranslationModel> model, string id)
        {
            var result = 0;
            foreach (var item in model)
            {
                if (await AddTranslate(id, item.Language.Key, item.Value)) result++;
            }

            return await Task.FromResult(result);
        }

        public Task<bool> Tag(string key, string tagName)
        {
            if (string.IsNullOrEmpty(key)
               || string.IsNullOrEmpty(tagName))
            {
                return Task.FromResult(false);
            }

            var word = Context.Words.FirstOrDefault(x => x.Key == key && (x.IsActive && !x.IsDeleted) && x.Tags.Count(y => y.Name == tagName) == 0);
            if (word == null)
            {
                return Task.FromResult(false);
            }

            var tag = new Tag
            {
                Name = tagName,
                UrlName = tagName.ToUrlSlug(),
            };

            word.Tags = new List<Tag> { tag };
            Context.Entry(word).State = EntityState.Modified;

            return Task.FromResult(Context.SaveChanges() > 0);
        }

        public Task<List<Word>> GetByAppId(string appId)
        {
            var words = Context.Words.Where(x => x.AppId == appId && (x.IsActive && !x.IsDeleted)).ToList();
            return Task.FromResult(words);
        }

        public Task<List<Word>> GetByAppName(string appName)
        {
            var words = Context.Words.Where(x => x.App.Name == appName && (x.IsActive && !x.IsDeleted)).ToList();
            return Task.FromResult(words);
        }
        public Task<List<Word>> GetAll()
        {
            var words = Context.Words.Where(x => x.IsActive && !x.IsDeleted).ToList();
            return Task.FromResult(words);
        }

        public bool IsDuplicateKey(WordModel model)
        {
            model.Key = model.Key.ToUrlSlug();
            return Context.Words.Any(x => x.Key == model.Key
                                          && x.IsActive && !x.IsDeleted
                                          && x.AppId == model.AppId);
        }

        public async Task<string> Copy(string copyFromWord, string appIds, string createdBy, bool force)
        {
            var addedCount = 0;
            var createCount = 0;
            var extCount = 0;
            var result = new StringBuilder();
            var toAppIdList = JsonSerializer.DeserializeFromString<List<string>>(appIds);
            var fromWord = WordModel.Map(await GetById(copyFromWord));
            var translations = fromWord.Translations;

            foreach (var appId in toAppIdList)
            {
                var app = await _appService.Get(appId);
                fromWord.AppId = appId;
                fromWord.CreatedBy = createdBy;
                fromWord.Tag = app.Name;

                var toWord = WordModel.Map(await GetByKeyAndAppId(fromWord.Key, appId));

                if (toWord == null)
                {
                    var wordId = await Create(fromWord);
                    createCount = await AddTranslateList(translations, wordId);
                }
                else if (!force)
                {
                    var fromWordTranslates = fromWord.Translations.ToLookup(x => x.Language.Key, x => x);
                    var toWordTranslates = toWord.Translations.ToLookup(x => x.Language.Key, x => x);
                    var exceptLangs = fromWordTranslates.Select(x => x.Key).Except(toWordTranslates.Select(x => x.Key));
                    translations = fromWordTranslates.Where(x => exceptLangs.Contains(x.Key)).Select(x => x.First()).ToList();
                    createCount = translations.Count;
                }
                else
                {
                    extCount = toWord.Translations.Count;
                    addedCount = await AddTranslateList(translations, toWord.Id);
                }

                result.AppendFormat("<h4>{0}</h4>", app.Name);
                result.AppendFormat("{0}: <span class='label label-info'>{1}</span>, ", "existing_translates".Localize(), extCount);
                result.AppendFormat("{0}: <span class='label label-danger'>{1}</span>, ", "added_translates".Localize(), addedCount - extCount);
                result.AppendFormat("{0}: <span class='label label-success'>{1}</span>, ", "created_translates".Localize(), createCount);
            }
            return await Task.FromResult(result.ToString());
        }

        public async Task<string> ExportWordsToExcel(string prefixName)
        {
            var words = await GetAll();
            using (var p = new ExcelPackage())
            {
                p.Workbook.Properties.Title = "exported_words".Localize();

                p.Workbook.Worksheets.Add("exported_words_sheet_name".Localize());
                var workSheet = p.Workbook.Worksheets[1];

                //display table header
                workSheet.Cells[1, 1].Value = "key".Localize();
                workSheet.Cells[1, 2].Value = "description".Localize();
                workSheet.Cells[1, 3].Value = "tags".Localize();
                workSheet.Cells[1, 4].Value = "translation_count".Localize();
                workSheet.Cells[1, 5].Value = "column_header_translation_tr".Localize();
                workSheet.Cells[1, 6].Value = "column_header_translation_en".Localize();
                workSheet.Cells[1, 7].Value = "column_header_translation_az".Localize();
                workSheet.Cells[1, 8].Value = "column_header_translation_cn".Localize();
                workSheet.Cells[1, 9].Value = "column_header_translation_fr".Localize();
                workSheet.Cells[1, 10].Value = "column_header_translation_gr".Localize();
                workSheet.Cells[1, 11].Value = "column_header_translation_it".Localize();
                workSheet.Cells[1, 12].Value = "column_header_translation_kz".Localize();
                workSheet.Cells[1, 13].Value = "column_header_translation_ru".Localize();
                workSheet.Cells[1, 14].Value = "column_header_translation_sp".Localize();
                workSheet.Cells[1, 15].Value = "column_header_translation_tk".Localize();

                //set styling of header
                workSheet.Cells[1, 1, 1, 15].Style.Font.Bold = true;
                workSheet.Cells[1, 1, 1, 15].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                for (var i = 0; i < words.Count; i++)
                {
                    var row = i + 2;
                    var word = words[i];

                    workSheet.Cells[row, 1].Value = word.Key;
                    workSheet.Cells[row, 2].Value = word.Description;
                    workSheet.Cells[row, 3].Value = string.Join(", ", word.Tags);
                    workSheet.Cells[row, 4].Value = word.TranslationCount;
                    workSheet.Cells[row, 5].Value = word.Translation_TR;
                    workSheet.Cells[row, 6].Value = word.Translation_EN;
                    workSheet.Cells[row, 7].Value = word.Translation_AZ;
                    workSheet.Cells[row, 8].Value = word.Translation_CN;
                    workSheet.Cells[row, 9].Value = word.Translation_FR;
                    workSheet.Cells[row, 10].Value = word.Translation_GR;
                    workSheet.Cells[row, 11].Value = word.Translation_IT;
                    workSheet.Cells[row, 12].Value = word.Translation_KZ;
                    workSheet.Cells[row, 13].Value = word.Translation_RU;
                    workSheet.Cells[row, 14].Value = word.Translation_SP;
                    workSheet.Cells[row, 15].Value = word.Translation_TK;
                }

                for (var i = 1; i <= 15; i++)
                {
                    workSheet.Column(i).AutoFit();
                }

                var fileName = string.Format("{0}-{1}.xls", prefixName, DateTime.Now.ToString("s").Replace(':', '-').Replace("T", "-"));
                var filePath = string.Format("/public/files/{0}", fileName);
                var mapPath = HttpContext.Current.Server.MapPath(filePath);

                File.WriteAllBytes(mapPath, p.GetAsByteArray());

                return fileName;
            }
        }
    }

    public interface IWordService
    {
        Task<string> Create(WordModel model);
        Task<int> CreateList(List<WordModel> model, string appId, string userId);
        Task<string> Update(WordModel model);
        Task<bool> Delete(WordModel model);
        Task<int> DeleteByAppId(string appId, string createdBy);
        Task<PagedList<Word>> GetByUserId(string userId, int pageNumber);
        Task<Word> GetByKeyAndAppId(string key, string appId);
        Task<Word> GetByKeyAndAppName(string key, string appUrl);
        Task<Word> GetById(string id);
        Task<PagedList<Word>> GetWords(int pageNumber);
        Task<PagedList<Word>> GetWords(string appId, int pageNumber);
        Task<PagedList<Word>> GetNotTranslated(int pageNumber);
        Task<bool> AddTranslate(string id, string language, string translation);
        Task<int> AddTranslateList(List<TranslationModel> model, string id);
        Task<bool> Tag(string key, string tag);
        Task<List<Word>> GetByAppId(string appId);
        Task<List<Word>> GetByAppName(string appName);
        Task<List<Word>> GetAll();
        bool IsDuplicateKey(WordModel model);
        Task<string> Copy(string copyFromWord, string appIds, string createdBy, bool force);
        Task<string> ExportWordsToExcel(string prefixName);
    }
}