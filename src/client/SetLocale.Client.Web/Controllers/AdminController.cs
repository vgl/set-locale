using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

using OfficeOpenXml;
using OfficeOpenXml.Style;

using SetLocale.Client.Web.Entities;
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
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Importexcel(string isOverWrite)
        {
            var excelFileBase = Request.Files["import_excel"];
            if (excelFileBase != null && excelFileBase.ContentLength > 0)
            {
                var excelFile = Request.Files["import_excel"];
                if (excelFile != null)
                {
                    string extension = Path.GetExtension(excelFile.FileName);
                    if (extension == ".xls" || extension == ".xlsx")
                    {
                        var date = DateTime.Now.Date.ToString("dd-MM-yy");
                        var excelName = date + "-" + Guid.NewGuid();

                        string path1 = string.Format("{0}/{1}", Server.MapPath("~/Public/files"), excelName + extension);

                        if (System.IO.File.Exists(path1))
                            System.IO.File.Delete(path1);

                        excelFile.SaveAs(path1);
                        var existingFile = new FileInfo(path1);

                        using (var package = new ExcelPackage(existingFile))
                        {
                            ExcelWorkbook workBook = package.Workbook;
                            if (workBook == null) return Redirect("/admin/import");
                            if (workBook.Worksheets.Count <= 0) return Redirect("/admin/import");
                            ExcelWorksheet currentWorksheet = workBook.Worksheets.First();

                            const string tR = "TR";
                            const string eN = "EN";
                            const string aZ = "AZ";
                            const string cN = "CN";
                            const string fR = "FR";
                            const string gR = "GR";
                            const string iT = "IT";
                            const string kZ = "KZ";
                            const string rU = "RU";
                            const string sP = "SP";
                            const string tK = "TK";

                            for (int i = 2; i < currentWorksheet.Dimension.End.Row; i++)
                            {

                                var key = currentWorksheet.Cells[i, 1].Value.ToString();
                                var desc = currentWorksheet.Cells[i, 2].Value.ToString();
                                var tag = currentWorksheet.Cells[i, 3].Value.ToString();

                                var translationTr = currentWorksheet.Cells[i, 4].Value.ToString();
                                var translationEn = currentWorksheet.Cells[i, 5].Value.ToString();
                                var translationAz = currentWorksheet.Cells[i, 6].Value.ToString();
                                var translationCn = currentWorksheet.Cells[i, 7].Value.ToString();
                                var translationFr = currentWorksheet.Cells[i, 8].Value.ToString();
                                var translationGr = currentWorksheet.Cells[i, 9].Value.ToString();
                                var translationIt = currentWorksheet.Cells[i, 10].Value.ToString();
                                var translationKz = currentWorksheet.Cells[i, 11].Value.ToString();
                                var translationRu = currentWorksheet.Cells[i, 12].Value.ToString();
                                var translationSp = currentWorksheet.Cells[i, 13].Value.ToString();
                                var translationTk = currentWorksheet.Cells[i, 14].Value.ToString();
                                string item;

                                try
                                {
                                    if (isOverWrite == "true")
                                    {
                                        item =
                                            await
                                                _wordService.Update(new WordModel
                                                {
                                                    Key = key,
                                                    Description = desc,
                                                    Tag = tag
                                                });
                                    }
                                    else
                                    {
                                        item =
                                            await
                                                _wordService.Create(new WordModel
                                                {
                                                    Key = key,
                                                    Description = desc,
                                                    Tag = tag
                                                });
                                    }
                                   await _wordService.Translate(key, tR, translationTr);
                                   await _wordService.Translate(key, eN, translationEn);
                                   await _wordService.Translate(key, aZ, translationAz);
                                   await _wordService.Translate(key, cN, translationCn);
                                   await _wordService.Translate(key, fR, translationFr);
                                   await _wordService.Translate(key, gR, translationGr);
                                   await _wordService.Translate(key, iT, translationIt);
                                   await _wordService.Translate(key, kZ, translationKz);
                                   await _wordService.Translate(key, rU, translationRu);
                                   await _wordService.Translate(key, sP, translationSp);
                                   await _wordService.Translate(key, tK, translationTk);
                                }
                                catch (Exception ex)
                                {
                                    //do nothing
                                }
                                finally
                                {

                                }
                            }

                        }
                    }
                }
                return Redirect("/admin/import");
            }
            return Redirect("/admin/import");
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