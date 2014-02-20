using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

using OfficeOpenXml;
using OfficeOpenXml.Style;
using ServiceStack.Text;
using set.locale.Helpers;
using set.locale.Data.Entities;
using set.locale.Data.Services;
using set.locale.Models;

namespace set.locale.Controllers
{
    public class WordController : BaseController
    {
        private readonly IWordService _wordService;
        private readonly IAppService _appService;
        public WordController(
            IWordService wordService,
            IAppService appService)
        {
            _wordService = wordService;
            _appService = appService;
        }

        [HttpGet, AllowAnonymous]
        public async Task<ActionResult> Detail(string id)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToHome();
            ViewBag.ID = id;

            var entity = await _wordService.GetById(id);
            if (entity == null) return RedirectToHome();

            var apps = await _appService.GetByUserId(User.Identity.GetId());
            ViewBag.Apps = apps.Select(AppModel.Map);

            var model = WordModel.Map(entity);
            return View(model);
        }

        [HttpGet, AllowAnonymous]
        public async Task<ViewResult> All(int id = 0)
        {
            var pageNumber = id;
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            var items = await _wordService.GetWords(pageNumber);
            var list = items.Items.Select(WordModel.Map).ToList();
            var model = new PageModel<WordModel>
            {
                Items = list,
                HasNextPage = items.HasNextPage,
                HasPreviousPage = items.HasPreviousPage,
                Number = items.Number,
                TotalCount = items.TotalCount,
                TotalPageCount = items.TotalPageCount
            };

            return View(model);
        }

        [HttpGet]
        public async Task<ViewResult> NotTranslated(int id = 1)
        {
            var pageNumber = id;
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            var words = await _wordService.GetNotTranslated(pageNumber);
            var list = words.Items.Select(WordModel.Map).ToList();
            var model = new PageModel<WordModel>
            {
                Items = list,
                HasNextPage = words.HasNextPage,
                HasPreviousPage = words.HasPreviousPage,
                Number = words.Number,
                TotalCount = words.TotalCount,
                TotalPageCount = words.TotalPageCount
            };

            return View(model);
        }

        [HttpGet]
        public async Task<ViewResult> New()
        {
            var apps = await _appService.GetByUserId(User.Identity.GetId());
            ViewBag.Apps = apps.Select(AppModel.Map);
            var model = new WordModel();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> New(WordModel model)
        {
            SetPleaseTryAgain(model);
            var apps = await _appService.GetByUserId(User.Identity.GetId());
            ViewBag.Apps = apps.Select(AppModel.Map);

            if (model.IsNotValid())
            {
                return View(model);
            }

            model.CreatedBy = User.Identity.GetId();

            model.Tag = (await _appService.Get(model.AppId)).Name;

            var id = await _wordService.Create(model);
            if (id != null)
            {
                return Redirect("/word/detail/" + id);
            }

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<JsonResult> Translate(string id, string language, string translation)
        {
            var model = new ResponseModel { IsOk = false };

            if (string.IsNullOrEmpty(id)
                || string.IsNullOrEmpty(language))
            {
                return Json(model, JsonRequestBehavior.DenyGet);
            }

            model.IsOk = await _wordService.AddTranslate(id, language, translation);
            return Json(model, JsonRequestBehavior.DenyGet);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<JsonResult> Tag(string key, string tag)
        {
            var model = new ResponseModel { IsOk = false };

            if (string.IsNullOrEmpty(key)
                || string.IsNullOrEmpty(tag))
            {
                return Json(model, JsonRequestBehavior.DenyGet);
            }

            model.IsOk = await _wordService.Tag(key, tag);
            return Json(model, JsonRequestBehavior.DenyGet);
        }

        private async Task<string> ExportWordsToExcel()
        {
            var words = await _wordService.GetAll();
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

                //fill table data
                Func<ICollection<Tag>, string> tagsToString = (tags) =>
                {
                    var result = string.Empty;
                    foreach (var tag in tags)
                    {
                        if (string.IsNullOrEmpty(result))
                        {
                            result = tag.Name;
                        }
                        else
                        {
                            result += string.Format(", {0}", tag.Name);
                        }
                    }
                    return result;
                };


                var tagName = string.Empty;
                for (var i = 0; i < words.Count; i++)
                {
                    var row = i + 2;
                    var word = words[i];

                    workSheet.Cells[row, 1].Value = word.Key;
                    workSheet.Cells[row, 2].Value = word.Description;

                    var tags = tagsToString(word.Tags);
                    if (string.IsNullOrEmpty(tagName))
                    {
                        tagName = tags;
                    }

                    workSheet.Cells[row, 3].Value = tags;
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

                var fileName = string.Format("{0}-{1}.xlsx", tagName, DateTime.Now.ToString("s").Replace(':', '-').Replace("T", "-"));
                var filePath = string.Format("/public/files/{0}", fileName);
                var mapPath = Server.MapPath(filePath);

                System.IO.File.WriteAllBytes(mapPath, p.GetAsByteArray());

                return fileName;
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<JsonResult> Export()
        {
            var model = new ResponseModel { IsOk = false };
            var fileName = await ExportWordsToExcel();

            model.IsOk = true;
            model.Result = fileName;

            return Json(model, JsonRequestBehavior.DenyGet);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<bool> Copy(string copyFrom, string appIds, bool force)
        {
            try
            {
                var toAppIdList = JsonSerializer.DeserializeFromString<List<string>>(appIds);
                var fromWord = WordModel.Map(await _wordService.GetById(copyFrom));
                var translations = fromWord.Translations;

                foreach (var appId in toAppIdList)
                {
                    var app = await _appService.Get(appId);

                    fromWord.AppId = appId;
                    fromWord.CreatedBy = User.Identity.GetId();
                    fromWord.Tag = app.Name;

                    if (!_wordService.IsDuplicateKey(fromWord))
                    {
                        var wordId = await _wordService.Create(fromWord);
                        await _wordService.AddTranslateList(translations, wordId);
                        continue;
                    }

                    var toWord = WordModel.Map(await _wordService.GetByKey(fromWord.Key, appId));

                    if (!force)
                    {
                        ILookup<string, TranslationModel> fromWordTranslates = fromWord.Translations.ToLookup(x => x.Language.Key, x => x);
                        ILookup<string, TranslationModel> toWordTranslates = toWord.Translations.ToLookup(x => x.Language.Key, x => x);
                        var exceptLangs = fromWordTranslates.Select(x => x.Key).Except(toWordTranslates.Select(x => x.Key));
                        translations = fromWordTranslates.Where(x => exceptLangs.Contains(x.Key)).Select(x => x.First()).ToList();
                    }

                    await _wordService.AddTranslateList(translations, toWord.Id);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}