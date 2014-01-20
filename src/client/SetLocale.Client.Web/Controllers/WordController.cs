using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Castle.Core.Internal;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Services;
using SetLocale.Client.Web.Helpers;

namespace SetLocale.Client.Web.Controllers
{
    public class WordController : BaseController
    {
        private readonly IWordService _wordService;
        public WordController(
            IWordService wordService,
            IUserService userService, 
            IFormsAuthenticationService formsAuthenticationService) 
            : base(userService, formsAuthenticationService)
        {
            _wordService = wordService;
        }

        [HttpGet, AllowAnonymous]
        public async Task<ActionResult> Detail(string id)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToHome();

            var entity = await _wordService.GetByKey(id);           // Entity null kontrolü Test tarafında yapılmadı.
            if (entity == null) return RedirectToHome();

            var model = WordModel.MapEntityToModel(entity);
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
            var list = items.Items.Select(WordModel.MapEntityToModel).ToList();

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
        public  async Task<ViewResult> NotTranslated(int id = 1)
        {
            var pageNumber = id;
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            var words = await _wordService.GetNotTranslated(pageNumber);
            var list = words.Items.Select(WordModel.MapEntityToModel).ToList();
            
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
        public ViewResult New()
        {
            var model = new WordModel();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> New(WordModel model)
        {
            if (!model.IsValidForNew())
            {
                model.Msg = "bir sorun oluştu";
                return View(model);
            }

            model.CreatedBy = User.Identity.GetUserId();
            
            var key = await _wordService.Create(model);
            if (key == null)
            {
                model.Msg = "bir sorun oluştu, daha önce eklenmiş olabilir";
                return View(model);
            }

            return Redirect("/word/detail/" + key);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<JsonResult> Translate(string key, string language, string translation)
        {
            var model = new ResponseModel { Ok = false };

            if (string.IsNullOrEmpty(key)
                || string.IsNullOrEmpty(language)
                || string.IsNullOrEmpty(translation))
            {
                return Json(model, JsonRequestBehavior.DenyGet);
            }

            model.Ok = await _wordService.Translate(key, language, translation);
            return Json(model, JsonRequestBehavior.DenyGet);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<JsonResult> Tag(string key, string tag)
        {
            var model = new ResponseModel { Ok = false };

            if (string.IsNullOrEmpty(key)
                || string.IsNullOrEmpty(tag))
            {
                return Json(model, JsonRequestBehavior.DenyGet);
            }

            model.Ok = await _wordService.Tag(key, tag);
            return Json(model, JsonRequestBehavior.DenyGet);
        }

        private Stream CreateFile()
        {
            var words = _wordService.GetAll().Result;

            using (var p = new ExcelPackage())
            {
                p.Workbook.Properties.Title = "Exported words";

                p.Workbook.Worksheets.Add("words");
                var workSheet = p.Workbook.Worksheets[1];

                //display table header
                workSheet.Cells[1, 1].Value = _htmlHelper.LocalizationString("Key");
                workSheet.Cells[1, 2].Value = _htmlHelper.LocalizationString("Description");
                workSheet.Cells[1, 3].Value = _htmlHelper.LocalizationString("Tags");
                workSheet.Cells[1, 4].Value = _htmlHelper.LocalizationString("TranslationCount");
                workSheet.Cells[1, 5].Value = _htmlHelper.LocalizationString("Translation_TR");
                workSheet.Cells[1, 6].Value = _htmlHelper.LocalizationString("Translation_EN");
                workSheet.Cells[1, 7].Value = _htmlHelper.LocalizationString("Translation_AZ");
                workSheet.Cells[1, 8].Value = _htmlHelper.LocalizationString("Translation_CN");
                workSheet.Cells[1, 9].Value = _htmlHelper.LocalizationString("Translation_FR");
                workSheet.Cells[1, 10].Value = _htmlHelper.LocalizationString("Translation_GR");
                workSheet.Cells[1, 11].Value = _htmlHelper.LocalizationString("Translation_IT");
                workSheet.Cells[1, 12].Value = _htmlHelper.LocalizationString("Translation_KZ");
                workSheet.Cells[1, 13].Value = _htmlHelper.LocalizationString("Translation_RU");
                workSheet.Cells[1, 14].Value = _htmlHelper.LocalizationString("Translation_SP");
                workSheet.Cells[1, 15].Value = _htmlHelper.LocalizationString("Translation_TK");

                //set styling of header
                workSheet.Cells[1, 1, 1, 15].Style.Font.Bold = true;
                workSheet.Cells[1, 1, 1, 15].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //fill table data
                Func<ICollection<Tag>, string> tagsToString = (tags) =>
                {
                    var result = string.Empty;
                    foreach (var tag in tags)
                    {
                        if (result.IsNullOrEmpty())
                            result = tag.Name;
                        else
                            result += string.Format(", {0}", tag.Name);
                    }
                    return result;
                };

                for (int i = 0; i < words.Count; i++)
                {
                    var word = words[i];

                    workSheet.Cells[i + 2, 1].Value = word.Key;
                    workSheet.Cells[i + 2, 2].Value = word.Description;
                    workSheet.Cells[i + 2, 3].Value = tagsToString(word.Tags);
                    workSheet.Cells[i + 2, 4].Value = word.TranslationCount;
                    workSheet.Cells[i + 2, 5].Value = word.Translation_TR;
                    workSheet.Cells[i + 2, 6].Value = word.Translation_EN;
                    workSheet.Cells[i + 2, 7].Value = word.Translation_AZ;
                    workSheet.Cells[i + 2, 8].Value = word.Translation_CN;
                    workSheet.Cells[i + 2, 9].Value = word.Translation_FR;
                    workSheet.Cells[i + 2, 10].Value = word.Translation_GR;
                    workSheet.Cells[i + 2, 11].Value = word.Translation_IT;
                    workSheet.Cells[i + 2, 12].Value = word.Translation_KZ;
                    workSheet.Cells[i + 2, 13].Value = word.Translation_RU;
                    workSheet.Cells[i + 2, 14].Value = word.Translation_SP;
                    workSheet.Cells[i + 2, 15].Value = word.Translation_TK;
                }

                //set autofit of the columns
                for (int i = 1; i <= 15; i++)
                    workSheet.Column(i).AutoFit();

                var fileData = p.GetAsByteArray();
                

                //var file = File(fileData, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

                return new FileStreamResult(new MemoryStream(fileData), System.Net.Mime.MediaTypeNames.Application.Octet).FileStream;
            }
        }

        [HttpPost, AllowAnonymous]
        public ActionResult Export()
        {
            //adding cookie
            /*var newcookie = new HttpCookie("fileDownloadToken", "token");
            newcookie.Expires = DateTime.UtcNow.AddDays(3);
            Response.SetCookie(newcookie);*/

            Thread.Sleep(new TimeSpan(0,0,0,1));
            var file = CreateFile();
            var fileName = Guid.NewGuid() + ".xlsx";
            return File(file, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}