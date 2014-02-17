using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using OfficeOpenXml;
using OfficeOpenXml.Style;

using SetLocale.Client.Web.Entities;
using SetLocale.Client.Web.Helpers;
using SetLocale.Client.Web.Models;
using SetLocale.Client.Web.Services;

namespace SetLocale.Client.Web.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IAppService _appService;
        private readonly IWordService _wordService;

        public AdminController(
            IUserService userService,
            IWordService wordService,
            IFormsAuthenticationService formsAuthenticationService,
            IAppService appService)
            : base(userService, formsAuthenticationService)
        {
            _appService = appService;
            _wordService = wordService;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (CurrentUser.RoleId != SetLocaleRole.Admin.Value)
            {
                filterContext.Result = RedirectToHome();
            }

            base.OnActionExecuting(filterContext);
        }

        [HttpGet]
        public ActionResult Index()
        {
            return Redirect("/admin/apps");
        }

        [HttpGet]
        public ActionResult Import()
        {
            return View(new ExcelImportModel());
        }

        [HttpPost]
        public async Task<ActionResult> Import(HttpPostedFileBase file, bool isOverWrite = false)
        {
            var model = new ExcelImportModel();

            if (file == null
                || file.ContentLength <= 0)
            {
                model.Msg = _htmlHelper.LocalizationString("please_select_file");
                return View(model);
            }

            var extension = Path.GetExtension(file.FileName);
            if (extension != ".xls"
                && extension != ".xlsx")
            {
                model.Msg = _htmlHelper.LocalizationString("please_select_excel_file");
                return View(model);
            }

            var excelName = string.Format("{0:yyyy-MM-dd}-{1}", DateTime.Now, Guid.NewGuid());
            var path = string.Format("{0}/{1}{2}", Server.MapPath("~/Public/files"), excelName, extension);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            file.SaveAs(path);

            var existingFile = new FileInfo(path);
            using (var package = new ExcelPackage(existingFile))
            {
                var workBook = package.Workbook;
                if (workBook == null
                    || workBook.Worksheets.Count <= 0)
                {
                    model.Msg = _htmlHelper.LocalizationString("excel_file_has_problems");
                    return View(model);
                }

                var currentWorksheet = workBook.Worksheets.First();
                var count = currentWorksheet.Dimension.End.Column;
                if (count <= 2)
                {
                    for (var i = 2; i < currentWorksheet.Dimension.End.Row; i++)
                    {
                        var key = currentWorksheet.Cells[i, 1].Value.ToString();
                        var tag = currentWorksheet.Cells[i, 2].Value.ToString();
                        var wordModels = new WordModel { Key = key, Tag = tag, CreatedBy = User.Identity.GetUserId() };
                        try
                        {
                            if (isOverWrite)
                            {
                                await _wordService.Update(wordModels);
                            }
                            else
                            {
                                await _wordService.Create(wordModels);
                            }
                        }
                        catch
                        {
                            model.Msg = _htmlHelper.LocalizationString("please_try_again");
                            return View(model);
                        }
                    }
                }
                else
                {
                    for (var i = 2; i < currentWorksheet.Dimension.End.Row; i++)
                    {
                        var key = currentWorksheet.Cells[i, 1].Value.ToString();
                        var desc = currentWorksheet.Cells[i, 2].Value.ToString();
                        var tag = currentWorksheet.Cells[i, 3].Value.ToString();

                        var translationTR = currentWorksheet.Cells[i, 4].Value.ToString();
                        var translationEN = currentWorksheet.Cells[i, 5].Value.ToString();
                        var translationAZ = currentWorksheet.Cells[i, 6].Value.ToString();
                        var translationCN = currentWorksheet.Cells[i, 7].Value.ToString();
                        var translationFR = currentWorksheet.Cells[i, 8].Value.ToString();
                        var translationGR = currentWorksheet.Cells[i, 9].Value.ToString();
                        var translationIT = currentWorksheet.Cells[i, 10].Value.ToString();
                        var translationKZ = currentWorksheet.Cells[i, 11].Value.ToString();
                        var translationRU = currentWorksheet.Cells[i, 12].Value.ToString();
                        var translationSP = currentWorksheet.Cells[i, 13].Value.ToString();
                        var translationTK = currentWorksheet.Cells[i, 14].Value.ToString();

                        var wordModel = new WordModel { Key = key, Description = desc, Tag = tag, CreatedBy = User.Identity.GetUserId() };

                        try
                        {
                            if (isOverWrite)
                            {
                                await _wordService.Update(wordModel);
                            }
                            else
                            {
                                await _wordService.Create(wordModel);
                            }

                            await _wordService.Translate(key, "TR", translationTR);
                            await _wordService.Translate(key, "EN", translationEN);
                            await _wordService.Translate(key, "AZ", translationAZ);
                            await _wordService.Translate(key, "CN", translationCN);
                            await _wordService.Translate(key, "FR", translationFR);
                            await _wordService.Translate(key, "GR", translationGR);
                            await _wordService.Translate(key, "IT", translationIT);
                            await _wordService.Translate(key, "KZ", translationKZ);
                            await _wordService.Translate(key, "RU", translationRU);
                            await _wordService.Translate(key, "SP", translationSP);
                            await _wordService.Translate(key, "TK", translationTK);
                        }
                        catch
                        {
                            model.Msg = _htmlHelper.LocalizationString("please_try_again");
                            return View(model);
                        }
                    }
                }

            }

            model.Msg = _htmlHelper.LocalizationString("import_successful");
            model.IsSuccess = true;

            return View(model);
        }

        [HttpGet]
        public ViewResult NewTranslator()
        {
            var model = new UserModel();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> NewTranslator(UserModel model)
        {
            if (!model.IsValidForNewTranslator())
            {
                model.Msg = "bir sorun oluştu...";
                return View(model);
            }

            model.Password = Guid.NewGuid().ToString().Replace("-", string.Empty);
            model.Language = Thread.CurrentThread.CurrentUICulture.Name;
            var userId = await _userService.Create(model, SetLocaleRole.Translator.Value);
            if (userId == null)
            {
                model.Msg = "bir sorun oluştu...";
                return View(model);
            }

            //todo:send mail to translator to welcome and ask for reset password

            return Redirect("/admin/users");
        }

        [HttpGet]
        public async Task<ActionResult> Users(int id = 0, int page = 1)
        {
            var pageNumber = page;
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            PagedList<User> users;

            ViewBag.RoleId = id;
            if (SetLocaleRole.IsValid(id))
            {
                users = await _userService.GetAllByRoleId(id, pageNumber);
            }
            else
            {
                users = await _userService.GetUsers(pageNumber);
            }

            var list = users.Items.Select(UserModel.MapUserToUserModel).ToList();

            var model = new PageModel<UserModel>
            {
                Items = list,
                HasNextPage = users.HasNextPage,
                HasPreviousPage = users.HasPreviousPage,
                Number = users.Number,
                TotalCount = users.TotalCount,
                TotalPageCount = users.TotalPageCount
            };

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Apps(int id = 0)
        {
            var pageNumber = id;
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
            var apps = await _appService.GetApps(pageNumber);

            var list = apps.Items.Select(AppModel.MapFromEntity).ToList();

            var model = new PageModel<AppModel>
            {
                Items = list,
                HasNextPage = apps.HasNextPage,
                HasPreviousPage = apps.HasPreviousPage,
                Number = apps.Number,
                TotalCount = apps.TotalCount,
                TotalPageCount = apps.TotalPageCount
            };

            return View(model);
        }

    }
}